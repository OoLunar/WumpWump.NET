namespace WumpWump.Net.Rest.Entities
{
    public record DiscordSticker
    {
        /// <summary>
        /// id of the sticker
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// for standard stickers, id of the pack the sticker is from
        /// </summary>
        public DiscordOptional<DiscordSnowflake> PackId { get; init; }

        /// <summary>
        /// name of the sticker
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// description of the sticker
        /// </summary>
        public required string? Description { get; init; }

        /// <summary>
        /// autocomplete/suggestion tags for the sticker (max 200 characters)
        /// </summary>
        /// <remarks>
        /// A comma separated list of keywords is the format used in this field by standard stickers, but this is just a convention.
        /// Incidentally the client will always use a name generated from an emoji as the value of this field when creating or modifying
        /// a guild sticker.
        /// </remarks>
        public required string Tags { get; init; }

        /// <summary>
        /// type of sticker
        /// </summary>
        public required DiscordStickerType Type { get; init; }

        /// <summary>
        ///	type of sticker format
        /// </summary>
        public required DiscordStickerFormatType FormatType { get; init; }

        /// <summary>
        /// whether this guild sticker can be used, may be false due to loss of Server Boosts
        /// </summary>
        public DiscordOptional<bool> Available { get; init; }

        /// <summary>
        /// id of the guild that owns this sticker
        /// </summary>
        public DiscordOptional<DiscordSnowflake> GuildId { get; init; }

        /// <summary>
        /// the user that uploaded the guild sticker
        /// </summary>
        public DiscordOptional<DiscordUser> User { get; init; }

        /// <summary>
        /// the standard sticker's sort order within its pack
        /// </summary>
        public DiscordOptional<int> SortValue { get; init; }
    }
}
