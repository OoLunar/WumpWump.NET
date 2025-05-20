using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Represents a member of a lobby, including optional metadata and flags.
    /// </summary>
    public record DiscordLobbyMember
    {
        /// <summary>
        /// the id of the user
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// dictionary of string key/value pairs. The max total length is 1000.
        /// </summary>
        public DiscordOptional<IReadOnlyDictionary<string, string>?> Metadata { get; init; }

        /// <summary>
        /// lobby member flags combined as a bitfield
        /// </summary>
        public DiscordOptional<DiscordLobbyMemberFlags> Flags { get; init; }
    }
}