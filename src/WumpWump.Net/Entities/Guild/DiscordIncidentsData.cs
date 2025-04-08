using System;

namespace WumpWump.Net.Entities
{
    public record DiscordIncidentsData
    {
        /// <summary>
        /// when invites get enabled again
        /// </summary>
        public required DateTimeOffset? InvitesDisabledUntil { get; init; }

        /// <summary>
        /// when direct messages get enabled again
        /// </summary>
        public required DateTimeOffset? DmsDisabledUntil { get; init; }

        /// <summary>
        /// when the dm spam was detected
        /// </summary>
        public DiscordOptional<DateTimeOffset?> DmSpamDetectedAt { get; init; }

        /// <summary>
        /// when the raid was detected
        /// </summary>
        public DiscordOptional<DateTimeOffset?> RaidDetectedAt { get; init; }
    }
}