namespace WumpWump.Net.Entities
{
    public record DiscordWelcomeScreenChannel
    {
        /// <summary>
        /// the channel's id
        /// </summary>
        public required DiscordSnowflake ChannelId { get; init; }

        /// <summary>
        /// the description shown for the channel
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// the emoji id, if the emoji is custom
        /// </summary>
        public required DiscordSnowflake? EmojiId { get; init; }

        /// <summary>
        /// the emoji name if custom, the unicode character if standard, or null if no emoji is set
        /// </summary>
        public required string? EmojiName { get; init; }
    }
}