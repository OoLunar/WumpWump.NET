using System;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// The thread metadata object contains a number of thread-specific channel fields that are not needed by other channel types.
    /// </summary>
    public record DiscordThreadMetadata
    {
        /// <summary>
        /// whether the thread is archived
        /// </summary>
        public required bool Archived { get; init; }

        /// <summary>
        /// the thread will stop showing in the channel list after <see cref="DiscordDefaultAutoArchiveDuration"/> minutes of inactivity,
        /// can be set to: <see cref="DiscordDefaultAutoArchiveDuration.OneHour"/>, <see cref="DiscordDefaultAutoArchiveDuration.OneDay"/>,
        /// <see cref="DiscordDefaultAutoArchiveDuration.ThreeDays"/>, <see cref="DiscordDefaultAutoArchiveDuration.OneWeek"/>
        /// </summary>
        public required DiscordDefaultAutoArchiveDuration AutoArchiveDuration { get; init; }

        /// <summary>
        /// timestamp when the thread's archive status was last changed, used for calculating recent activity
        /// </summary>
        public required DateTimeOffset ArchiveTimestamp { get; init; }

        /// <summary>
        /// whether the thread is locked; when a thread is locked, only users with <see cref="DiscordPermission.ManageThreads"/> can unarchive it
        /// </summary>
        public required bool Locked { get; init; }

        /// <summary>
        /// whether non-moderators can add other non-moderators to a thread; only available on private threads
        /// </summary>
        public DiscordOptional<bool> Invitable { get; init; }

        /// <summary>
        /// timestamp when the thread was created; only populated for threads created after 2022-01-09
        /// </summary>
        public DiscordOptional<DateTimeOffset> CreateTimestamp { get; init; }
    }
}
