using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance.Buffers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Entities.Payloads;

namespace WumpWump.Net.Gateway.Modules
{
    public class DiscordGatewayMessageModule : IDiscordGatewayMessageModule
    {
        public JsonSerializerOptions JsonSerializerOptions { get; init; }

        protected ClientWebSocket _webSocket = new();
        protected CancellationTokenSource _cancellationTokenSource = new();
        protected readonly HttpMessageInvoker _httpMessageInvoker;
        protected readonly ILogger<DiscordGatewayMessageModule> _logger;

        protected Task? _receiveTask;
        protected readonly Channel<IDiscordGatewayPayload> _receiveChannel = Channel.CreateUnboundedPrioritized<IDiscordGatewayPayload>(new()
        {
            SingleReader = true,
            SingleWriter = true,
            AllowSynchronousContinuations = false,
            Comparer = new DiscordGatewayPayloadComparer()
        });

        protected Task? _parseTask;
        protected readonly Pipe _webSocketParsePipe = new();

        protected Task? _sendTask;
        protected readonly Channel<IDiscordGatewayPayload> _sendChannel = Channel.CreateUnboundedPrioritized<IDiscordGatewayPayload>(new()
        {
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false,
            Comparer = new DiscordGatewayPayloadComparer()
        });

        public DiscordGatewayMessageModule([FromKeyedServices("WumpWump.Net.Gateway")] JsonSerializerOptions jsonSerializerOptions, [FromKeyedServices("WumpWump.Net.Gateway")] HttpMessageInvoker httpMessageInvoker, ILogger<DiscordGatewayMessageModule>? logger = null)
        {
            JsonSerializerOptions = jsonSerializerOptions;
            _httpMessageInvoker = httpMessageInvoker;
            _logger = logger ?? NullLogger<DiscordGatewayMessageModule>.Instance;
        }

        public async ValueTask ConnectAsync(Uri gatewayUrl, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (IsConnected())
            {
                throw new InvalidOperationException("Client is already connected or in the process of connecting.");
            }

            ArgumentNullException.ThrowIfNull(gatewayUrl, nameof(gatewayUrl));
            if (_webSocket.State is not WebSocketState.None)
            {
                // ClientWebsocket cannot be reused, so when
                // we're reconnecting we must create a new one.
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                _webSocketParsePipe.Reset();
                _webSocket.Dispose();
                _webSocket = new ClientWebSocket();
            }

            // At the time of writing, the Discord gateway does not support
            // Http/2 (more specifically, does not support the extended CONNECT
            // feaature required by WebSockets) so we must use Http/1.1.
            // If this ever changes in the future, the user should be able to special
            // case the Discord.com domain by passing a custom HttpMessageInvoker through
            // the constructor.
            _webSocket.Options.HttpVersion = HttpVersion.Version11;
            _webSocket.Options.HttpVersionPolicy = HttpVersionPolicy.RequestVersionExact;
            _logger.LogDebug("Connecting to '{GatewayUrl}'...", gatewayUrl);
            await _webSocket.ConnectAsync(gatewayUrl, _httpMessageInvoker, cancellationToken);
            _logger.LogInformation("Connected to '{GatewayUrl}'", gatewayUrl);
            _receiveTask = ReceiveAsync();
            _parseTask = ParseAsync();
            _sendTask = SendAsync();
        }

        public async ValueTask DisconnectAsync(WebSocketCloseStatus? webSocketCloseStatus = null, string? webSocketCloseDescription = null, CancellationToken cancellationToken = default)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogDebug("Disconnecting from the gateway...");
            if (webSocketCloseStatus is not null && IsConnected())
            {
                await _webSocket.CloseAsync(webSocketCloseStatus.Value, webSocketCloseDescription, cancellationToken);
                _logger.LogInformation("Disconnected from the Gateway with close code {CloseCode} ({}) and reason: '{Reason}'", (int)webSocketCloseStatus.Value, webSocketCloseStatus, webSocketCloseDescription);
            }
            else
            {
                _webSocket.Abort();
                _logger.LogInformation("Disconnected from the Gateway by closing the underlying TCP Connection.");
            }

            _logger.LogDebug("Stopping all tasks...");
            await _cancellationTokenSource.CancelAsync();
            await _receiveTask!;
            await _parseTask!;
            await _sendTask!;
            _logger.LogDebug("Stopped all tasks.");
        }

        public bool IsConnected()
            => _webSocket.State is not WebSocketState.None and not WebSocketState.CloseSent and not WebSocketState.CloseReceived and not WebSocketState.Closed and not WebSocketState.Aborted;

        public async ValueTask<IDiscordGatewayPayload> ReadAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_receiveChannel.Reader.Count == 0 && !IsConnected())
            {
                // We call DisconnectAsync to clean up the other resources.
                await DisconnectAsync(null, null, CancellationToken.None);
                throw new InvalidOperationException("Client is not connected to the gateway.");
            }
            else if (_receiveChannel.Reader.TryRead(out IDiscordGatewayPayload? payload))
            {
                return payload;
            }

            await _receiveChannel.Reader.WaitToReadAsync(cancellationToken);
            return await _receiveChannel.Reader.ReadAsync(cancellationToken);
        }

        public async ValueTask WriteAsync(IDiscordGatewayPayload payload, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!IsConnected())
            {
                // We call DisconnectAsync to clean up the other resources.
                await DisconnectAsync(null, null, CancellationToken.None);
                throw new InvalidOperationException("Client is not connected to the gateway.");
            }
            else if (_sendChannel.Writer.TryWrite(payload))
            {
                return;
            }

            await _sendChannel.Writer.WaitToWriteAsync(cancellationToken);
            await _sendChannel.Writer.WriteAsync(payload, cancellationToken);
        }

        protected async Task ReceiveAsync()
        {
            if (!IsConnected())
            {
                throw new InvalidOperationException("Client is not connected to the gateway.");
            }

            _logger.LogDebug("Started receiving messages from the gateway.");
            while (!_cancellationTokenSource.IsCancellationRequested && IsConnected())
            {
                try
                {
                    // Receive the message
                    Memory<byte> buffer = _webSocketParsePipe.Writer.GetMemory(4096);
                    ValueWebSocketReceiveResult webSocketResult = await _webSocket.ReceiveAsync(buffer, _cancellationTokenSource.Token);

                    // Should be noted that this will hardly ever happen, if at all
                    // due to https://github.com/discord/discord-api-docs/issues/6011
                    if (webSocketResult.MessageType == WebSocketMessageType.Close)
                    {
                        _logger.LogDebug("WebSocket closed with state {State} {Description}", _webSocket.CloseStatus, _webSocket.CloseStatusDescription);
                        _webSocketParsePipe.Writer.CancelPendingFlush();
                        break;
                    }

                    // Save the bytes received to the pipe
                    _webSocketParsePipe.Writer.Advance(webSocketResult.Count);

                    // If the message wasn't fully received, continue reading
                    if (!webSocketResult.EndOfMessage)
                    {
                        continue;
                    }

                    // Flush the pipe to process the message
                    await _webSocketParsePipe.Writer.FlushAsync(_cancellationTokenSource.Token);
                }
                catch (Exception error)
                {
                    _webSocketParsePipe.Writer.CancelPendingFlush();
                    if (_cancellationTokenSource.IsCancellationRequested || error is OperationCanceledException)
                    {
                        // Client is disconnecting
                        break;
                    }
                    else if (!IsConnected())
                    {
                        // Discord disconnected us. ReadAsync/WriteAsync will finish
                        // processing previous messages and then throw once the
                        // channels are empty. Ideally then a different module will
                        // attempt to reconnect.
                        _logger.LogDebug("Discord disconnected us.");
                        break;
                    }

                    _logger.LogError(error, "An error occurred while receiving a message from the gateway.");
                    continue;
                }
            }

            await _webSocketParsePipe.Writer.CompleteAsync();
            _logger.LogDebug("Stopped receiving messages from the gateway.");
        }

        protected async Task ParseAsync()
        {
            _logger.LogDebug("Started parsing messages from the gateway.");
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    // Dequeue the raw message from the pipe
                    ReadResult readResult = await _webSocketParsePipe.Reader.ReadAsync(_cancellationTokenSource.Token);
                    if (readResult.IsCanceled)
                    {
                        // Cancelled when we cannot preform websocket operations
                        break;
                    }
                    else if (readResult.Buffer.IsEmpty)
                    {
                        // No data to process
                        _webSocketParsePipe.Reader.AdvanceTo(readResult.Buffer.Start, readResult.Buffer.End);
                        continue;
                    }
                    else if (_logger.IsEnabled(LogLevel.Trace))
                    {
                        // readResult.Buffer.ToArray is extremely expensive.
                        _logger.LogTrace("Received raw payload: {Payload}", Encoding.UTF8.GetString(readResult.Buffer.ToArray()));
                    }

                    // Deserialize the message
                    IDiscordGatewayPayload? payload = DeserializePayload(ref readResult, out long bytesConsumed);

                    // Not enough data to deserialize the message
                    if (payload is null)
                    {
                        _webSocketParsePipe.Reader.AdvanceTo(readResult.Buffer.Start, readResult.Buffer.End);
                        continue;
                    }

                    // Process the message
                    _webSocketParsePipe.Reader.AdvanceTo(readResult.Buffer.Slice(readResult.Buffer.Start, bytesConsumed).End);
                    if (!_receiveChannel.Writer.TryWrite(payload))
                    {
                        await _receiveChannel.Writer.WaitToWriteAsync(_cancellationTokenSource.Token);
                        await _receiveChannel.Writer.WriteAsync(payload, _cancellationTokenSource.Token);
                    }

                    // Break the loop if needed
                    if (readResult.IsCompleted)
                    {
                        break;
                    }
                }
                catch (Exception error)
                {
                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        break;
                    }

                    _logger.LogError(error, "An error occurred while processing a message from the gateway.");
                    continue;
                }
            }

            await _webSocketParsePipe.Reader.CompleteAsync();
            _logger.LogDebug("Stopped parsing messages from the gateway.");
        }

        protected async Task SendAsync()
        {
            using ArrayPoolBufferWriter<byte> bufferWriter = new(ArrayPool<byte>.Shared, 256);
            try
            {
                while (await _sendChannel.Reader.WaitToReadAsync(_cancellationTokenSource.Token))
                {
                    while (_sendChannel.Reader.TryRead(out IDiscordGatewayPayload? payload))
                    {
                        if (_cancellationTokenSource.IsCancellationRequested || !IsConnected())
                        {
                            // Client is no longer sending messages
                            break;
                        }

                        // Serialize the message
                        WebSocketMessageType messageType = SerializePayload(payload, bufferWriter);

                        // Send the message
                        await _webSocket.SendAsync(bufferWriter.WrittenMemory[..bufferWriter.WrittenCount], messageType, true, _cancellationTokenSource.Token);

                        // Print the raw payload to the logger, if enabled
                        if (_logger.IsEnabled(LogLevel.Trace))
                        {
                            _logger.LogTrace("Sent payload: {Payload}", Encoding.UTF8.GetString(bufferWriter.WrittenSpan));
                        }

                        // Advance the reader
                        bufferWriter.Clear();
                    }
                }
            }
            catch (Exception error)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    // Client is disconnecting
                    return;
                }
                else if (!IsConnected())
                {
                    // Discord disconnected us. ReadAsync/WriteAsync will finish
                    // processing previous messages and then throw once the
                    // channels are empty. Ideally then a different module will
                    // attempt to reconnect.
                    _logger.LogWarning("Discord disconnected us.");
                    return;
                }

                _logger.LogError(error, "An error occurred while sending a message to the gateway.");
            }

            // Consume the remaining send messages since reconnecting
            // may cause us to send messages that are no longer valid.
            while (_sendChannel.Reader.TryRead(out IDiscordGatewayPayload? payload))
            {
                _logger.LogWarning("Discarding message that was not sent to the Gateway: {Payload}", payload);
            }

            _logger.LogDebug("Stopped sending gateway messages.");
        }

        protected virtual IDiscordGatewayPayload? DeserializePayload(ref ReadResult readResult, out long bytesConsumed)
        {
            // Deserialize the message
            Utf8JsonReader reader = new(readResult.Buffer);
            IDiscordGatewayPayload? payload = JsonSerializer.Deserialize<IDiscordGatewayPayload>(ref reader, JsonSerializerOptions);
            bytesConsumed = reader.BytesConsumed;
            return payload;
        }

        protected virtual WebSocketMessageType SerializePayload(IDiscordGatewayPayload payload, ArrayPoolBufferWriter<byte> bufferWriter)
        {
            // Serialize the message
            using Utf8JsonWriter writer = new(bufferWriter);
            JsonSerializer.Serialize(writer, payload, JsonSerializerOptions);
            writer.Flush();
            return WebSocketMessageType.Text;
        }

        public async ValueTask DisposeAsync()
        {
            if (_webSocket is null)
            {
                return;
            }

            await DisconnectAsync(null, null, CancellationToken.None);
            _cancellationTokenSource.Dispose();
            _webSocket.Dispose();
            _webSocketParsePipe.Writer.Complete();
            _webSocketParsePipe.Reader.Complete();
            _receiveChannel.Writer.Complete();
            _sendChannel.Writer.Complete();
            _receiveTask?.Dispose();
            _parseTask?.Dispose();
            _sendTask?.Dispose();
            _receiveTask = null;
            _parseTask = null;
            _sendTask = null;
            _webSocket = null!;
            GC.SuppressFinalize(this);
        }
    }
}
