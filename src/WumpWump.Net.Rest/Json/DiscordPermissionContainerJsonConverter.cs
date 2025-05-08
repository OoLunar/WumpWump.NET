using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.Json
{
    public class DiscordPermissionContainerJsonConverter : JsonConverter<DiscordPermissionContainer>
    {
        protected static readonly int _sizeOfUInt128 = Unsafe.SizeOf<UInt128>();
        protected static readonly UInt128 _maxPermissionsUInt128Value = UInt128.MaxValue << Math.Clamp(DiscordPermissionContainer.MAXIMUM_BIT_COUNT, 0, _sizeOfUInt128 * 8);

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
            else if (DiscordPermissionContainer.MAXIMUM_BIT_COUNT > 64 && UInt128.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out UInt128 permissionBitsInt128) && permissionBitsInt128 <= _maxPermissionsUInt128Value)
            {
                return new DiscordPermissionContainer(permissionBitsInt128);
            }
            else if (DiscordPermissionContainer.MAXIMUM_BIT_COUNT > 128 && BigInteger.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out BigInteger permissionBitsBigInteger) && permissionBitsBigInteger.GetBitLength() <= DiscordPermissionContainer.MAXIMUM_BIT_COUNT)
            {
                return new DiscordPermissionContainer(permissionBitsBigInteger);
            }

            throw new JsonException($"Failed to parse DiscordPermissionContainer from string, is it larger than {DiscordPermissionContainer.MAXIMUM_BIT_COUNT} bits? Value: {value}");
        }

        public override void Write(Utf8JsonWriter writer, DiscordPermissionContainer value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}
