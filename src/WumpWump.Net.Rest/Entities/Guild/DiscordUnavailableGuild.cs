namespace WumpWump.Net.Rest.Entities
{
    public record DiscordUnavailableGuild
    {
        /// <summary>
        /// guild id
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// Whether the guild is unavailable
        /// </summary>
        public required bool Unavailable { get; init; }
    }
}
