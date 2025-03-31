namespace WumpWump.Net.Entities
{
    /// <summary>
    /// An object that specifies the emoji to use as the default way to react to a forum post. Exactly one of <see cref="EmojiId"/> and <see cref="EmojiName"/> must be set.
    /// </summary>
    public record DiscordChannelDefaultReaction
    {
        /// <summary>
        /// The id of a guild's custom emoji.
        /// </summary>
        public required DiscordSnowflake? EmojiId { get; init; }

        /// <summary>
        /// The unicode character of the emoji.
        /// </summary>
        public required string? EmojiName { get; init; }
    }
}
