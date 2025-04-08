using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace WumpWump.Net.Entities
{
    public partial struct DiscordPermissionContainer
    {
        /// <summary>
        /// Sets the specified permission to the value of <paramref name="value"/>.
        /// </summary>
        /// <param name="permission">The permission/bit to set.</param>
        /// <param name="value">The value to set the permission to.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="bitIndex"/> is less than 0 or greater than <see cref="MAXIMUM_BIT_COUNT"/>.</exception>
        public void SetFlag(DiscordPermission permission, bool value) => SetFlag((int)permission, value);

        /// <summary>
        /// Sets the specified bit to the value of <paramref name="value"/>.
        /// </summary>
        /// <param name="bitIndex">The index of the bit to set.</param>
        /// <param name="value">The value to set the bit to.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="bitIndex"/> is less than 0 or greater than <see cref="MAXIMUM_BIT_COUNT"/>.</exception>
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
                _data[byteIndex] |= (byte)(1 << bitPosition);
            }
            else
            {
                _data[byteIndex] &= (byte)~(1 << bitPosition);
            }
        }

        /// <summary>
        /// Checks if the specified bit is set to true.
        /// </summary>
        /// <remarks>
        /// If you're doing a permissions check, use <see cref="HasPermission(DiscordPermission)"/> instead
        /// as that method will also check for the <see cref="DiscordPermission.Administrator"/> permission.
        /// </remarks>
        /// <param name="permission">The permission/bit to check.</param>
        /// <returns>True if the bit is set to true, false otherwise.</returns>
        public readonly bool HasFlag(DiscordPermission permission) => HasFlag((int)permission);

        /// <summary>
        /// Checks if the specified bit is set to true.
        /// </summary>
        /// <remarks>
        /// If you're doing a permissions check, use <see cref="HasPermission(DiscordPermission)"/> instead
        /// as that method will also check for the <see cref="DiscordPermission.Administrator"/> permission.
        /// </remarks>
        /// <param name="bitIndex">The index of the bit to check.</param>
        /// <returns>True if the bit is set to true, false otherwise.</returns>
        public readonly bool HasFlag(int bitIndex)
        {
            if (bitIndex is < 0 or >= MAXIMUM_BIT_COUNT)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex), $"The passed value must be between 0 and {MAXIMUM_BIT_COUNT}.");
            }

            int byteIndex = bitIndex >> 3;
            int bitPosition = bitIndex & 7;

            return (_data[byteIndex] & (1 << bitPosition)) != 0;
        }

        /// <summary>
        /// Checks if the container has the specified permission. Will also check for the <see cref="DiscordPermission.Administrator"/> permission.
        /// </summary>
        /// <param name="permission">The permission to check for.</param>
        /// <returns>True if the permission is set, false otherwise.</returns>
        public readonly bool HasPermission(DiscordPermission permission) => HasFlag(permission) || HasFlag(DiscordPermission.Administrator);

        /// <summary>
        /// Returns the raw data of the container as a <see cref="Span{byte}"/>. Any modifications to the
        /// returned span will be reflected in the container. Use this method with caution.
        /// </summary>
        /// <returns>A <see cref="Span{byte}"/> containing the raw data of the container.</returns>
        public Span<byte> AsSpan() => MemoryMarshal.CreateSpan(ref _data.ElementZero, MAXIMUM_BYTE_COUNT);

        /// <summary>
        /// Stringifies the container into decimal format. This is not the same as the
        /// <see cref="ToPermissionsString"/> method, which will return the names of the permissions.
        /// </summary>
        /// <returns>A string representation of the container in decimal format.</returns>
        public override readonly string ToString()
        {
            // If the highest bit is less than 64, we can use a ulong
            // Otherwise use a UInt128, or a BigInteger
            ulong value = 0;
            for (int i = 0; i < MAXIMUM_BIT_COUNT; i++)
            {
                if (HasFlag(i))
                {
                    value |= 1ul << i;
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// Converts the container to a hex string.
        /// </summary>
        /// <returns>>A hex string representation of the container.</returns>
        public string ToHexString()
        {
            Span<byte> bytes = AsSpan();
            bytes.Reverse();
            return Convert.ToHexString(_data);
        }

        /// <summary>
        /// Returns a string representation of the container in the format of "Permission1, Permission2, Permission3".
        /// If an unknown permission is set, it will be included in the string as the bit index ("64").
        /// </summary>
        /// <returns>>A string representation of the container.</returns>
        public string ToPermissionsString()
        {
            ulong value = BinaryPrimitives.ReadUInt64LittleEndian(AsSpan());
            int totalBitsSet = BitOperations.PopCount(value);
            if (totalBitsSet == 0)
            {
                return "None";
            }

            string[] names = new string[totalBitsSet];
            for (int i = MAXIMUM_BIT_COUNT - 1 - BitOperations.LeadingZeroCount(value); totalBitsSet > 0; i--)
            {
                if (HasFlag(i))
                {
                    names[--totalBitsSet] = _enumNames[i];
                }
            }

            return string.Join(", ", names);
        }

        /// <summary>
        /// Returns a generated hash code for the container. This is not guaranteed
        /// to be unique, but it should be sufficiently unique for most use cases.
        /// </summary>
        /// <returns>>A hash code for the container.</returns>
        public override readonly int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                hash = (hash * 31) + _data[i];
            }

            return hash;
        }

        /// <summary>
        /// Determines whether the specified <see cref="DiscordPermissionContainer"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="DiscordPermissionContainer"/> to compare with this instance.</param>
        /// <returns><seec langword="true"/> if the specified <see cref="DiscordPermissionContainer"/> is equal to this instance; otherwise, <see langword="false"/>.</returns>
        public bool Equals(DiscordPermissionContainer other) => AsSpan().SequenceEqual(other.AsSpan());

        /// <inheritdoc cref="object.Equals(object?)"/>
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is DiscordPermissionContainer other && Equals(other);

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