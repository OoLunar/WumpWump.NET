using System;
using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    public record DiscordChannel
    {
        public required DiscordSnowflake Id { get; init; }
        public required DiscordChannelType Type { get; init; }
        public DiscordOptional<DiscordSnowflake> GuildId { get; init; }
        public DiscordOptional<int> Position { get; init; }
        public DiscordOptional<IReadOnlyList<DiscordOverwrite>> PermissionOverwrites { get; init; }
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
        public DiscordOptional<DiscordVideoQualityMode> VideoQualityMode { get; init; }
        public DiscordOptional<int> MessageCount { get; init; }
        public DiscordOptional<int> MemberCount { get; init; }
        public DiscordOptional<DiscordThreadMetadata> ThreadMetadata { get; init; }
        public DiscordOptional<DiscordThreadMember> Member { get; init; }
        public DiscordOptional<DiscordDefaultAutoArchiveDuration> DefaultAutoArchiveDuration { get; init; }
        public DiscordOptional<DiscordPermissionContainer> Permissions { get; init; }
        public DiscordOptional<DiscordChannelFlags> Flags { get; init; }
        public DiscordOptional<int> TotalMessageSent { get; init; }
        public DiscordOptional<IReadOnlyList<DiscordForumTag>> AvailableTags { get; init; }
        public DiscordOptional<IReadOnlyList<DiscordSnowflake>> AppliedTags { get; init; }
        public DiscordOptional<DiscordDefaultReaction> DefaultReactionEmoji { get; init; }
        public DiscordOptional<int> DefaultThreadRateLimitPerUser { get; init; }
        public DiscordOptional<DiscordSortOrderTypes?> DefaultSortOrder { get; init; }
        public DiscordOptional<DiscordForumLayoutTypes> DefaultForumLayout { get; init; }
    }
}
