namespace WumpWump.Net.Entities
{
    /// <summary>
    /// An object that represents a tag that is able to be applied to a thread in a <see cref="DiscordChannelType.GuildForum"/> or <see cref="DiscordChannelType.GuildMedia"/> channel.
    /// </summary>
    /// <remarks>
    /// When updating a <see cref="DiscordChannelType.GuildForum"/> or a <see cref="DiscordChannelType.GuildMedia"/> channel, tag objects in <see cref="DiscordChannel.AvailableTags"/> only require the name field.
    /// </remarks>
    public record DiscordChannelForumTag
    {
        /// <summary>
        /// the id of the tag
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// the name of the tag (0-20 characters)
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// whether this tag can only be added to or removed from threads by a member with the <see cref="DiscordPermission.ManageThreads"/> permission
        /// </summary>
        public required bool Moderated { get; init; }

        /// <summary>
        /// the id of a guild's custom emoji *
        /// </summary>
        /// <remarks>
        /// At most one of <see cref="EmojiId"/> and <see cref="EmojiName"/> may be set to a non-null value.
        /// </remarks>
        public required DiscordSnowflake? EmojiId { get; init; }

        /// <summary>
        /// the unicode character of the emoji *
        /// </summary>
        /// <remarks>
        /// At most one of <see cref="EmojiId"/> and <see cref="EmojiName"/> may be set to a non-null value.
        /// </remarks>
        public required string? EmojiName { get; init; }
    }
}
