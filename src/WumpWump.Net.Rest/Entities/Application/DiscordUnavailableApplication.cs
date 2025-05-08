namespace WumpWump.Net.Rest.Entities
{
    public record DiscordUnavailableApplication
    {
        /// <summary>
        /// ID of the app
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// The flags of the application
        /// </summary>
        public required DiscordApplicationFlags Flags { get; init; }
    }
}
