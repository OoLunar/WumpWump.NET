using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OoLunar.AsyncEvents;
using WumpWump.Net.Gateway.Commands;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events.EventArgs;
using WumpWump.Net.Gateway.Payloads;

namespace WumpWump.Net.Gateway.Events.EventHandlers
{
    public class DiscordGatewayConnectionHandler
    {
        protected readonly ILogger<DiscordGatewayConnectionHandler> _logger;

        protected ulong? _lastSequenceReceived;
        protected Uri? _resumeUrl;
        protected string? _sessionId;

        public DiscordGatewayConnectionHandler(ILogger<DiscordGatewayConnectionHandler> logger)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _logger = logger;
        }

        [AsyncEventHandler]
        public async ValueTask HandleHelloAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayHelloPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_resumeUrl is null ^ _sessionId is null)
            {
                throw new InvalidOperationException("Discord passed invalid data to the client. Either both resume URL and session ID should be null or both should be set.");
            }
            else if (_resumeUrl is not null)
            {
                // Resume the session
                await asyncEventArgs.Client.SendGatewayPayloadAsync(DiscordGatewayOpCode.Resume, new DiscordGatewayResumeCommand()
                {
                    SessionId = _sessionId!,
                    Sequence = _lastSequenceReceived,
                    Token = asyncEventArgs.Client.RestClient.RestClientOptions.DiscordToken
                }, cancellationToken);
            }
            else
            {
                // Send the identify payload
                await asyncEventArgs.Client.SendGatewayPayloadAsync(DiscordGatewayOpCode.Identify, new DiscordGatewayIdentifyCommand()
                {
                    Intents = DiscordGatewayIntents.All,
                    Properties = new(),
                    Token = asyncEventArgs.Client.RestClient.RestClientOptions.DiscordToken,
                    Compress = false
                }, cancellationToken);
            }
        }

        [AsyncEventHandler]
        public ValueTask HandleReadyAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayReadyPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (asyncEventArgs.Data is null)
            {
                throw new InvalidOperationException($"Null data was incorrectly passed to {nameof(HandleReadyAsync)}");
            }

            _resumeUrl = asyncEventArgs.Data.ResumeGatewayUrl;
            _sessionId = asyncEventArgs.Data.SessionId;
            return ValueTask.CompletedTask;
        }

        [AsyncEventHandler]
        public ValueTask HandleInvalidSessionAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayInvalidSessionPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (asyncEventArgs.Data is null)
            {
                throw new InvalidOperationException($"Null data was incorrectly passed to {nameof(HandleInvalidSessionAsync)}");
            }

            if (_resumeUrl is null || _sessionId is null)
            {
                throw new InvalidOperationException($"{nameof(HandleInvalidSessionAsync)} was called before {nameof(HandleReadyAsync)}");
            }

            if (asyncEventArgs.Data.ShouldResume)
            {
                // Reconnect and resume
                _lastSequenceReceived = asyncEventArgs.Client.LastSequenceReceived;
                _ = asyncEventArgs.Client.ReconnectAsync(false, _resumeUrl, CancellationToken.None).AsTask();
            }
            else
            {
                // Reconnect and re-identify
                _resumeUrl = null;
                _sessionId = null;
                _lastSequenceReceived = null;
                _ = asyncEventArgs.Client.ReconnectAsync(false, CancellationToken.None).AsTask();
            }

            return ValueTask.CompletedTask;
        }

        [AsyncEventHandler]
        public async ValueTask HandleReconnectAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayReconnectPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            // Reconnect and resume
            _lastSequenceReceived = asyncEventArgs.Client.LastSequenceReceived;
            await asyncEventArgs.Client.ReconnectAsync(false, _resumeUrl, CancellationToken.None);
        }

        [AsyncEventHandler]
        public async ValueTask HandleZombieAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayZombiedPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            if (asyncEventArgs.Data is null)
            {
                throw new InvalidOperationException($"Null data was incorrectly passed to {nameof(HandleZombieAsync)}");
            }

            // Try to reconnect nicely the first time. If it fails, force a new connection
            _logger.LogDebug("Our gateway connection to Discord has been zombied. Attempting to reconnect...");
            _lastSequenceReceived = asyncEventArgs.Client.LastSequenceReceived;
            if (asyncEventArgs.Data.MissedHeartbeats is < 3)
            {
                await asyncEventArgs.Client.ReconnectAsync(false, _resumeUrl, CancellationToken.None);
            }
            else
            {
                // Force a new connection
                await asyncEventArgs.Client.ReconnectAsync(true, null, CancellationToken.None);
            }
        }
    }
}
