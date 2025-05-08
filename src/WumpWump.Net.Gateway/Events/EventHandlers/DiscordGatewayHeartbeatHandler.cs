using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OoLunar.AsyncEvents;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events.EventArgs;
using WumpWump.Net.Gateway.Payloads;

namespace WumpWump.Net.Gateway.Events.EventHandlers
{
    public class DiscordGatewayHeartbeatHandler : IAsyncDisposable
    {
        protected DiscordGatewayClient? _client;
        protected TimeSpan? _heartbeatInterval;
        protected ILogger<DiscordGatewayHeartbeatHandler> _logger;
        protected int _missedHeartbeats;

        protected Task? _heartbeatTask;
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
            await _client.SendGatewayPayloadAsync(DiscordGatewayOpCode.Heartbeat, _client.LastSequenceReceived, _client.CancellationToken);
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

            // Send the heartbeat to Discord
            await _client.SendGatewayPayloadAsync(DiscordGatewayOpCode.Heartbeat, _client.LastSequenceReceived, _cancellationTokenSource.Token);

            // Start the heartbeater again
            _heartbeatTask.Dispose();
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
            _missedHeartbeats = 0;
            return ValueTask.CompletedTask;
        }

        [AsyncEventHandler]
        public ValueTask HandleSessionInvalidatedAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayInvalidSessionPayload> _, CancellationToken cancellationToken = default)
            => StopAsync(CancellationToken.None);

        [AsyncEventHandler]
        public ValueTask HandleReconnectAsync(DiscordGatewayAsyncEventArgs<DiscordGatewayReconnectPayload> _, CancellationToken cancellationToken = default)
            => StopAsync(CancellationToken.None);

        public async ValueTask StopAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_client is null || _heartbeatTask is null)
            {
                throw new InvalidOperationException($"{nameof(HandleSessionInvalidatedAsync)} was called before {nameof(HandleHelloAsync)}");
            }

            // Cancel the heartbeater
            await _cancellationTokenSource.CancelAsync();

            // Wait for the heartbeater to finish
            await _heartbeatTask;
            _heartbeatTask.Dispose();
            _heartbeatTask = null;
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
                    await _client.SendGatewayPayloadAsync(DiscordGatewayOpCode.Heartbeat, _client.LastSequenceReceived, _cancellationTokenSource.Token);
                    _missedHeartbeats++;

                    // Since at the time of writing, the current heartbeat is
                    // at 42.5 seconds (and has been for years), we can generally
                    // assume that heartbeats will be few and far between,
                    // giving us plenty of time to execute the async event
                    // before the next heartbeat is required.
                    if (_missedHeartbeats > 1)
                    {
                        try
                        {
                            await _client.InvokeEventAsync(new DiscordGatewayAsyncEventArgs<DiscordGatewayZombiedPayload>
                            {
                                Client = _client,
                                Payload = new DiscordGatewayPayload<DiscordGatewayZombiedPayload>()
                                {
                                    OpCode = DiscordGatewayOpCode.Dispatch,
                                    Data = new DiscordGatewayZombiedPayload
                                    {
                                        LastSequenceReceived = _client.LastSequenceReceived,
                                        MissedHeartbeats = _missedHeartbeats,
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
