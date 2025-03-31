using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Json
{
    public sealed class DiscordSnowflakeJsonConverter : JsonConverter<DiscordSnowflake>
    {
        public override DiscordSnowflake Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => ulong.TryParse(reader.GetString(), NumberStyles.Number, CultureInfo.InvariantCulture, out ulong snowflake) ? snowflake : default;

        public override void Write(Utf8JsonWriter writer, DiscordSnowflake value, JsonSerializerOptions options) => writer.WriteStringValue(value.Value.ToString(CultureInfo.InvariantCulture));
    }
}
