using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Represents a lobby within Discord. See Managing Lobbies for more information.
    /// </summary>
    public record DiscordLobby
    {
        /// <summary>
        /// the id of this channel
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// application that created the lobby
        /// </summary>
        public required DiscordSnowflake ApplicationId { get; init; }

        /// <summary>
        /// dictionary of string key/value pairs. The max total length is 1000.
        /// </summary>
        public required IReadOnlyDictionary<string, string>? Metadata { get; init; }

        /// <summary>
        /// members of the lobby
        /// </summary>
        public required IReadOnlyList<DiscordLobbyMember> Members { get; init; }

        /// <summary>
        /// the guild channel linked to the lobby
        /// </summary>
        public DiscordOptional<DiscordChannel> LinkedChannel { get; init; }
    }
}