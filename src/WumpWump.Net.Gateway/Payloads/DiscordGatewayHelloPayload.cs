using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events;

namespace WumpWump.Net.Gateway.Payloads
{
    [DiscordGatewayEvent(DiscordGatewayOpCode.Hello, null)]
    public record DiscordGatewayHelloPayload
    {
        /// <summary>
        /// Interval (in milliseconds) an app should heartbeat with
        /// </summary>
        public required int HeartbeatInterval { get; init; }
    }
}
