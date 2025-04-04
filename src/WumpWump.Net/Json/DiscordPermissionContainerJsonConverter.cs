using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using WumpWump.Net.Entities;

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
            DiscordPermissionContainer permissions = DiscordPermissionContainer.None;
            if (ulong.TryParse(value, out ulong permissionBits))
            {
                for (int i = 0; i < 64; i++)
                {
                    if ((permissionBits & (1UL << i)) != 0)
                    {
                        permissions.SetFlag(i, true);
                    }
                }

                return permissions;
            }

            // Try to fast path with UInt128 (This will work until like, 2035 when Discord decides to completely break everything)
            if (UInt128.TryParse(value, out UInt128 permissionBits128) && permissionBits128 <= _maxUInt128ValueForPermissions)
            {
                for (int i = 0; i < _maxUInt128BitCount; i++)
                {
                    if ((permissionBits128 & (UInt128.One << i)) != 0)
                    {
                        permissions.SetFlag(i, true);
                    }
                }

                return permissions;
            }

            // BigInteger it is
            if (!BigInteger.TryParse(value, out BigInteger permissionBitsBigInt))
            {
                throw new JsonException($"Failed to parse {value} as a BigInteger");
            }

            // Check if the number is too large
            long bitLength = permissionBitsBigInt.GetBitLength();
            if (bitLength > DiscordPermissionContainer.MAXIMUM_BIT_COUNT)
            {
                throw new JsonException($"Permission bits are too large: {value}");
            }

            for (int i = 0; i < bitLength; i++)
            {
                if ((permissionBitsBigInt & (BigInteger.One << i)) != 0)
                {
                    permissions.SetFlag(i, true);
                }
            }

            return permissions;
        }

        public override void Write(Utf8JsonWriter writer, DiscordPermissionContainer value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }
}