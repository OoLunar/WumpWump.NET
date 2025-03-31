namespace WumpWump.Net.Entities
{
    public record DiscordIntegrationApplication
    {
        /// <summary>
        /// the id of the app
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// the name of the app
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// the <a href="https://discord.com/developers/docs/reference#image-formatting">icon hash</a> of the app
        /// </summary>
        public required string? Icon { get; init; }

        /// <summary>
        /// the description of the app
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// the bot associated with this application
        /// </summary>
        public DiscordOptional<DiscordUser> Bot { get; init; }
    }
}
