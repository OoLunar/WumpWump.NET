using System;

namespace WumpWump.Net.Entities
{
    /// <summary>
    /// Extra information about an invite, will extend the <see cref="DiscordInvite"/> object.
    /// </summary>
    public record DiscordInviteMetadata : DiscordInvite
    {
        /// <summary>
        /// number of times this invite has been used
        /// </summary>
        public required int Uses { get; init; }

        /// <summary>
        /// max number of times this invite can be used
        /// </summary>
        public required int MaxUses { get; init; }

        /// <summary>
        /// duration (in seconds) after which the invite expires
        /// </summary>
        public required int MaxAge { get; init; }

        /// <summary>
        /// whether this invite only grants temporary membership
        /// </summary>
        public required bool Temporary { get; init; }

        /// <summary>
        /// when this invite was created
        /// </summary>
        public required DateTimeOffset CreatedAt { get; init; }
    }
}