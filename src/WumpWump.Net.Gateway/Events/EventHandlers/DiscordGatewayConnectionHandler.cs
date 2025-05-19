using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OoLunar.AsyncEvents;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Entities.Commands;
using WumpWump.Net.Gateway.Entities.Payloads;
using WumpWump.Net.Gateway.Events.EventArgs;

namespace WumpWump.Net.Gateway.Events.EventHandlers
{
    public class DiscordGatewayConnectionHandler
    {
        protected readonly ILogger<DiscordGatewayConnectionHandler> _logger;

        public DiscordGatewayConnectionHandler(ILogger<DiscordGatewayConnectionHandler> logger)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _logger = logger;
        }

        [AsyncEventHandler]
        public static async ValueTask HandleHelloAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayHelloPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (asyncEventArgs.Client.SessionInformation.SessionId is not null)
            {
                // Resume the session
                await asyncEventArgs.Client.SendGatewayPayloadAsync(DiscordGatewayOpCode.Resume, new DiscordGatewayResumeCommand()
                {
                    SessionId = asyncEventArgs.Client.SessionInformation.SessionId!,
                    Sequence = asyncEventArgs.Client.SessionInformation.LastSequence,
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
        public static ValueTask HandleReadyAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayReadyPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (asyncEventArgs.Data is null)
            {
                throw new InvalidOperationException($"Null data was incorrectly passed to {nameof(HandleReadyAsync)}");
            }

            asyncEventArgs.Client.SetSessionInformation(asyncEventArgs.Client.SessionInformation with
            {
                ResumeUrl = asyncEventArgs.Data.ResumeGatewayUrl,
                SessionId = asyncEventArgs.Data.SessionId
            });

            return ValueTask.CompletedTask;
        }

        [AsyncEventHandler]
        public static ValueTask HandleInvalidSessionAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayInvalidSessionPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (asyncEventArgs.Data is null)
            {
                throw new InvalidOperationException($"Null data was incorrectly passed to {nameof(HandleInvalidSessionAsync)}");
            }

            WebSocketCloseStatus? closeStatus;
            if (asyncEventArgs.Data.ShouldResume)
            {
                // Reconnect and resume
                closeStatus = null;
            }
            else
            {
                // Reconnect and re-identify
                asyncEventArgs.Client.SetSessionInformation(asyncEventArgs.Client.SessionInformation with
                {
                    ResumeUrl = asyncEventArgs.Client.SessionInformation.GatewayInformation.Url,
                    SessionId = null,
                    LastSequence = null
                });

                closeStatus = WebSocketCloseStatus.NormalClosure;
            }

            _ = asyncEventArgs.Client.ReconnectAsync(closeStatus, asyncEventArgs.Client.SessionInformation.ResumeUrl, CancellationToken.None).AsTask();
            return ValueTask.CompletedTask;
        }

        [AsyncEventHandler]
        public static ValueTask HandleReconnectAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayReconnectPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            // Reconnect and resume
            //asyncEventArgs.Client.SetSessionInformation(asyncEventArgs.Client.SessionInformation with
            //{
            //    LastSequence = null
            //});

            _ = asyncEventArgs.Client.ReconnectAsync(null, asyncEventArgs.Client.SessionInformation.ResumeUrl, CancellationToken.None).AsTask();
            return ValueTask.CompletedTask;
        }

        [AsyncEventHandler]
        public ValueTask HandleZombieAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayZombiedPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            if (asyncEventArgs.Data is null)
            {
                throw new InvalidOperationException($"Null data was incorrectly passed to {nameof(HandleZombieAsync)}");
            }

            // Try to reconnect nicely the first time. If it fails, force a new connection
            _logger.LogDebug("Our gateway connection to Discord has been zombied. Attempting to reconnect...");
            _ = asyncEventArgs.Client.ReconnectAsync(null, asyncEventArgs.Data.MissedHeartbeats is < 3
                ? asyncEventArgs.Client.SessionInformation.ResumeUrl
                : asyncEventArgs.Client.SessionInformation.GatewayInformation.Url,
            CancellationToken.None).AsTask();
            return ValueTask.CompletedTask;
        }
    }
}
