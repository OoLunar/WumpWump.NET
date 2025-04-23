using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace WumpWump.Net.Entities
{
    public partial struct DiscordPermissionContainer
    {
        /// <summary>
        /// The maximum amount of flags that this container can hold. As Discord adds more permissions,
        /// this number may change, however the API on the container will (should) not.
        /// </summary>
        /// TODO: When incrementing this, ensure it's in increments of 32 bits.
        /// This is due to the cast to uint in the To*String methods.
        public const int MAXIMUM_BIT_COUNT = 64;

        /// <summary>
        /// The maximum amount of bytes that this container can hold. This is the same as
        /// <see cref="MAXIMUM_BIT_COUNT"/> divided by 8, rounded up to the nearest byte.
        /// Alternatively, this can be seen as doing <c>sizeof(DiscordPermissionContainerData)</c>.
        /// </summary>
        // >> 3 is equivalent to dividing by 8 (since 2³ = 8)
        public const int MAXIMUM_BYTE_COUNT = (MAXIMUM_BIT_COUNT + 7) >> 3;

        [InlineArray(MAXIMUM_BYTE_COUNT)]
        private struct DiscordPermissionContainerData
        {
            [SuppressMessage("Design", "IDE0044", Justification = "InlineArray cannot have readonly fields")]
            [SuppressMessage("Design", "IDE0051", Justification = "This is a placeholder for the inline array")]
            public byte ElementZero;

            public bool SetFlag(int bitIndex, bool value)
            {
                int byteIndex = bitIndex >> 3;
                int bitPosition = bitIndex & 7;

                if (value)
                {
                    this[byteIndex] |= (byte)(1 << bitPosition);
                    return true;
                }
                else
                {
                    this[byteIndex] &= (byte)~(1 << bitPosition);
                    return false;
                }
            }

            public readonly bool HasFlag(int bitIndex)
            {
                int byteIndex = bitIndex >> 3;
                int bitPosition = bitIndex & 7;

                return (this[byteIndex] & (1 << bitPosition)) != 0;
            }
        }
    }
}
