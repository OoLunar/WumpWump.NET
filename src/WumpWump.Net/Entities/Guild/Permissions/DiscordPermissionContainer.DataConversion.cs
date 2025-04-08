namespace WumpWump.Net.Entities
{
    public partial struct DiscordPermissionContainer
    {
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

        public static implicit operator DiscordPermissionContainer(DiscordPermission permission) => new(permission);

        public static DiscordPermissionContainer operator |(DiscordPermissionContainer left, DiscordPermission right)
        {
            DiscordPermissionContainer result = new(left);
            result._data.SetFlag((int)right, true);
            return result;
        }

        public static DiscordPermissionContainer operator &(DiscordPermissionContainer left, DiscordPermission right)
        {
            DiscordPermissionContainer result = new(left);
            result._data.SetFlag((int)right, true);
            return result;
        }

        public static DiscordPermissionContainer operator ^(DiscordPermissionContainer left, DiscordPermission right)
        {
            DiscordPermissionContainer result = new(left);
            result._data.SetFlag((int)right, true);
            return result;
        }
    }
}