using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// The connection object that the user has attached.
    /// </summary>
    public record DiscordUserConnection
    {
        /// <summary>
        /// id of the connection account
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// the username of the connection account
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// the <a href="https://discord.com/developers/docs/resources/user#connection-object-services">service</a> of this connection
        /// </summary>
        public required string Type { get; init; }

        /// <summary>
        /// whether the connection is revoked
        /// </summary>
        public DiscordOptional<bool> Revoked { get; init; }

        /// <summary>
        /// an array of partial <see cref="DiscordIntegration"/>
        /// </summary>
        public DiscordOptional<IReadOnlyList<DiscordIntegration>> Integrations { get; init; }

        /// <summary>
        /// whether the connection is verified
        /// </summary>
        public required bool Verified { get; init; }

        /// <summary>
        /// whether friend sync is enabled for this connection
        /// </summary>
        public required bool FriendSync { get; init; }

        /// <summary>
        /// whether activities related to this connection will be shown in presence updates
        /// </summary>
        public required bool ShowActivity { get; init; }

        /// <summary>
        /// whether this connection has a corresponding third party OAuth2 token
        /// </summary>
        public required bool TwoWayLink { get; init; }

        /// <summary>
        /// <see cref="DiscordUserConnectionVisibility"/> of this connection
        /// </summary>
        public required DiscordUserConnectionVisibility Visibility { get; init; }
    }
}
