using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using WumpWump.Net.Json;

namespace WumpWump.Net.Entities
{
    /// <summary>
    /// Permissions are a way to limit and grant certain abilities to users in Discord.
    /// A set of base permissions can be configured at the guild level for different roles.
    /// When these roles are attached to users, they grant or revoke specific privileges within the guild.
    /// Along with the guild-level permissions, Discord also supports permission overwrites that can be
    /// assigned to individual roles or members on a per-channel basis.
    /// </summary>
    [SuppressMessage("Design", "WWL0002", Justification = "InlineArray cannot be record structs.")]
    [DebuggerDisplay("{ToString(),nq}")]
    [InlineArray(MAXIMUM_BYTE_COUNT)]
    [JsonConverter(typeof(DiscordPermissionContainerJsonConverter))]
    public partial struct DiscordPermissionContainer
    {
        // Discord uses bits to store permissions, so we need to have a maximum bit count which supports all permissions
#if ENABLE_LARGE_PERMISSIONS
        public const int MAXIMUM_BIT_COUNT = 256;
#else
        public const int MAXIMUM_BIT_COUNT = 64;
#endif

        // >> 3 is equivalent to dividing by 8 (since 2³ = 8)
        public const int MAXIMUM_BYTE_COUNT = (MAXIMUM_BIT_COUNT + 7) >> 3;

        public static readonly DiscordPermissionContainer None;
        public static readonly DiscordPermissionContainer All;

        private static readonly int _sizeOfUInt128 = Unsafe.SizeOf<UInt128>();

        static DiscordPermissionContainer()
        {
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                All[i] = byte.MaxValue;
            }
        }

        [SuppressMessage("Design", "IDE0044", Justification = "InlineArray cannot have readonly fields")]
        [SuppressMessage("Design", "IDE0051", Justification = "This is a placeholder for the inline array")]
        private byte _element0;

        public DiscordPermissionContainer(DiscordPermission permission) => SetFlag((int)permission, true);

        public void SetFlag(DiscordPermission permission, bool value) => SetFlag((int)permission, value);

        public void SetFlag(int bitIndex, bool value)
        {
            if (bitIndex is < 0 or >= MAXIMUM_BIT_COUNT)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex), $"Bit index must be between 0 and {MAXIMUM_BIT_COUNT}.");
            }

            int byteIndex = bitIndex >> 3;
            int bitPosition = bitIndex & 7;

            if (value)
            {
                this[byteIndex] |= (byte)(1 << bitPosition);
            }
            else
            {
                this[byteIndex] &= (byte)~(1 << bitPosition);
            }
        }

        public readonly bool HasFlag(int bitIndex)
        {
            if (bitIndex is < 0 or >= MAXIMUM_BIT_COUNT)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex), $"The passed value must be between 0 and {MAXIMUM_BIT_COUNT}.");
            }

            int byteIndex = bitIndex >> 3;
            int bitPosition = bitIndex & 7;

            return (this[byteIndex] & (1 << bitPosition)) != 0;
        }

        public readonly bool HasFlag(DiscordPermission permission) => HasFlag((int)permission);
        public readonly bool HasPermission(DiscordPermission permission) => HasFlag(permission) || HasFlag(DiscordPermission.Administrator);

        public unsafe Span<byte> AsSpan() => new(Unsafe.AsPointer(ref _element0), MAXIMUM_BYTE_COUNT);

        public override readonly string ToString()
        {
            // Grab the highest bit set
            int highestBit = 0;
            for (int i = MAXIMUM_BIT_COUNT - 1; i >= 0; i--)
            {
                if (HasFlag(i))
                {
                    highestBit = i;
                    break;
                }
            }

            if (highestBit < sizeof(ulong) * 8)
            {
                // If the highest bit is less than 64, we can use a ulong
                ulong value = 0;
                for (int i = 0; i <= highestBit; i++)
                {
                    if (HasFlag(i))
                    {
                        value |= 1ul << i;
                    }
                }

                return value.ToString();
            }
            else if (highestBit < _sizeOfUInt128 * 8)
            {
                // If the highest bit is less than 128, we can use a UInt128
                UInt128 value = UInt128.Zero;
                for (int i = 0; i <= highestBit; i++)
                {
                    if (HasFlag(i))
                    {
                        value |= UInt128.One << i;
                    }
                }

                return value.ToString();
            }

            // Otherwise, we need to use a BigInteger
            BigInteger bigValue = 0;
            for (int i = 0; i <= highestBit; i++)
            {
                if (HasFlag(i))
                {
                    bigValue |= BigInteger.One << i;
                }
            }

            return bigValue.ToString();
        }

        public string ToHexString()
        {
            Span<byte> bytes = AsSpan();
            bytes.Reverse();
            return Convert.ToHexString(this);
        }

        public readonly string ToPermissionsString()
        {
            StringBuilder stringBuilder = new();
            for (int i = 0; i < MAXIMUM_BIT_COUNT; i++)
            {
                if (HasFlag(i))
                {
                    stringBuilder.Append(((DiscordPermission)i).ToString());
                    stringBuilder.Append(", ");
                }
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Length -= 2; // Remove the last comma and space
            }
            else
            {
                stringBuilder.Append("None");
            }

            return stringBuilder.ToString();
        }

        public override readonly int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                hash = (hash * 31) + this[i];
            }

            return hash;
        }

        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is DiscordPermissionContainer other && this == other;
        public static bool operator ==(DiscordPermissionContainer left, DiscordPermissionContainer right) => left.AsSpan().SequenceEqual(right.AsSpan());
        public static bool operator !=(DiscordPermissionContainer left, DiscordPermissionContainer right) => !(left == right);
        public static DiscordPermissionContainer operator |(DiscordPermissionContainer left, DiscordPermissionContainer right)
        {
            DiscordPermissionContainer result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result[i] = (byte)(left[i] | right[i]);
            }

            return result;
        }

        public static DiscordPermissionContainer operator &(DiscordPermissionContainer left, DiscordPermissionContainer right)
        {
            DiscordPermissionContainer result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result[i] = (byte)(left[i] & right[i]);
            }

            return result;
        }

        public static DiscordPermissionContainer operator ^(DiscordPermissionContainer left, DiscordPermissionContainer right)
        {
            DiscordPermissionContainer result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result[i] = (byte)(left[i] ^ right[i]);
            }

            return result;
        }

        public static DiscordPermissionContainer operator ~(DiscordPermissionContainer container)
        {
            DiscordPermissionContainer result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result[i] = (byte)~container[i];
            }

            return result;
        }

        public static implicit operator DiscordPermissionContainer(DiscordPermission permission) => new(permission);
    }
}