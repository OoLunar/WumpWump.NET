using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace WumpWump.Net.Entities
{
    public partial struct DiscordPermissionContainer
    {
        public static implicit operator DiscordPermissionContainer(DiscordPermission permission) => new(permission);
        public static implicit operator DiscordPermissionContainer(ulong permissions) => new(permissions);
        public static implicit operator DiscordPermissionContainer(Int128 permissions) => new(permissions);
        public static implicit operator DiscordPermissionContainer(BigInteger permissions) => new(permissions);

        /// <summary>
        /// Returns the first 8 bytes of the <see cref="DiscordPermissionContainer"/> as a <see cref="ulong"/>.
        /// </summary>
        /// <param name="container">The <see cref="DiscordPermissionContainer"/> to convert.</param>
        public static unsafe explicit operator ulong(DiscordPermissionContainer container)
        {
            ulong result = 0;
            Unsafe.Copy(ref result, &container._data[0]);
            return result;
        }


        /// <summary>
        /// Returns the first 16 bytes of the <see cref="DiscordPermissionContainer"/> as a <see cref="Int128"/>.
        /// </summary>
        /// <param name="container">The <see cref="DiscordPermissionContainer"/> to convert.</param>
        public static unsafe explicit operator UInt128(DiscordPermissionContainer container)
        {
            UInt128 result = 0;
            Unsafe.Copy(ref result, &container._data[0]);
            return result;
        }

        /// <summary>
        /// Returns the container as a <see cref="BigInteger"/>.
        /// </summary>
        /// <param name="container">The <see cref="DiscordPermissionContainer"/> to convert.</param>
        public static explicit operator BigInteger(DiscordPermissionContainer container) => new(container._data, true, false);

        public static bool operator <(DiscordPermissionContainer left, DiscordPermissionContainer right)
        {
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                if (left._data[i] < right._data[i])
                {
                    return true;
                }
                else if (left._data[i] > right._data[i])
                {
                    return false;
                }
            }

            // All bytes are equal
            return false;
        }

        public static bool operator >(DiscordPermissionContainer left, DiscordPermissionContainer right) => right < left;

        public static bool operator <=(DiscordPermissionContainer left, DiscordPermissionContainer right) => !(right < left);

        public static bool operator >=(DiscordPermissionContainer left, DiscordPermissionContainer right) => !(left < right);

        public static DiscordPermissionContainer operator |(DiscordPermissionContainer left, DiscordPermissionContainer right)
        {
            DiscordPermissionContainer result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result._data[i] = (byte)(left._data[i] | right._data[i]);
            }

            return result;
        }

        public static DiscordPermissionContainer operator &(DiscordPermissionContainer left, DiscordPermissionContainer right)
        {
            DiscordPermissionContainer result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result._data[i] = (byte)(left._data[i] & right._data[i]);
            }

            return result;
        }

        public static DiscordPermissionContainer operator ^(DiscordPermissionContainer left, DiscordPermissionContainer right)
        {
            DiscordPermissionContainer result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result._data[i] = (byte)(left._data[i] ^ right._data[i]);
            }

            return result;
        }

        public static DiscordPermissionContainer operator ~(DiscordPermissionContainer container)
        {
            DiscordPermissionContainer result = new();
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                result._data[i] = (byte)~container._data[i];
            }

            return result;
        }

        public static DiscordPermissionContainer operator |(DiscordPermissionContainer left, DiscordPermission right)
        {
            DiscordPermissionContainer result = new(left);
            result._data.SetFlag((int)right, true);
            return result;
        }

        public static DiscordPermissionContainer operator &(DiscordPermissionContainer left, DiscordPermission right)
        {
            DiscordPermissionContainer result = new();
            result._data.SetFlag((int)right, left._data.HasFlag((int)right));
            return result;
        }

        public static DiscordPermissionContainer operator ^(DiscordPermissionContainer left, DiscordPermission right)
        {
            DiscordPermissionContainer result = new(left);
            result._data.SetFlag((int)right, !left._data.HasFlag((int)right));
            return result;
        }

        /// <summary>
        /// Determines whether the specified <see cref="DiscordPermissionContainer"/> is equal to this instance.
        /// </summary>
        /// <param name="left">The left <see cref="DiscordPermissionContainer"/> to compare.</param>
        /// <param name="right">The right <see cref="DiscordPermissionContainer"/> to compare.</param>
        /// <returns><seec langword="true"/> if the specified <see cref="DiscordPermissionContainer"/> is equal to this instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(DiscordPermissionContainer left, DiscordPermissionContainer right) => left.Equals(right);

        /// <summary>
        /// Determines whether the specified <see cref="DiscordPermissionContainer"/> is not equal to this instance.
        /// </summary>
        /// <param name="left">The left <see cref="DiscordPermissionContainer"/> to compare.</param>
        /// <param name="right">The right <see cref="DiscordPermissionContainer"/> to compare.</param>
        /// <returns><seec langword="true"/> if the specified <see cref="DiscordPermissionContainer"/> is not equal to this instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(DiscordPermissionContainer left, DiscordPermissionContainer right) => !(left == right);
    }
}