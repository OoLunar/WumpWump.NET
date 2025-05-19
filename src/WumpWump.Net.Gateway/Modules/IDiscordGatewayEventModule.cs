using System.Threading;
using System.Threading.Tasks;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events.EventArgs;

namespace WumpWump.Net.Gateway.Modules
{
    public interface IDiscordGatewayEventModule
    {
        bool IsRunning { get; }

        ValueTask<bool> InvokeAsync(IDiscordGatewayPayload payload, CancellationToken cancellationToken = default);
        ValueTask<bool> InvokeAsync(DiscordGatewayAsyncEventArgs eventArgs, CancellationToken cancellationToken = default);
        ValueTask QueueAsync(IDiscordGatewayPayload payload, CancellationToken cancellationToken = default);
        ValueTask QueueAsync(DiscordGatewayAsyncEventArgs eventArgs, CancellationToken cancellationToken = default);
        ValueTask StartAsync(DiscordGatewayClient client);
        ValueTask StopAsync();
    }
}
