using OoLunar.AsyncEvents;
using WumpWump.Net.Gateway.Entities;

namespace WumpWump.Net.Gateway.Events.EventArgs
{
    public class DiscordGatewayAsyncEventArgs : AsyncEventArgs
    {
        public required DiscordGatewayClient Client { get; init; }
        public required IDiscordGatewayPayload Payload { get; init; }
    }
}
