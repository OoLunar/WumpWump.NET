using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Entities.Payloads;

namespace WumpWump.Net.Gateway.Json
{
    public class DiscordGatewayHeartbeatAckPayloadJsonConverter : JsonConverter<DiscordGatewayHeartbeatAckPayload>
    {
        public override DiscordGatewayHeartbeatAckPayload Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.Null)
            {
                throw new JsonException("Expected null token type.");
            }

            // DiscordHeartbeatAckPayload is an empty payload, so we just return a new instance
            return new DiscordGatewayHeartbeatAckPayload();
        }

        public override void Write(Utf8JsonWriter writer, DiscordGatewayHeartbeatAckPayload value, JsonSerializerOptions options)
            // DiscordHeartbeatAckPayload is an empty payload, so we just write null
            => writer.WriteNullValue();
    }
}
