using System;

namespace WumpWump.Net.Entities
{
    /// <summary>
    /// The thread metadata object contains a number of thread-specific channel fields that are not needed by other channel types.
    /// </summary>
    public record DiscordChannelThreadMetadata
    {
        /// <summary>
        /// whether the thread is archived
        /// </summary>
        public required bool Archived { get; init; }

        /// <summary>
        /// the thread will stop showing in the channel list after <see cref="DiscordChannelDefaultAutoArchiveDuration"/> minutes of inactivity,
        /// can be set to: <see cref="DiscordChannelDefaultAutoArchiveDuration.OneHour"/>, <see cref="DiscordChannelDefaultAutoArchiveDuration.OneDay"/>,
        /// <see cref="DiscordChannelDefaultAutoArchiveDuration.ThreeDays"/>, <see cref="DiscordChannelDefaultAutoArchiveDuration.OneWeek"/>
        /// </summary>
        public required DiscordChannelDefaultAutoArchiveDuration AutoArchiveDuration { get; init; }

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
