using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Entities.Payloads;

namespace WumpWump.Net.Gateway.Json
{
    public class DiscordGatewayHeartbeatPayloadJsonConverter : JsonConverter<DiscordGatewayHeartbeatPayload>
    {
        public override DiscordGatewayHeartbeatPayload Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType switch
        {
            JsonTokenType.Number => new DiscordGatewayHeartbeatPayload
            {
                SequenceNumber = reader.GetUInt64()
            },
            JsonTokenType.Null => new DiscordGatewayHeartbeatPayload()
            {
                SequenceNumber = null
            },
            _ => throw new JsonException($"Expected a {JsonTokenType.Number} or {JsonTokenType.Null} token type, but got {reader.TokenType}.")
        };

        public override void Write(Utf8JsonWriter writer, DiscordGatewayHeartbeatPayload value, JsonSerializerOptions options)
        {
            if (value.SequenceNumber.HasValue)
            {
                writer.WriteNumberValue(value.SequenceNumber.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
