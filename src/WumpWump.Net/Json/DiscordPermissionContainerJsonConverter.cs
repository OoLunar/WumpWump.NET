using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using WumpWump.Net.Entities;
#if ENABLE_LARGE_PERMISSIONS
using System.Numerics;
#endif

namespace WumpWump.Net.Json
{
    public class DiscordPermissionContainerJsonConverter : JsonConverter<DiscordPermissionContainer>
    {
        protected static readonly int _sizeOfUInt128 = Unsafe.SizeOf<UInt128>();
        protected static readonly int _maxUInt128BitCount = _sizeOfUInt128 * 8;
        protected static readonly UInt128 _maxUInt128ValueForPermissions = UInt128.MaxValue << Math.Clamp(DiscordPermissionContainer.MAXIMUM_BIT_COUNT, 0, Unsafe.SizeOf<UInt128>());

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
            else if (ulong.TryParse(value, out ulong permissionBits))
            {
                return new DiscordPermissionContainer(permissionBits);
            }
#if ENABLE_LARGE_PERMISSIONS
            // Try to fast path with UInt128 (This will work until like, 2035 when Discord decides to completely break everything)
            else if (UInt128.TryParse(value, out UInt128 permissionBits128) && permissionBits128 <= _maxUInt128ValueForPermissions)
            {
                return new DiscordPermissionContainer(permissionBits128);
            }
            // BigInteger it is
            else if (BigInteger.TryParse(value, out BigInteger permissionBitsBigInt))
            {
                try
                {
                    return new DiscordPermissionContainer(permissionBitsBigInt);
                }
                catch (InvalidOperationException error)
                {
                    throw new JsonException(error.Message);
                }
            }
#endif

            throw new JsonException($"Failed to parse DiscordPermissionContainer from string, is it larger than {DiscordPermissionContainer.MAXIMUM_BIT_COUNT} bits? Value: {value}");
        }

        public override void Write(Utf8JsonWriter writer, DiscordPermissionContainer value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }
}