using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OoLunar.AsyncEvents;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Entities.Payloads;
using WumpWump.Net.Gateway.Events.EventArgs;

namespace WumpWump.Net.Gateway.Events.EventHandlers
{
    public class DiscordGatewayHeartbeatHandler : IAsyncDisposable
    {
        protected TimeSpan? _heartbeatInterval;
        protected DateTime _lastPayloadReceived;
        protected DateTime _lastHeartbeatReceived;
        protected DateTime _lastHeartbeatSent;

        protected Task? _heartbeatTask;
        protected DiscordGatewayClient? _client;
        protected ILogger<DiscordGatewayHeartbeatHandler> _logger;
        protected CancellationTokenSource _cancellationTokenSource = new();

        public DiscordGatewayHeartbeatHandler(ILogger<DiscordGatewayHeartbeatHandler> logger)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _logger = logger;
        }

        [AsyncEventHandler]
        public async ValueTask HandleHelloAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayHelloPayload> asyncEventArgs, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (asyncEventArgs.Data is null)
            {
                throw new InvalidOperationException($"Null data was passed to {nameof(HandleHelloAsync)}");
            }

            bool firstRun = _client is null;
            _client = asyncEventArgs.Client;
            _heartbeatInterval = TimeSpan.FromMilliseconds(asyncEventArgs.Data.HeartbeatInterval);

            if (!firstRun)
            {
                // Per Discord's documentation, we should wait a random amount of time (Jitter) before sending the first heartbeat
                // We're choosing to do this only when reconnecting since a random amount of time has usually passed when the bot
                // is first connected. This is to prevent long startup times when the bot is first connected.
                await Task.Delay(_heartbeatInterval.Value * Math.Min(Random.Shared.NextDouble(), 0.25), _client.CancellationToken);
            }

            // Start the heartbeater
            await _client.SendGatewayPayloadAsync(DiscordGatewayOpCode.Heartbeat, _client.SessionInformation.LastSequence, _client.CancellationToken);
            _lastHeartbeatSent = DateTime.Now;
            _heartbeatTask = HeartbeatBackgroundTaskAsync();
        }

        [AsyncEventHandler]
        public async ValueTask HandleHeartbeatAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayHeartbeatPayload> _, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_client is null || _heartbeatTask is null)
            {
                throw new InvalidOperationException($"{nameof(HandleHeartbeatAsync)} was called before {nameof(HandleHelloAsync)}");
            }

            // If we're heartbeating twice at once, for some reason,
            // just let the other one finish and return
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            // Reset the heartbeat background task since
            // we're manually sending a heartbeat to Discord
            await _cancellationTokenSource.CancelAsync();
            if (_heartbeatTask is not null)
            {
                await _heartbeatTask;
            }

            // Send the heartbeat to Discord
            await _client.SendGatewayPayloadAsync(DiscordGatewayOpCode.Heartbeat, _client.SessionInformation.LastSequence, _cancellationTokenSource.Token);
            _lastHeartbeatSent = DateTime.Now;

            // Start the heartbeater again
            _heartbeatTask = HeartbeatBackgroundTaskAsync();
        }

        [AsyncEventHandler]
        public ValueTask HandleHeartbeatAckAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayHeartbeatAckPayload> _, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_client is null || _heartbeatTask is null)
            {
                throw new InvalidOperationException($"{nameof(HandleHeartbeatAckAsync)} was called before {nameof(HandleHelloAsync)}");
            }

            // Reset the missed heartbeats
            _lastHeartbeatReceived = DateTime.Now;
            _client.SetSessionInformation(_client.SessionInformation with
            {
                Ping = _lastHeartbeatReceived - _lastHeartbeatSent
            });

            _logger.LogTrace("Ping: {Ping:N0}ms", _client.SessionInformation.Ping!.Value.TotalMilliseconds);
            return ValueTask.CompletedTask;
        }

        [AsyncEventHandler]
        public ValueTask HandleSessionInvalidatedAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayInvalidSessionPayload> _, CancellationToken cancellationToken = default)
            => StopAsync(CancellationToken.None);

        [AsyncEventHandler]
        public ValueTask HandleManualReconnectAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayManualReconnectPayload> _, CancellationToken cancellationToken = default)
            => StopAsync(CancellationToken.None);

        [AsyncEventHandler]
        public ValueTask HandleReconnectAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayReconnectPayload> _, CancellationToken cancellationToken = default)
            => StopAsync(CancellationToken.None);

        [AsyncEventHandler]
        public ValueTask LastPayloadReceivedAsync(DiscordWebsocketMessageEventArgs _, CancellationToken cancellationToken = default)
        {
            // Reset the last payload received time
            _lastPayloadReceived = DateTime.Now;
            return ValueTask.CompletedTask;
        }

        public async ValueTask StopAsync(CancellationToken cancellationToken = default)
        {
            if (_heartbeatTask is null)
            {
                return;
            }

            // Cancel the heartbeater
            cancellationToken.ThrowIfCancellationRequested();
            await _cancellationTokenSource.CancelAsync();

            // Wait for the heartbeater to finish
            if (_heartbeatTask is not null)
            {
                await _heartbeatTask;
            }

            _client?.SetSessionInformation(_client.SessionInformation with
            {
                Ping = null
            });
        }

        protected async Task HeartbeatBackgroundTaskAsync()
        {
            if (_client is null || _heartbeatInterval is null)
            {
                throw new InvalidOperationException($"{nameof(HeartbeatBackgroundTaskAsync)} was called before {nameof(HandleHelloAsync)}");
            }

            // Reset the cancellation token source so only this
            // object and the gateway client can cancel the task
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_client.CancellationToken);

            // When passing a cancellation token to the PeriodicTimer,
            // it will throw an OperationCanceledException instead of
            // returning false. Since we're going to be passing the token
            // to the gateway client anyways, we'll just catch the exception
            // instead of registering a delegate to dispose the timer (which
            // would correctly cause it to reutrn false).
            using PeriodicTimer timer = new(_heartbeatInterval.Value);
            try
            {
                while (await timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
                {
                    await _client.SendGatewayPayloadAsync(DiscordGatewayOpCode.Heartbeat, _client.SessionInformation.LastSequence, _cancellationTokenSource.Token);
                    _lastHeartbeatSent = DateTime.Now;

                    // Check if the connection zombied by checking if
                    // Discord has missed more than 1 heartbeat and if
                    // the last payload was sent more than 1.5 times the
                    // heartbeat interval ago. In the extremely unlikely
                    // event that Discord is still sending us payloads
                    // but not returning our heartbeats, we will not zombify
                    // since we're still receiving important data.
                    int missedHeartbeats = (int)((_lastHeartbeatSent - _lastHeartbeatReceived).TotalMilliseconds / _heartbeatInterval.Value.TotalMilliseconds);
                    if (missedHeartbeats > 1 && _lastPayloadReceived + (_heartbeatInterval.Value / 2) < _lastHeartbeatSent)
                    {
                        try
                        {
                            // Since at the time of writing, the current heartbeat is
                            // at 42.5 seconds (and has been for years), we can generally
                            // assume that heartbeats will be few and far between,
                            // giving us plenty of time to execute the async event
                            // before the next heartbeat is required.
                            await _client.EventModule.InvokeAsync(new DiscordGatewayAsyncEventArgs<DiscordGatewayZombiedPayload>
                            {
                                Client = _client,
                                Payload = new DiscordGatewayPayload<DiscordGatewayZombiedPayload>()
                                {
                                    OpCode = DiscordGatewayOpCode.Dispatch,
                                    Data = new DiscordGatewayZombiedPayload
                                    {
                                        LastSequenceReceived = _client.SessionInformation.LastSequence,
                                        MissedHeartbeats = missedHeartbeats,
                                    },
                                    EventName = "ZOMBIED",
                                    Sequence = null
                                }
                            }, CancellationToken.None);
                        }
                        catch (Exception error)
                        {
                            _logger.LogError(error, "An exception occured when invoking the zombied event");
                        }
                    }
                }
            }
            catch (OperationCanceledException) { }

            _heartbeatTask = null;
        }

        public async ValueTask DisposeAsync()
        {
            if (_client is null)
            {
                return;
            }

            // Cancel the heartbeater
            await StopAsync();
            _client = null;
            _heartbeatInterval = null;
            GC.SuppressFinalize(this);
        }
    }
}
