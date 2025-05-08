using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Payloads;

namespace WumpWump.Net.Gateway.Json
{
    public class DiscordGatewayReconnectPayloadJsonConverter : JsonConverter<DiscordGatewayReconnectPayload>
    {
        public override DiscordGatewayReconnectPayload Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => reader.TokenType is JsonTokenType.Null
                ? new DiscordGatewayReconnectPayload()
                : throw new JsonException($"Expected {nameof(JsonTokenType.Null)} but got {reader.TokenType}");

        public override void Write(Utf8JsonWriter writer, DiscordGatewayReconnectPayload value, JsonSerializerOptions options)
            => writer.WriteNullValue();
    }
}
