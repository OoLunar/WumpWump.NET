using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Json;

namespace WumpWump.Net.Gateway.Payloads
{
    [DiscordGatewayEvent(DiscordGatewayOpCode.HeartbeatACK, null)]
    [JsonConverter(typeof(DiscordGatewayHeartbeatAckPayloadJsonConverter))]
    public record DiscordGatewayHeartbeatAckPayload;
}
