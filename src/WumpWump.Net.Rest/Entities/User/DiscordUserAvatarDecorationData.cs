namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// The data for the user's <a href="https://support.discord.com/hc/en-us/articles/13410113109911-Avatar-Decorations">avatar decoration</a>.
    /// </summary>
    public record DiscordUserAvatarDecorationData
    {
        /// <summary>
        /// the <a href="https://discord.com/developers/docs/reference#image-formatting">avatar decoration hash</a>
        /// </summary>
        public required string Asset { get; init; }

        /// <summary>
        /// id of the avatar decoration's SKU
        /// </summary>
        public required DiscordSnowflake SkuId { get; init; }
    }
}
