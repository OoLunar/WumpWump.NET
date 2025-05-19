using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Json;

namespace WumpWump.Net.Gateway.Entities.Payloads
{
    [DiscordGatewayEvent(DiscordGatewayOpCode.InvalidSession, null)]
    [JsonConverter(typeof(DiscordGatewayInvalidSessionPayloadJsonConverter))]
    public record DiscordGatewayInvalidSessionPayload
    {
        /// <summary>
        /// Whether the session can be resumed on a new connection.
        /// </summary>
        public required bool ShouldResume { get; init; }
    }
}
