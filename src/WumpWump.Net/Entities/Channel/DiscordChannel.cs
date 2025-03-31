using System;
using System.Collections.Generic;

namespace WumpWump.Net.Entities
{
    public record DiscordChannel
    {
        public required DiscordSnowflake Id { get; init; }
        public required DiscordChannelType Type { get; init; }
        public DiscordOptional<DiscordSnowflake> GuildId { get; init; }
        public DiscordOptional<int> Position { get; init; }
        public DiscordOptional<IReadOnlyList<DiscordPermissionOverwrite>> PermissionOverwrites { get; init; }
        public DiscordOptional<string?> Name { get; init; }
        public DiscordOptional<string?> Topic { get; init; }
        public DiscordOptional<bool> Nsfw { get; init; }
        public DiscordOptional<DiscordSnowflake?> LastMessageId { get; init; }
        public DiscordOptional<int> Bitrate { get; init; }
        public DiscordOptional<int> UserLimit { get; init; }
        public DiscordOptional<int> RateLimitPerUser { get; init; }
        public DiscordOptional<IReadOnlyList<DiscordUser>> Recipients { get; init; }
        public DiscordOptional<string?> Icon { get; init; }
        public DiscordOptional<DiscordSnowflake?> OwnerId { get; init; }
        public DiscordOptional<DiscordSnowflake?> ApplicationId { get; init; }
        public DiscordOptional<bool> Managed { get; init; }
        public DiscordOptional<DiscordSnowflake?> ParentId { get; init; }
        public DiscordOptional<DateTimeOffset?> LastPinTimestamp { get; init; }
        public DiscordOptional<string?> RtcRegion { get; init; }
        public DiscordOptional<DiscordChannelVideoQualityMode> VideoQualityMode { get; init; }
        public DiscordOptional<int> MessageCount { get; init; }
        public DiscordOptional<int> MemberCount { get; init; }
        public DiscordOptional<DiscordChannelThreadMetadata> ThreadMetadata { get; init; }
        public DiscordOptional<DiscordChannelThreadMember> Member { get; init; }
        public DiscordOptional<DiscordChannelDefaultAutoArchiveDuration> DefaultAutoArchiveDuration { get; init; }
        public DiscordOptional<DiscordPermissions> Permissions { get; init; }
        public DiscordOptional<DiscordChannelFlags> Flags { get; init; }
        public DiscordOptional<int> TotalMessageSent { get; init; }
        public DiscordOptional<IReadOnlyList<DiscordChannelForumTag>> AvailableTags { get; init; }
        public DiscordOptional<IReadOnlyList<DiscordSnowflake>> AppliedTags { get; init; }
        public DiscordOptional<DiscordChannelDefaultReaction> DefaultReactionEmoji { get; init; }
        public DiscordOptional<int> DefaultThreadRateLimitPerUser { get; init; }
        public DiscordOptional<DiscordChannelSortOrderTypes?> DefaultSortOrder { get; init; }
        public DiscordOptional<DiscordChannelDefaultForumLayoutTypes> DefaultForumLayout { get; init; }
    }
}
