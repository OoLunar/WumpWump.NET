using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Json;

namespace WumpWump.Net.Gateway.Payloads
{
    [DiscordGatewayEvent(DiscordGatewayOpCode.Heartbeat, null)]
    [JsonConverter(typeof(DiscordGatewayHeartbeatPayloadJsonConverter))]
    public record DiscordGatewayHeartbeatPayload
    {
        public ulong? SequenceNumber { get; init; }
    }
}
