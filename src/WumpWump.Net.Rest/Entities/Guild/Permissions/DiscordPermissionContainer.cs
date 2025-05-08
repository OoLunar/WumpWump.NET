using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using WumpWump.Net.Rest.Json;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Permissions are a way to limit and grant certain abilities to users in Discord.
    /// A set of base permissions can be configured at the guild level for different roles.
    /// When these roles are attached to users, they grant or revoke specific privileges within the guild.g
    /// Along with the guild-level permissions, Discord also supports permission overwrites that can be
    /// assigned to individual roles or members on a per-channel basis.
    /// </summary>
    [JsonConverter(typeof(DiscordPermissionContainerJsonConverter))]
    [DebuggerDisplay("{ToPermissionsString()}")]
    [SuppressMessage("Design", "WWL0002", Justification = "InlineArray cannot be record structs.")]
    public partial struct DiscordPermissionContainer
    {
        /// <summary>
        /// A <see cref="DiscordPermission"/> container that has all flags set to true. Not to be
        /// confused with <see cref="DiscordPermission.Administrator"/>, which only has the 8th bit
        /// set due to special handling by Discord.
        /// </summary>
        public static readonly DiscordPermissionContainer All;

        /// <summary>
        /// A <see cref="DiscordPermission"/> container that has all flags set to false.
        /// </summary>
        public static readonly DiscordPermissionContainer None;

        /// <summary>
        /// An array of all the <see cref="DiscordPermission"/> names. <see cref="ToPermissionsString"/>
        /// uses this for quick indexing and to avoid string allocations. This will enumerate non-defined
        /// permissions as their index in the array. I.E, <c>1 << 64</c> will be <c>"64"</c> while
        /// <c>1 << 8</c> will be <see cref="DiscordPermission.Administrator"/>.
        /// </summary>
        private static readonly string[] _enumNames;

        public bool this[int bitIndex] { get => HasFlag(bitIndex); set => SetFlag(bitIndex, value); }

        /// <summary>
        /// The actual data member that stores the flags. We wrap around this to provide a
        /// more user-friendly API, such as <see cref="System.Collections.Generic.IEnumerable{DiscordPermission}"/>,
        /// which is impossible to do directly on an <see cref="System.Runtime.CompilerServices.InlineArrayAttribute"/>
        /// at this time of writing. The more I look at that attribute, the more I recognize
        /// how much of a compiler hack it truly is.
        /// </summary>
        private DiscordPermissionContainerData _data;

        static DiscordPermissionContainer()
        {
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                All._data[i] = byte.MaxValue;
            }

            _enumNames = new string[MAXIMUM_BIT_COUNT + 1];
            for (int i = 0; i < MAXIMUM_BIT_COUNT + 1; i++)
            {
                _enumNames[i] = ((DiscordPermission)i).ToString();
            }
        }

        /// <summary>
        /// Creates a new <see cref="DiscordPermissionContainer"/> with all flags set to false.
        /// Equivalent to <see cref="None"/>.
        /// </summary>
        public DiscordPermissionContainer() { }

        /// <summary>
        /// Creates a new <see cref="DiscordPermissionContainer"/> with the specified permission set to true.
        /// </summary>
        /// <param name="permission">The permission to set.</param>
        public DiscordPermissionContainer(DiscordPermission permission)
        {
            if ((int)permission > MAXIMUM_BIT_COUNT - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(permission), permission, "Permission is out of range.");
            }

            _data = new DiscordPermissionContainerData();
            _data.SetFlag((int)permission, true);
        }

        /// <summary>
        /// Creates a new <see cref="DiscordPermissionContainer"/> with the specified permissions set to true.
        /// </summary>
        /// <param name="permissions">The permissions to set.</param>
        public DiscordPermissionContainer(params ReadOnlySpan<DiscordPermission> permissions)
        {
            _data = new DiscordPermissionContainerData();
            foreach (DiscordPermission permission in permissions)
            {
                if ((int)permission > MAXIMUM_BIT_COUNT - 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(permissions), permission, "Permission is out of range.");
                }

                _data.SetFlag((int)permission, true);
            }
        }

        /// <summary>
        /// Copies the flags set in the specified <see cref="DiscordPermissionContainer"/> to this instance.
        /// </summary>
        /// <param name="container">The <see cref="DiscordPermissionContainer"/> to copy from.</param>
        public DiscordPermissionContainer(DiscordPermissionContainer container)
        {
            for (int i = 0; i < MAXIMUM_BYTE_COUNT; i++)
            {
                _data[i] = container._data[i];
            }
        }

        /// <summary>
        /// Parses the flags set in the specified <see cref="ulong"/> to this instance.
        /// </summary>
        /// <param name="permissions">The <see cref="ulong"/> to parse from.</param>
        public DiscordPermissionContainer(ulong permissions)
        {
            _data = new DiscordPermissionContainerData();
            for (int i = 0; i < Math.Min(MAXIMUM_BIT_COUNT, 64); i++)
            {
                if ((permissions & (1ul << i)) != 0)
                {
                    _data.SetFlag(i, true);
                }
            }
        }

        public DiscordPermissionContainer(UInt128 permissions)
        {
            _data = new DiscordPermissionContainerData();
            for (int i = 0; i < Math.Min(MAXIMUM_BIT_COUNT, 128); i++)
            {
                if ((permissions & (UInt128.One << i)) != 0)
                {
                    _data.SetFlag(i, true);
                }
            }
        }

        public DiscordPermissionContainer(BigInteger permissions)
        {
            _data = new DiscordPermissionContainerData();
            for (int i = 0; i < Math.Min(MAXIMUM_BIT_COUNT, permissions.GetBitLength()); i++)
            {
                if ((permissions & (BigInteger.One << i)) != 0)
                {
                    _data.SetFlag(i, true);
                }
            }
        }
    }
}
