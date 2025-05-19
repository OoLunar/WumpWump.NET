using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Json;

namespace WumpWump.Net.Gateway.Entities.Payloads
{
    [DiscordGatewayEvent(DiscordGatewayOpCode.Reconnect, null)]
    [JsonConverter(typeof(DiscordGatewayReconnectPayloadJsonConverter))]
    public record DiscordGatewayReconnectPayload;
}
