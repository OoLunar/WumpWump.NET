using System;

namespace WumpWump.Net.Entities
{
    /// <summary>
    /// A thread member object contains information about a user that has joined a thread.
    /// </summary>
    public record DiscordThreadMember
    {
        /// <summary>
        /// ID of the thread
        /// </summary>
        public DiscordOptional<DiscordSnowflake> Id { get; init; }

        /// <summary>
        /// ID of the user
        /// </summary>
        /// <remarks>
        /// These fields are omitted on the member sent within each thread in the <a href="https://discord.com/developers/docs/events/gateway-events#guild-create">GUILD_CREATE</a> event.
        /// </remarks>
        public DiscordOptional<DiscordSnowflake> UserId { get; init; }

        /// <summary>
        /// Time the user last joined the thread
        /// </summary>
        public required DateTimeOffset JoinTimestamp { get; init; }

        /// <summary>
        /// Any user-thread settings, currently only used for notifications
        /// </summary>
        public required int Flags { get; init; }

        /// <summary>
        /// Additional information about the user
        /// </summary>
        /// <remarks>
        /// The member field is only present when <c>with_member</c> is set to true when calling List Thread Members or Get Thread Member.
        /// </remarks>
        public DiscordOptional<DiscordMember> Member { get; init; }
    }
}
