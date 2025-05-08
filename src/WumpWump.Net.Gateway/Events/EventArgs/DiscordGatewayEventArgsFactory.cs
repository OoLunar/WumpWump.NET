using WumpWump.Net.Gateway.Entities;

namespace WumpWump.Net.Gateway.Events.EventArgs
{
    public delegate DiscordGatewayAsyncEventArgs DiscordGatewayEventArgsFactory(DiscordGatewayClient client, IDiscordGatewayPayload payload);
}
