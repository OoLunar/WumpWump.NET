using System;
using System.Collections.Generic;

namespace WumpWump.Net.Entities
{
    [Obsolete("Discord has deprecated this and it will be removed in a future version.")]
    public record DiscordInviteStageInstance
    {
        /// <summary>
        /// the members speaking in the Stage
        /// </summary>
        public required IReadOnlyList<DiscordMember> Members { get; set; }

        /// <summary>
        /// the number of users in the Stage
        /// </summary>
        public required int ParticipationCount { get; init; }

        /// <summary>
        /// the number of users speaking in the Stage
        /// </summary>
        public required int SpeakerCount { get; init; }

        /// <summary>
        /// the topic of the Stage instance (1-120 characters)
        /// </summary>
        public required string Topic { get; init; }
    }
}