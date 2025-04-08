using System;

namespace WumpWump.Net.Entities
{
    public record DiscordEntitlement
    {
        public required DiscordSnowflake Id { get; init; }
        public required DiscordSnowflake SkuId { get; init; }
        public required DiscordSnowflake ApplicationId { get; init; }
        public DiscordOptional<DiscordSnowflake> UserId { get; init; }
        public required DiscordEntitlementType Type { get; init; }
        public required bool Deleted { get; init; }
        public required DateTimeOffset? StartsAt { get; init; }
        public required DateTimeOffset? EndsAt { get; init; }
        public DiscordOptional<DiscordSnowflake> GuildId { get; init; }
        public DiscordOptional<bool> Consumed { get; init; }
    }
}