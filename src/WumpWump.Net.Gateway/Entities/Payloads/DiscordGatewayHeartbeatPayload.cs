using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Json;

namespace WumpWump.Net.Gateway.Entities.Payloads
{
    [DiscordGatewayEvent(DiscordGatewayOpCode.Heartbeat, null)]
    [JsonConverter(typeof(DiscordGatewayHeartbeatPayloadJsonConverter))]
    public record DiscordGatewayHeartbeatPayload
    {
        public ulong? SequenceNumber { get; init; }
    }
}
