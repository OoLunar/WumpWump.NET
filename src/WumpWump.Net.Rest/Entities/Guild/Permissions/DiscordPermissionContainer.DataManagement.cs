using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WumpWump.Net.Rest.Entities
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
        public Span<byte> AsSpan()
        {
            Span<byte> bytes = MemoryMarshal.CreateSpan(ref _data.ElementZero, MAXIMUM_BYTE_COUNT);
            if (!BitConverter.IsLittleEndian)
            {
                bytes.Reverse();
            }

            return bytes;
        }

        /// <summary>
        /// Stringifies the container into decimal format. This is not the same as the
        /// <see cref="ToPermissionsString"/> method, which will return the names of the permissions.
        /// </summary>
        /// <returns>A string representation of the container in decimal format.</returns>
        public override string ToString() => ToString(null, null);

        /// <inheritdoc cref="ulong.ToString(IFormatProvider?)"/>
        public string ToString(IFormatProvider? formatProvider) => ToString(null, formatProvider);

        /// <inheritdoc cref="ulong.ToString(string?)"/>
        public string ToString([StringSyntax("NumericFormat")] string? format) => ToString(format, null);

        /// <inheritdoc cref="ulong.ToString(string?, IFormatProvider?)"/>
        public string ToString([StringSyntax("NumericFormat")] string? format, IFormatProvider? formatProvider)
        {
            int leadingZeroCount = 0;
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i += 4)
            {
                int chunkZeroCount = BitOperations.LeadingZeroCount(Unsafe.As<byte, uint>(ref Unsafe.AsRef(ref _data[i])));
                if (chunkZeroCount != 32)
                {
                    break;
                }

                leadingZeroCount += 4;
            }

            int rangeLength = MAXIMUM_BYTE_COUNT - leadingZeroCount;
            return rangeLength switch
            {
                0 => "0",
                <= 1 => _data.ElementZero.ToString(format, formatProvider),
                <= 2 => Unsafe.As<byte, ushort>(ref Unsafe.AsRef(ref _data.ElementZero)).ToString(format, formatProvider),
                <= 4 => Unsafe.As<byte, uint>(ref Unsafe.AsRef(ref _data.ElementZero)).ToString(format, formatProvider),
                <= 8 => Unsafe.As<byte, ulong>(ref Unsafe.AsRef(ref _data.ElementZero)).ToString(format, formatProvider),
                <= 16 => Unsafe.As<byte, UInt128>(ref Unsafe.AsRef(ref _data.ElementZero)).ToString(format, formatProvider),
                _ => new BigInteger(AsSpan(), true, false).ToString(format, formatProvider)
            };
        }

        /// <summary>
        /// Converts the container to a hex string.
        /// </summary>
        /// <returns>>A hex string representation of the container.</returns>
        public string ToHexString()
        {
            // We don't call AsSpan here because then we'd reverse the bytes twice, which is redundant.
            Span<byte> bytes = MemoryMarshal.CreateSpan(ref _data.ElementZero, MAXIMUM_BYTE_COUNT);
            if (BitConverter.IsLittleEndian)
            {
                bytes.Reverse();
            }

            return Convert.ToHexString(bytes);
        }

        /// <summary>
        /// Returns a string representation of the container in the format of "Permission1, Permission2, Permission3".
        /// If an unknown permission is set, it will be included in the string as the bit index ("64").
        /// </summary>
        /// <returns>>A string representation of the container.</returns>
        public string ToPermissionsString()
        {
            int totalBitsSet = 0;
            for (int i = 0; i < MAXIMUM_BYTE_COUNT - 4; i += 4)
            {
                totalBitsSet += BitOperations.PopCount(Unsafe.As<byte, uint>(ref Unsafe.AsRef(ref _data[i])));
            }

            if (totalBitsSet == 0)
            {
                return "None";
            }

            int leadingZeroCount = 0;
            for (int i = 0; i < MAXIMUM_BYTE_COUNT - 4; i += 4)
            {
                int chunkZeroCount = BitOperations.LeadingZeroCount(Unsafe.As<byte, uint>(ref Unsafe.AsRef(ref _data[i])));
                if (chunkZeroCount != 32)
                {
                    break;
                }

                leadingZeroCount += 4;
            }

            string[] names = new string[totalBitsSet];
            for (int i = MAXIMUM_BIT_COUNT - 1 - leadingZeroCount; totalBitsSet > 0; i--)
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
    }
}
