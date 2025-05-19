using System.Text.Json.Nodes;
using WumpWump.Net.Gateway.Events;

namespace WumpWump.Net.Gateway.Entities.Payloads
{
    [DiscordGatewayEvent((DiscordGatewayOpCode)(-1), "Unknown")]
    public record DiscordGatewayUnknownPayload
    {
        /// <summary>
        /// The raw json from the <see cref="DiscordGatewayPayload{T}.Data"/> property.
        /// </summary>
        public JsonObject? Json { get; init; }
    }
}
