using WumpWump.Net.Gateway.Events;

namespace WumpWump.Net.Gateway.Entities.Payloads
{
    [DiscordGatewayEvent((DiscordGatewayOpCode)(-1), "Reconnect")]
    public record DiscordGatewayManualReconnectPayload
    {
        /// <summary>
        /// Not a Discord API field. Indicates whether the client has manually
        /// disconnected from the gateway with the intent to reconnect and resume.
        /// </summary>
        public bool NewSession { get; init; }
    }
}
