namespace WumpWump.Net.Entities
{
    /// <summary>
    /// The Activity Location is an object that describes the location in which an activity instance is running.
    /// </summary>
    public record DiscordApplicationActivityLocation
    {
        /// <summary>
        /// Unique identifier for the location
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// Enum describing kind of location
        /// </summary>
        /// <remarks>
        /// Will either be <c>gc</c> or <c>pc</c>.
        /// </remarks>
        public required string Kind { get; init; }

        /// <summary>
        /// ID of the <see cref="DiscordChannel"/>
        /// </summary>
        public required DiscordSnowflake ChannelId { get; init; }

        /// <summary>
        /// ID of the <see cref="DiscordGuild"/>
        /// </summary>
        public DiscordOptional<DiscordSnowflake?> GuildId { get; init; }
    }
}
