using System.Drawing;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Roles represent a set of permissions attached to a group of users. Roles have names, colors, and can be
    /// "pinned" to the side bar, causing their members to be listed separately. Roles can have separate permission
    /// profiles for the global context (guild) and channel context. The @everyone role has the same ID as the guild
    /// it belongs to.
    /// </summary>
    public record DiscordRole
    {
        /// <summary>
        /// role id
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// role name
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// integer representation of hexadecimal color code
        /// </summary>
        public required Color Color { get; init; }

        /// <summary>
        /// if this role is pinned in the user listing
        /// </summary>
        public required bool Hoist { get; init; }

        /// <summary>
        /// role icon hash
        /// </summary>
        public DiscordOptional<string?> Icon { get; init; }

        /// <summary>
        /// role unicode emoji
        /// </summary>
        public DiscordOptional<string?> UnicodeEmoji { get; init; }

        /// <summary>
        /// position of this role (roles with the same position are sorted by id)
        /// </summary>
        public required int Position { get; init; }

        /// <summary>
        /// permission bit set
        /// </summary>
        public required DiscordPermissionContainer Permissions { get; init; }

        /// <summary>
        /// whether this role is managed by an integration
        /// </summary>
        public required bool Managed { get; init; }

        /// <summary>
        /// whether this role is mentionable
        /// </summary>
        public required bool Mentionable { get; init; }

        /// <summary>
        /// the tags this role has
        /// </summary>
        public DiscordOptional<DiscordRoleTags> Tags { get; init; }

        /// <summary>
        /// role flags combined as a bitfield
        /// </summary>
        public required DiscordRoleFlags Flags { get; init; }
    }
}
