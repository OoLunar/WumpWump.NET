using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Payloads;

namespace WumpWump.Net.Gateway.Json
{
    public class DiscordGatewayInvalidSessionPayloadJsonConverter : JsonConverter<DiscordGatewayInvalidSessionPayload>
    {
        public override DiscordGatewayInvalidSessionPayload Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not JsonTokenType.True and not JsonTokenType.False)
            {
                throw new JsonException("Expected a boolean value.");
            }

            bool shouldResume = reader.GetBoolean();
            return new DiscordGatewayInvalidSessionPayload
            {
                ShouldResume = shouldResume
            };
        }

        public override void Write(Utf8JsonWriter writer, DiscordGatewayInvalidSessionPayload value, JsonSerializerOptions options)
            => writer.WriteBooleanValue(value.ShouldResume);
    }
}
