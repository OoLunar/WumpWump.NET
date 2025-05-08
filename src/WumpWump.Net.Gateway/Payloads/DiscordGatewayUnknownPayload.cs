using System.Text.Json.Nodes;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events;

namespace WumpWump.Net.Gateway.Payloads
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
