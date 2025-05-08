namespace WumpWump.Net.Rest.Entities
{
    public record DiscordRoleTags
    {
        public DiscordOptional<DiscordSnowflake> BotId { get; init; }
        public DiscordOptional<DiscordSnowflake> IntegrationId { get; init; }
        public DiscordOptional<object?> PremiumSubscriber { get; init; }
        public DiscordOptional<DiscordSnowflake> SubscriptionListingId { get; init; }
        public DiscordOptional<object?> AvailableForPurchase { get; init; }
        public DiscordOptional<object?> GuildConnections { get; init; }
    }
}
