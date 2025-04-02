using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

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
    [InlineArray(MAXIMUM_BYTE_COUNT)]
    public partial struct DiscordPermissions
    {
        // Discord uses bits to store permissions, so we need to have a maximum bit count which supports all permissions
        public const int MAXIMUM_BIT_COUNT = 128;

        // >> 3 is equivalent to dividing by 8 (since 2³ = 8)
        public const int MAXIMUM_BYTE_COUNT = (MAXIMUM_BIT_COUNT + 7) >> 3;

        [SuppressMessage("Design", "IDE0044", Justification = "InlineArray cannot have readonly fields")]
        [SuppressMessage("Design", "IDE0051", Justification = "This is a placeholder for the inline array")]
        private byte _element0;

        public DiscordPermissions()
        {
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                this[i] = 0;
            }
        }

        private static DiscordPermissions CreateFull()
        {
            DiscordPermissions perms = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                perms[i] = byte.MaxValue;
            }

            return perms;
        }

        public static DiscordPermissions Create(int bitIndex)
        {
            if (bitIndex is < 0 or >= MAXIMUM_BIT_COUNT)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            }

            DiscordPermissions perms = new();
            perms[bitIndex / 8] = (byte)(1 << (bitIndex % 8));
            return perms;
        }

        public readonly bool HasFlag(int bitIndex) => bitIndex is < 0 or >= MAXIMUM_BIT_COUNT
            ? throw new ArgumentOutOfRangeException(nameof(bitIndex))
            : (this[bitIndex / 8] & (1 << (bitIndex % 8))) != 0;

        public readonly bool HasFlag(DiscordPermissions flag)
        {
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                if ((this[i] & flag[i]) != flag[i])
                {
                    return false;
                }
            }

            return true;
        }

        public void SetFlag(DiscordPermissions flag, bool value)
        {
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                if (value)
                {
                    this[i] |= flag[i];
                }
                else
                {
                    this[i] &= (byte)~flag[i];
                }
            }
        }

        public readonly Span<byte> AsSpan() => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in _element0), MAXIMUM_BYTE_COUNT);

        public static DiscordPermissions operator |(DiscordPermissions left, DiscordPermissions right)
        {
            DiscordPermissions result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result[i] = (byte)(left[i] | right[i]);
            }

            return result;
        }

        public static DiscordPermissions operator &(DiscordPermissions left, DiscordPermissions right)
        {
            DiscordPermissions result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result[i] = (byte)(left[i] & right[i]);
            }

            return result;
        }

        public static DiscordPermissions operator ~(DiscordPermissions value)
        {
            DiscordPermissions result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result[i] = (byte)~value[i];
            }

            return result;
        }

        public readonly bool Equals(DiscordPermissions other)
        {
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                if (this[i] != other[i])
                {
                    return false;
                }
            }
            return true;
        }

        public override readonly bool Equals(object? obj) => obj is DiscordPermissions other && Equals(other);

        public override readonly int GetHashCode()
        {
            HashCode hash = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                hash.Add(this[i]);
            }

            return hash.ToHashCode();
        }

        public static bool operator ==(DiscordPermissions left, DiscordPermissions right) => left.Equals(right);
        public static bool operator !=(DiscordPermissions left, DiscordPermissions right) => !left.Equals(right);

        public static implicit operator int(DiscordPermissions permissions) => permissions.GetHashCode();

        public override readonly string ToString()
        {
            Span<byte> bytes = AsSpan();
            bytes.Reverse();
            return Convert.ToHexString(bytes);
        }

        public readonly string ToHumanReadableString()
        {
            StringBuilder builder = new();
            for (int i = 0; i < MAXIMUM_BIT_COUNT; i++)
            {
                if (!HasFlag(i))
                {
                    continue;
                }

                DiscordPermissions flag = Create(i);
                if (Names.TryGetValue(flag.GetHashCode(), out string? name))
                {
                    builder.Append($"{name}, ");
                }
                else
                {
                    builder.Append($"{i}, ");
                }
            }

            if (builder.Length > 0)
            {
                builder.Length -= 2;
            }

            return builder.ToString();
        }
    }
}
