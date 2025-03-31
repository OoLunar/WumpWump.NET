using System.Collections.Generic;

namespace WumpWump.Net.Entities
{
    /// <summary>
    /// The role connection object that an application has attached to a user.
    /// </summary>
    public record DiscordApplicationRoleConnection
    {
        /// <summary>
        /// the vanity name of the platform a bot has connected (max 50 characters)
        /// </summary>
        public required string? PlatformName { get; init; }

        /// <summary>
        /// the username on the platform a bot has connected (max 100 characters)
        /// </summary>
        public required string? PlatformUsername { get; init; }

        /// <summary>
        /// object mapping <see cref="DiscordApplicationRoleConnectionMetadata"/> to their <c>string</c>-ified value (max 100 characters) for the user on the platform a bot has connected
        /// </summary>
        public required IReadOnlyDictionary<string, DiscordApplicationRoleConnectionMetadata> Metadata { get; init; }
    }
}
