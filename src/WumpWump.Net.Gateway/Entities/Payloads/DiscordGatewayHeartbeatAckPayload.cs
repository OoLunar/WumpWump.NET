using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Json;

namespace WumpWump.Net.Gateway.Entities.Payloads
{
    [DiscordGatewayEvent(DiscordGatewayOpCode.HeartbeatACK, null)]
    [JsonConverter(typeof(DiscordGatewayHeartbeatAckPayloadJsonConverter))]
    public record DiscordGatewayHeartbeatAckPayload;
}
