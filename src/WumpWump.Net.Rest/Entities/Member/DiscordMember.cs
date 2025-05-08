using System;
using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    public record DiscordMember
    {
        /// <summary>
        /// the user this guild member represents
        /// </summary>
        /// <remarks>
        /// The field user won't be included in the member object attached to <c>MESSAGE_CREATE</c> and <c>MESSAGE_UPDATE</c> gateway events.
        /// </remarks>
        public DiscordOptional<DiscordUser> User { get; init; }

        /// <summary>
        /// this user's guild nickname
        /// </summary>
        public DiscordOptional<string?> Nick { get; init; }

        /// <summary>
        /// the member's <a href="https://discord.com/developers/docs/reference#image-formatting">guild avatar hash</a>
        /// </summary>
        public DiscordOptional<string?> Avatar { get; init; }

        /// <summary>
        /// the member's <a href="https://discord.com/developers/docs/reference#image-formatting">guild banner hash</a>
        /// </summary>
        public DiscordOptional<string?> Banner { get; init; }

        /// <summary>
        /// array of <see cref="DiscordRole"/> object ids
        /// </summary>
        public required IReadOnlyList<DiscordSnowflake> Roles { get; init; }

        /// <summary>
        /// when the user joined the guild
        /// </summary>
        public required DateTimeOffset JoinedAt { get; init; }

        /// <summary>
        /// when the user started <a href="https://support.discord.com/hc/en-us/articles/360028038352-Server-Boosting-FAQ">boosting</a> the guild
        /// </summary>
        public DiscordOptional<DateTimeOffset?> PremiumSince { get; init; }

        /// <summary>
        /// whether the user is deafened in voice channels
        /// </summary>
        public required bool Deaf { get; init; }

        /// <summary>
        /// whether the user is muted in voice channels
        /// </summary>
        public required bool Mute { get; init; }

        /// <summary>
        /// <see cref="DiscordMemberFlags"/> represented as a bit set, defaults to <see cref="DiscordMemberFlags.None"/>
        /// </summary>
        public required DiscordMemberFlags Flags { get; init; }

        /// <summary>
        /// whether the user has not yet passed the guild's <see cref="DiscordMembershipScreening"/> requirements
        /// </summary>
        /// <remarks>
        /// In <c>GUILD_</c> events, pending will always be included as <see langword="true"/> or <see langword="false"/>. In non <c>GUILD_</c> events which can only be triggered by non-<c>pending</c> users, <c>pending</c> will not be included.
        /// </remarks>
        public DiscordOptional<bool> Pending { get; init; }

        /// <summary>
        /// total permissions of the member in the channel, including overwrites, returned when in the interaction object
        /// </summary>
        public DiscordOptional<DiscordPermissionContainer> Permissions { get; init; }

        /// <summary>
        /// when the user's <a href="https://support.discord.com/hc/en-us/articles/4413305239191-Time-Out-FAQ">timeout</a> will expire and the user will be able to communicate in the guild again, null or a time in the past if the user is not timed out
        /// </summary>
        public DiscordOptional<DateTimeOffset?> CommunicationDisabledUntil { get; init; }

        /// <summary>
        /// data for the member's guild avatar decoration
        /// </summary>
        public DiscordOptional<DiscordUserAvatarDecorationData> AvatarDecorationData { get; init; }
    }
}
