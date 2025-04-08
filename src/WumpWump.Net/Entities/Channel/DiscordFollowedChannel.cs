namespace WumpWump.Net.Entities
{
    public record DiscordFollowedChannel
    {
        /// <summary>
        /// source channel id
        /// </summary>
        public required DiscordSnowflake ChannelId { get; init; }

        /// <summary>
        /// created target webhook id
        /// </summary>
        public required DiscordSnowflake WebhookId { get; init; }
    }
}