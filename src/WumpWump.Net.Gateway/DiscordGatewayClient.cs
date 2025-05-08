using System;
using System.Buffers;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance.Buffers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OoLunar.AsyncEvents;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Events.EventArgs;
using WumpWump.Net.Gateway.Events.EventHandlers;
using WumpWump.Net.Gateway.Json;
using WumpWump.Net.Gateway.Payloads;
using WumpWump.Net.Rest;
using WumpWump.Net.Rest.Entities;
using WumpWump.Net.Rest.Entities.Gateway;

namespace WumpWump.Net.Gateway
{
    public class DiscordGatewayClient : IAsyncDisposable
    {
        public AsyncEventContainer AsyncEventContainer { get; init; }
        public JsonSerializerOptions DefaultSerializerOptions { get; init; }
        public DiscordRestClient RestClient { get; init; }
        public DiscordGatewayInformation? GatewayInformation { get; private set; }
        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public ulong? LastSequenceReceived => _lastSequenceReceived;
        public bool IsRateLimited => DateTimeOffset.UtcNow < RateLimitResetsAt;
        public DateTimeOffset RateLimitResetsAt => _lastPayloadSent.AddSeconds(1);

        protected readonly Channel<IDiscordGatewayPayload> _messageReceiveChannel = Channel.CreateUnboundedPrioritized<IDiscordGatewayPayload>(new()
        {
            SingleReader = true,
            SingleWriter = true,
            AllowSynchronousContinuations = false,
            Comparer = new DiscordGatewayPayloadComparer()
        });

        protected readonly Channel<IDiscordGatewayPayload> _messageSendChannel = Channel.CreateUnboundedPrioritized<IDiscordGatewayPayload>(new()
        {
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false,
            Comparer = new DiscordGatewayPayloadComparer()
        });

        protected readonly ILogger<DiscordGatewayClient> _logger;
        protected readonly FrozenDictionary<Type, DiscordGatewayEventArgsFactory> _payloadToEventArgs;
        protected CancellationTokenSource _cancellationTokenSource = new();
        protected Pipe _websocketMessagePipe = new();
        protected ClientWebSocket? _webSocket;
        protected ulong? _lastSequenceReceived;
        protected Task? _websocketReadTask;
        protected Task? _websocketParseTask;
        protected Task? _invokeEventTask;

        protected Task? _websocketSendTask;
        protected DateTimeOffset _lastPayloadSent = DateTimeOffset.UtcNow;
        protected DateTimeOffset _otherLastPayloadSent = DateTimeOffset.UtcNow;

        public DiscordGatewayClient(DiscordRestClient restClient, DiscordGatewayEventRegistration eventRegistrations, AsyncEventContainer? asyncEventContainer = null, ILogger<DiscordGatewayClient>? logger = null)
        {
            RestClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
            _logger = logger ?? NullLogger<DiscordGatewayClient>.Instance;

            if (asyncEventContainer is null)
            {
                asyncEventContainer = new AsyncEventContainer();
                asyncEventContainer.AddHandlers<DiscordGatewayConnectionHandler>(new DiscordGatewayConnectionHandler(NullLogger<DiscordGatewayConnectionHandler>.Instance));
                asyncEventContainer.AddHandlers<DiscordGatewayHeartbeatHandler>(new DiscordGatewayHeartbeatHandler(NullLogger<DiscordGatewayHeartbeatHandler>.Instance));
            }

            AsyncEventContainer = asyncEventContainer;
            DefaultSerializerOptions = new(DiscordRestClient.DefaultSerializerOptions);
            DefaultSerializerOptions.Converters.Add(new DiscordGatewayPayloadJsonConverter(eventRegistrations));

            Dictionary<Type, DiscordGatewayEventArgsFactory> payloadToEventArgs = [];
            foreach (DiscordGatewayEventTicket registration in eventRegistrations)
            {
                payloadToEventArgs.Add(registration.EventType, registration.CreateGatewayEventArgs);
            }

            _payloadToEventArgs = payloadToEventArgs.ToFrozenDictionary();
        }

        public async ValueTask ConnectAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_webSocket?.State is not null and not WebSocketState.None and not WebSocketState.Closed and not WebSocketState.Aborted)
            {
                throw new InvalidOperationException("Client is already connected or in the process of connecting.");
            }

            DiscordApiResponse<DiscordGatewayInformation> response = await RestClient.GetGatewayBotInformationAsync(cancellationToken);
            response.EnsureSuccess();
            await ConnectAsync(response.Data, cancellationToken);
        }

        public async ValueTask ConnectAsync(DiscordGatewayInformation gatewayInformation, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(gatewayInformation, nameof(gatewayInformation));
            cancellationToken.ThrowIfCancellationRequested();
            if (_webSocket?.State is not null and not WebSocketState.None and not WebSocketState.Closed and not WebSocketState.Aborted)
            {
                throw new InvalidOperationException("Client is already connected or in the process of connecting.");
            }

            GatewayInformation = gatewayInformation;

            _logger.LogTrace("Connecting to Discord Gateway at {Url}", gatewayInformation.Url);
            _webSocket = new ClientWebSocket();
            await _webSocket.ConnectAsync(new Uri(gatewayInformation.Url, "?v=10&encoding=json"), cancellationToken);
            _logger.LogDebug("Connected to Discord Gateway at {Url}", gatewayInformation.Url);
            _websocketSendTask = SendGatewayMessagesAsync();
            _websocketReadTask = ReceiveGatewayMessagesAsync();
            _websocketParseTask = ParseGatewayMessagesAsync();
            _invokeEventTask = InvokeGatewayEventsAsync();
        }

        public async ValueTask ReconnectAsync(bool forceDisconnect, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await DisconnectAsync(forceDisconnect, cancellationToken);
            await ConnectAsync(cancellationToken);
        }

        public async ValueTask ReconnectAsync(bool forceDisconnect, Uri? resumeUrl = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await DisconnectAsync(forceDisconnect, cancellationToken);
            if (GatewayInformation is null)
            {
                DiscordApiResponse<DiscordGatewayInformation> response = await RestClient.GetGatewayBotInformationAsync(cancellationToken);
                response.EnsureSuccess();
                GatewayInformation = response.Data;
            }

            if (resumeUrl is null)
            {
                await ConnectAsync(GatewayInformation, cancellationToken);
            }
            else
            {
                await ConnectAsync(GatewayInformation with
                {
                    Url = resumeUrl
                }, cancellationToken);
            }
        }

        public async ValueTask DisconnectAsync(bool forceDisconnect, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            await _cancellationTokenSource.CancelAsync();
            if (forceDisconnect && _webSocket is not null)
            {
                _webSocket.Abort();
            }
            else if (_webSocket?.State is not null and (WebSocketState.Open or WebSocketState.CloseSent or WebSocketState.CloseReceived))
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed connection", cancellationToken);
            }

            await _websocketParseTask!;
            await _websocketReadTask!;
            await _websocketSendTask!;
            await _invokeEventTask!;
            _websocketMessagePipe.Reset();
            _lastSequenceReceived = null;
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async ValueTask SendGatewayPayloadAsync<T>(DiscordGatewayOpCode opCode, T data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _messageSendChannel.Writer.WaitToWriteAsync(cancellationToken);
            await _messageSendChannel.Writer.WriteAsync(new DiscordGatewayPayload<T>()
            {
                OpCode = opCode,
                Data = data,
                Sequence = null,
                EventName = null
            }, cancellationToken);
        }

        public async ValueTask QueueEventAsync(IDiscordGatewayPayload payload, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payload);
            await _messageReceiveChannel.Writer.WaitToWriteAsync(cancellationToken);
            await _messageReceiveChannel.Writer.WriteAsync(payload, cancellationToken);
        }

        public async ValueTask<DiscordOptional<bool>> InvokeEventAsync(DiscordGatewayAsyncEventArgs args, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(args);
            cancellationToken.ThrowIfCancellationRequested();

            IAsyncEvent asyncEvent = AsyncEventContainer.GetAsyncEvent(args.GetType());

            try
            {
                // Since the event is being invoked by the gateway client,
                // it should be cancelled if the client disconnects.
                CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);
                return await asyncEvent.InvokeAsync(args, cancellationTokenSource.Token);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "An error occurred while invoking OpCode {OpCode} {EventName}", args.Payload.OpCode, args.Payload.GetType().Name);
                if (Debugger.IsAttached)
                {
                    Debugger.BreakForUserUnhandledException(error);
                }

                return DiscordOptional<bool>.Empty;
            }
        }

        protected async Task ReceiveGatewayMessagesAsync()
        {
            if (_webSocket is null)
            {
                throw new InvalidOperationException("WebSocket is not connected.");
            }

            _logger.LogDebug("Started receiving messages from Discord Gateway.");
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    // Receive the message
                    Memory<byte> buffer = _websocketMessagePipe.Writer.GetMemory(4096);
                    ValueWebSocketReceiveResult webSocketResult = await _webSocket.ReceiveAsync(buffer, _cancellationTokenSource.Token);

                    // Should be noted that this will hardly ever happen, if at all
                    // due to https://github.com/discord/discord-api-docs/issues/6011
                    if (webSocketResult.MessageType == WebSocketMessageType.Close)
                    {
                        _logger.LogDebug("WebSocket closed with state {State} {Description}", _webSocket.CloseStatus, _webSocket.CloseStatusDescription);
                        _websocketMessagePipe.Writer.CancelPendingFlush();
                        break;
                    }

                    // Save the bytes received to the pipe
                    _websocketMessagePipe.Writer.Advance(webSocketResult.Count);

                    // If the message wasn't fully received, continue reading
                    if (!webSocketResult.EndOfMessage)
                    {
                        continue;
                    }

                    // Flush the pipe to process the message
                    await _websocketMessagePipe.Writer.FlushAsync(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    // Client is disconnecting
                    break;
                }
                catch (WebSocketException)
                {
                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        // Client is disconnecting
                        break;
                    }

                    // Discord disconnected us, which means we need to reconnect
                    _logger.LogWarning("Discord disconnected us. Attempting to reconnect...");
                    _ = InvokeEventAsync(new()
                    {
                        Client = this,
                        Payload = new DiscordGatewayPayload<DiscordGatewayReconnectPayload?>()
                        {
                            Data = null,
                            OpCode = DiscordGatewayOpCode.Reconnect,
                            EventName = null,
                            Sequence = null
                        }
                    }, CancellationToken.None).AsTask();

                    break;
                }
                catch (Exception error)
                {
                    _logger.LogError(error, "An error occurred while receiving a message from the Discord Gateway.");
                    continue;
                }
            }

            await _websocketMessagePipe.Writer.CompleteAsync();
            if (_webSocket.State is WebSocketState.Open or WebSocketState.CloseSent or WebSocketState.CloseReceived)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed connection", _cancellationTokenSource.Token);
            }

            _logger.LogDebug("Stopped receiving messages from Discord Gateway.");
        }

        protected async Task ParseGatewayMessagesAsync()
        {
            _logger.LogDebug("Started processing gateway events.");
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    // Dequeue the raw message from the pipe
                    ReadResult readResult = await _websocketMessagePipe.Reader.ReadAsync(_cancellationTokenSource.Token);
                    if (readResult.Buffer.IsEmpty)
                    {
                        continue;
                    }
                    else if (_logger.IsEnabled(LogLevel.Trace))
                    {
                        // readResult.Buffer.ToArray is extremely expensive.
                        _logger.LogTrace("Received raw payload: {Payload}", Encoding.UTF8.GetString(readResult.Buffer.ToArray()));
                    }

                    // Deserialize the message
                    Utf8JsonReader reader = new(readResult.Buffer);
                    IDiscordGatewayPayload? payload = JsonSerializer.Deserialize<IDiscordGatewayPayload>(ref reader, DefaultSerializerOptions);

                    // Not enough data to deserialize the message
                    if (payload is null)
                    {
                        _websocketMessagePipe.Reader.AdvanceTo(readResult.Buffer.Start, readResult.Buffer.End);
                        continue;
                    }

                    // Process the message
                    _websocketMessagePipe.Reader.AdvanceTo(readResult.Buffer.Slice(readResult.Buffer.Start, reader.BytesConsumed).End);
                    await _messageReceiveChannel.Writer.WaitToWriteAsync(_cancellationTokenSource.Token);
                    await _messageReceiveChannel.Writer.WriteAsync(payload, _cancellationTokenSource.Token);
                    if (payload.Sequence is not null && (_lastSequenceReceived is null || payload.Sequence.Value > _lastSequenceReceived))
                    {
                        _lastSequenceReceived = payload.Sequence.Value;
                    }

                    // Break the loop if needed
                    if (readResult.IsCompleted)
                    {
                        break;
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            await _websocketMessagePipe.Reader.CompleteAsync();
            _logger.LogDebug("Stopped processing gateway events.");
        }

        protected async Task InvokeGatewayEventsAsync()
        {
            _logger.LogDebug("Started gateway event invocation loop.");
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                IDiscordGatewayPayload payload;
                try
                {
                    await _messageReceiveChannel.Reader.WaitToReadAsync(_cancellationTokenSource.Token);
                    payload = await _messageReceiveChannel.Reader.ReadAsync(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                try
                {
                    if (!_payloadToEventArgs.TryGetValue(payload.GetType().GenericTypeArguments[0], out DiscordGatewayEventArgsFactory? eventArgsFunc))
                    {
                        eventArgsFunc = _payloadToEventArgs[typeof(DiscordGatewayUnknownPayload)];
                    }
                    ;

                    DiscordGatewayAsyncEventArgs eventArgs = eventArgsFunc(this, payload);
                    IAsyncEvent asyncEvent = AsyncEventContainer.GetAsyncEvent(eventArgs.GetType());
                    await asyncEvent.InvokeAsync(eventArgs, _cancellationTokenSource.Token);
                }
                catch (Exception error)
                {
                    _logger.LogError(error, "An error occurred while invoking OpCode {OpCodeInt} ({OpCode}) with Event Name \"{EventName}\"", (int)payload.OpCode, payload.OpCode, payload.EventName);
                    if (Debugger.IsAttached)
                    {
                        Debugger.BreakForUserUnhandledException(error);
                    }

                    continue;
                }
            }

            _logger.LogDebug("Stopped gateway event invocation loop.");
        }

        protected async Task SendGatewayMessagesAsync()
        {
            if (_webSocket is null)
            {
                throw new InvalidOperationException("WebSocket is not connected.");
            }

            _logger.LogDebug("Started sending gateway events to Discord.");
            try
            {
                using ArrayPoolBufferWriter<byte> bufferWriter = new(ArrayPool<byte>.Shared, 256);
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    // Wait for the next message from the bot
                    await _messageSendChannel.Reader.WaitToReadAsync(_cancellationTokenSource.Token);

                    // Check if we can send messages before dequeuing
                    if (_webSocket.State is not WebSocketState.Open)
                    {
                        break;
                    }

                    // Check if we are rate limited
                    if (IsRateLimited)
                    {
                        _logger.LogWarning("Rate limited. Waiting for {Time}ms.", RateLimitResetsAt.Subtract(DateTimeOffset.UtcNow).TotalMilliseconds);
                        await Task.Delay(RateLimitResetsAt.Subtract(DateTimeOffset.UtcNow), _cancellationTokenSource.Token);
                    }

                    // Dequeue the message from the channel
                    IDiscordGatewayPayload payload = await _messageSendChannel.Reader.ReadAsync(_cancellationTokenSource.Token);
                    await using Utf8JsonWriter writer = new(bufferWriter);
                    JsonSerializer.Serialize(writer, payload, DefaultSerializerOptions);
                    await writer.FlushAsync(_cancellationTokenSource.Token);

                    // Send the message
                    await _webSocket.SendAsync(bufferWriter.WrittenMemory[..bufferWriter.WrittenCount], WebSocketMessageType.Text, true, _cancellationTokenSource.Token);

                    // Print the raw payload to the logger, if enabled
                    if (_logger.IsEnabled(LogLevel.Trace))
                    {
                        _logger.LogTrace("Sent payload: {Payload}", Encoding.UTF8.GetString(bufferWriter.WrittenSpan));
                    }

                    // Advance the reader
                    bufferWriter.Clear();
                }
            }
            catch (OperationCanceledException) { }
            catch (WebSocketException)
            {
                // If another task is shutting down the
                // websocket, do not try to reconnect twice.
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }

                _ = InvokeEventAsync(new()
                {
                    Client = this,
                    Payload = new DiscordGatewayPayload<DiscordGatewayReconnectPayload>()
                    {
                        Data = new()
                        {
                            ManualDisconnect = true
                        },
                        OpCode = DiscordGatewayOpCode.Reconnect,
                        EventName = null,
                        Sequence = null
                    }
                }, CancellationToken.None).AsTask();

                return;
            }
            catch (Exception error)
            {
                _logger.LogError(error, "An error occurred while sending a message to the Discord Gateway.");
            }

            _logger.LogDebug("Stopped sending gateway events to Discord.");
        }

        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync(true, CancellationToken.None);
            _webSocket?.Dispose();
            _websocketParseTask = null;
            _websocketReadTask = null;
            _invokeEventTask = null;
            _websocketSendTask = null;
            GC.SuppressFinalize(this);
        }
    }
}
