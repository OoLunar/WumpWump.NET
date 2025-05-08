using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Json;

namespace WumpWump.Net.Gateway.Payloads
{
    [DiscordGatewayEvent(DiscordGatewayOpCode.Reconnect, null)]
    [JsonConverter(typeof(DiscordGatewayReconnectPayloadJsonConverter))]
    public record DiscordGatewayReconnectPayload
    {
        /// <summary>
        /// Not a Discord API field. Indicates whether the client has manually
        /// disconnected from the gateway with the intent to reconnect.
        /// </summary>
        public bool ManualDisconnect { get; init; }
    }
}
