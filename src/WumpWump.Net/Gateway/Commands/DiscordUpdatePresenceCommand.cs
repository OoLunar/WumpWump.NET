using System.Collections.Generic;
using WumpWump.Net.Gateway.Entities;

namespace WumpWump.Net.Gateway.Commands
{
    /// <summary>
    /// Sent by the client to indicate a presence or status update.
    /// </summary>
    public readonly record struct DiscordUpdatePresenceCommand
    {
        /// <summary>
        /// Unix time (in milliseconds) of when the client went idle, or null if the client is not idle
        /// </summary>
        public required long? Since { get; init; }

        /// <summary>
        /// User's activities
        /// </summary>
        public required IReadOnlyList<DiscordGatewayActivity> Activities { get; init; }

        /// <summary>
        /// User's new <see cref="DiscordGatewayStatus"/>
        /// </summary>
        public required string Status { get; init; }

        /// <summary>
        /// Whether or not the client is afk
        /// </summary>
        public required bool Afk { get; init; }
    }
}