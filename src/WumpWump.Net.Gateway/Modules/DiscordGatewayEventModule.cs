using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OoLunar.AsyncEvents;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Entities.Payloads;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Events.EventArgs;
using WumpWump.Net.Gateway.Events.EventHandlers;

namespace WumpWump.Net.Gateway.Modules
{
    public class DiscordGatewayEventModule : IDiscordGatewayEventModule
    {
        public DiscordGatewayClient? Client { get; protected set; }
        public AsyncEventContainer AsyncEventContainer { get; init; }

        [MemberNotNullWhen(true, nameof(Client))]
        public bool IsRunning => _eventProcessingTask is not null;

        protected readonly ILogger<DiscordGatewayEventModule> _logger;
        protected readonly FrozenDictionary<Type, DiscordGatewayEventArgsFactory> _payloadToEventArgs;

        protected Task? _eventProcessingTask;
        protected readonly Channel<DiscordGatewayAsyncEventArgs> _eventChannel = Channel.CreateUnbounded<DiscordGatewayAsyncEventArgs>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false
        });

        public DiscordGatewayEventModule(AsyncEventContainer asyncEventContainer, DiscordGatewayEventRegistration eventRegistrations, ILogger<DiscordGatewayEventModule>? logger = null)
        {
            if (asyncEventContainer is null)
            {
                asyncEventContainer = new AsyncEventContainer();
                asyncEventContainer.AddHandlers<DiscordGatewayConnectionHandler>(new DiscordGatewayConnectionHandler(NullLogger<DiscordGatewayConnectionHandler>.Instance));
                asyncEventContainer.AddHandlers<DiscordGatewayHeartbeatHandler>(new DiscordGatewayHeartbeatHandler(NullLogger<DiscordGatewayHeartbeatHandler>.Instance));
            }

            AsyncEventContainer = asyncEventContainer;

            Dictionary<Type, DiscordGatewayEventArgsFactory> payloadToEventArgs = [];
            foreach (DiscordGatewayEventTicket registration in eventRegistrations)
            {
                payloadToEventArgs.Add(registration.EventType, registration.CreateGatewayEventArgs);
            }

            _payloadToEventArgs = payloadToEventArgs.ToFrozenDictionary();
            _logger = logger ?? NullLogger<DiscordGatewayEventModule>.Instance;
        }

        public ValueTask StartAsync(DiscordGatewayClient client)
        {
            if (_eventProcessingTask is not null)
            {
                throw new InvalidOperationException("Event processing task has already started.");
            }

            Client = client ?? throw new ArgumentNullException(nameof(client));
            _eventProcessingTask = ProcessEventsAsync();
            return ValueTask.CompletedTask;
        }

        public ValueTask StopAsync()
        {
            if (_eventProcessingTask is null)
            {
                throw new InvalidOperationException("Event processing task is not running.");
            }

            Task eventProcessingTask = _eventProcessingTask;
            _eventProcessingTask = null;
            return new ValueTask(eventProcessingTask);
        }

        public ValueTask QueueAsync(IDiscordGatewayPayload payload, CancellationToken cancellationToken = default)
        {
            if (!IsRunning)
            {
                throw new InvalidOperationException($"{nameof(StartAsync)} must be called before {nameof(QueueAsync)}.");
            }

            ArgumentNullException.ThrowIfNull(payload);
            return QueueAsync(CreateGatewayEventArgs(Client, payload), cancellationToken);
        }

        public ValueTask QueueAsync(DiscordGatewayAsyncEventArgs eventArgs, CancellationToken cancellationToken = default)
        {
            if (!IsRunning)
            {
                throw new InvalidOperationException($"{nameof(StartAsync)} must be called before {nameof(QueueAsync)}.");
            }
            else if (_eventChannel.Writer.TryWrite(eventArgs))
            {
                return ValueTask.CompletedTask;
            }

            return _eventChannel.Writer.WriteAsync(eventArgs, cancellationToken);
        }

        public ValueTask<bool> InvokeAsync(IDiscordGatewayPayload payload, CancellationToken cancellationToken = default)
        {
            if (!IsRunning)
            {
                throw new InvalidOperationException($"{nameof(StartAsync)} must be called before {nameof(InvokeAsync)}.");
            }

            ArgumentNullException.ThrowIfNull(payload);
            return InvokeAsync(CreateGatewayEventArgs(Client, payload), cancellationToken);
        }

        public ValueTask<bool> InvokeAsync(DiscordGatewayAsyncEventArgs eventArgs, CancellationToken cancellationToken = default)
        {
            if (!IsRunning)
            {
                throw new InvalidOperationException($"{nameof(StartAsync)} must be called before {nameof(InvokeAsync)}.");
            }

            ArgumentNullException.ThrowIfNull(eventArgs);
            IAsyncEvent asyncEvent = AsyncEventContainer.GetAsyncEvent(eventArgs.GetType());
            return asyncEvent.InvokeAsync(eventArgs, cancellationToken);
        }

        protected async Task ProcessEventsAsync()
        {
            if (Client is null)
            {
                throw new InvalidOperationException("Client is not set.");
            }

            // Delay so that the _eventProcessingTask can be assigned
            await Task.Delay(1);

            try
            {
                _logger.LogDebug("Started invoking events.");
                while (await _eventChannel.Reader.WaitToReadAsync(Client.CancellationToken))
                {
                    if (Client.CancellationToken.IsCancellationRequested)
                    {
                        // The cancellation token was triggered, so we can ignore this error.
                        break;
                    }

                    while (_eventChannel.Reader.TryRead(out DiscordGatewayAsyncEventArgs? eventArgs))
                    {
                        Debug.Assert(eventArgs is not null, "Event args should not be null");

                        try
                        {
                            await InvokeAsync(eventArgs, Client.CancellationToken);
                        }
                        catch (Exception error)
                        {
                            if (Debugger.IsAttached)
                            {
                                Debugger.BreakForUserUnhandledException(error);
                            }

                            _logger.LogError(error, "An error occurred while invoking the event handler: {EventType}", eventArgs.GetType().Name);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                if (!Client.CancellationToken.IsCancellationRequested)
                {
                    _logger.LogError(error, "Error invoking events");
                }
            }

            _logger.LogDebug("Stopped invoking events.");
        }

        protected DiscordGatewayAsyncEventArgs CreateGatewayEventArgs(DiscordGatewayClient client, IDiscordGatewayPayload payload)
        {
            Type payloadType = payload.GetType();
            return payloadType.IsGenericType && _payloadToEventArgs.TryGetValue(payloadType.GenericTypeArguments[0], out DiscordGatewayEventArgsFactory? factory)
                ? factory(client, payload)
                : _payloadToEventArgs[typeof(DiscordGatewayUnknownPayload)](client, payload);
        }
    }
}
