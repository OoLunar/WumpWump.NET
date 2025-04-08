using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Json
{
    public sealed class DiscordPermissionContainerJsonConverter : JsonConverter<DiscordPermissionContainer>
    {
        public override DiscordPermissionContainer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Expected string token, got {reader.TokenType}");
            }

            string? value = reader.GetString();
            if (string.IsNullOrEmpty(value) || value == "0")
            {
                return DiscordPermissionContainer.None;
            }
            // Try to fast path with ulong
            else if (ulong.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out ulong permissionBits))
            {
                return new DiscordPermissionContainer(permissionBits);
            }

            throw new JsonException($"Failed to parse DiscordPermissionContainer from string, is it larger than {DiscordPermissionContainer.MAXIMUM_BIT_COUNT} bits? Value: {value}");
        }

        public override void Write(Utf8JsonWriter writer, DiscordPermissionContainer value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }
}