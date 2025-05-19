using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Entities.Payloads;
using WumpWump.Net.Gateway.Modules;
using WumpWump.Net.Rest;
using WumpWump.Net.Rest.Entities.Gateway;

namespace WumpWump.Net.Gateway
{
    public class DiscordGatewayClient
    {
        public DiscordRestClient RestClient { get; init; }
        public IDiscordGatewayMessageModule MessageModule { get; init; }
        public IDiscordGatewayEventModule EventModule { get; init; }
        public DiscordGatewaySessionInformation SessionInformation { get; protected set; } = new()
        {
            GatewayInformation = null!,
            ResumeUrl = null!
        };

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        protected readonly ILogger<DiscordGatewayClient> _logger;
        protected CancellationTokenSource _cancellationTokenSource = new();
        protected Task? _eventProcessingTask;

        public DiscordGatewayClient(DiscordRestClient restClient, IDiscordGatewayEventModule gatewayEventModule, IDiscordGatewayMessageModule gatewayMessageModule, ILogger<DiscordGatewayClient>? logger = null)
        {
            RestClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
            EventModule = gatewayEventModule ?? throw new ArgumentNullException(nameof(gatewayEventModule));
            MessageModule = gatewayMessageModule ?? throw new ArgumentNullException(nameof(gatewayMessageModule));
            _logger = logger ?? NullLogger<DiscordGatewayClient>.Instance;
        }

        public void SetSessionInformation(DiscordGatewaySessionInformation sessionInformation)
        {
            ArgumentNullException.ThrowIfNull(sessionInformation, nameof(sessionInformation));
            SessionInformation = sessionInformation;
        }

        public async ValueTask ConnectAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (MessageModule.IsConnected())
            {
                throw new InvalidOperationException("Client is already connected or in the process of connecting.");
            }

            DiscordApiResponse<DiscordGatewayInformation> response = await RestClient.GetGatewayBotInformationAsync(cancellationToken);
            response.EnsureSuccess();

            SetSessionInformation(new()
            {
                GatewayInformation = response.Data,
                ResumeUrl = response.Data.Url
            });

            await ConnectAsync(SessionInformation.ResumeUrl, cancellationToken);
        }

        public async ValueTask ConnectAsync(Uri url, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(url, nameof(url));
            cancellationToken.ThrowIfCancellationRequested();
            if (MessageModule.IsConnected())
            {
                throw new InvalidOperationException("Client is already connected or in the process of connecting.");
            }

            _logger.LogTrace("Connecting to Discord Gateway at {Url}", url);
            await MessageModule.ConnectAsync(new Uri(url, "?v=10&encoding=json"), cancellationToken);
            _eventProcessingTask = ProcessEventsAsync();
            _logger.LogDebug("Connected to Discord Gateway at {Url}", url);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="forceDisconnect">If you're planning on resuming your current session, this MUST be <see cref="true"/>.</param>
        /// <param name="resumeUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask ReconnectAsync(WebSocketCloseStatus? webSocketCloseStatus = WebSocketCloseStatus.NormalClosure, Uri? resumeUrl = null, CancellationToken cancellationToken = default)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();
            await EventModule.InvokeAsync(new DiscordGatewayPayload<DiscordGatewayManualReconnectPayload>()
            {
                OpCode = DiscordGatewayOpCode.Reconnect,
                Data = new()
                {
                    NewSession = true
                },
                EventName = null,
                Sequence = null
            }, CancellationToken.None);

            await DisconnectAsync(webSocketCloseStatus, CancellationToken.None);

            // GatewayInformation will be null when the user calls
            // this method in their startup code. This can be a
            // reasonable assumption since the user can save their
            // session id externally and then call this method on
            // startup in an attempt to resume their session.
            DiscordGatewayInformation? gatewayInformation = SessionInformation.GatewayInformation ?? (await RestClient.GetGatewayBotInformationAsync(_cancellationTokenSource.Token)).Data;
            SetSessionInformation(SessionInformation with
            {
                GatewayInformation = gatewayInformation,
                ResumeUrl = resumeUrl ?? gatewayInformation.Url
            });

            await ConnectAsync(SessionInformation.ResumeUrl, _cancellationTokenSource.Token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="forceDisconnect">If you're planning on reconnecting and resuming your current session, this MUST be <see cref="true"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask DisconnectAsync(WebSocketCloseStatus? webSocketCloseStatus = WebSocketCloseStatus.NormalClosure, CancellationToken cancellationToken = default)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _cancellationTokenSource.CancelAsync();
            await MessageModule.DisconnectAsync(webSocketCloseStatus, null, CancellationToken.None);
            await _eventProcessingTask!;

            _eventProcessingTask = null;
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public ValueTask SendGatewayPayloadAsync<T>(DiscordGatewayOpCode opCode, T data, CancellationToken cancellationToken = default) => SendGatewayPayloadAsync(new DiscordGatewayPayload<T>()
        {
            OpCode = opCode,
            Data = data,
            EventName = null,
            Sequence = null
        }, cancellationToken);

        public ValueTask SendGatewayPayloadAsync(DiscordGatewayOpCode opCode, object? data, CancellationToken cancellationToken = default) => SendGatewayPayloadAsync(new DiscordGatewayPayload<object?>()
        {
            OpCode = opCode,
            Data = data,
            EventName = null,
            Sequence = null
        }, cancellationToken);

        public async ValueTask SendGatewayPayloadAsync(IDiscordGatewayPayload payload, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payload);
            await MessageModule.WriteAsync(payload, cancellationToken);
        }

        protected async Task ProcessEventsAsync()
        {
            if (_eventProcessingTask is not null)
            {
                throw new InvalidOperationException("Event processing task has already started.");
            }

            _logger.LogDebug("Starting processing gateway events.");
            await EventModule.StartAsync(this);
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                IDiscordGatewayPayload payload;
                try
                {
                    payload = await MessageModule.ReadAsync(_cancellationTokenSource.Token);
                }
                catch (Exception error)
                {
                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        break;
                    }
                    else if (!MessageModule.IsConnected())
                    {
                        // Let everyone knwo that we're reconnecting and attempting to resume.
                        await EventModule.InvokeAsync(new DiscordGatewayPayload<DiscordGatewayManualReconnectPayload>()
                        {
                            OpCode = DiscordGatewayOpCode.Reconnect,
                            Data = new()
                            {
                                NewSession = false
                            },
                            EventName = null,
                            Sequence = null
                        });

                        // Reconnect. The event handlers should resume on their own.
                        await MessageModule.DisconnectAsync(null, null, _cancellationTokenSource.Token);
                        await MessageModule.ConnectAsync(SessionInformation.ResumeUrl, _cancellationTokenSource.Token);
                        continue;
                    }

                    _logger.LogError(error, "Error while reading from the Discord Gateway.");
                    _ = ReconnectAsync(null, SessionInformation.ResumeUrl, CancellationToken.None).AsTask();
                    break;
                }

                if (payload.Sequence is not null)
                {
                    SetSessionInformation(SessionInformation with
                    {
                        LastSequence = payload.Sequence
                    });
                }

                await EventModule.QueueAsync(payload, _cancellationTokenSource.Token);
            }

            _logger.LogDebug("Stopped processing gateway events.");
            await EventModule.StopAsync();
        }
    }
}
