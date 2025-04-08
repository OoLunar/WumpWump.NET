namespace WumpWump.Net.Entities
{
    /// <summary>
    /// Additional data used when an action is executed. Different fields are relevant based on the value of action type.
    /// </summary>
    public record DiscordAutoModerationActionMetadata
    {
        /// <summary>
        /// channel to which user content should be logged
        /// </summary>
        /// <remarks>
        /// Associated Action Types: <see cref="DiscordAutoModerationActionType.SendAlertMessage"/>. Constraints: existing channel.
        /// </remarks>
        public required DiscordSnowflake ChannelId { get; init; }

        /// <summary>
        /// timeout duration in seconds
        /// </summary>
        /// <remarks>
        /// Associated Action Types: <see cref="DiscordAutoModerationActionType.Timeout"/>. Constraints: maximum of 2419200 seconds (4 weeks)
        /// </remarks>
        public required int DurationSeconds { get; init; }

        /// <summary>
        /// additional explanation that will be shown to members whenever their message is blocked
        /// </summary>
        /// <remarks>
        /// Associated Action Types: <see cref="DiscordAutoModerationActionType.BlockMessage"/>. Constraints: maximum of 150 characters.
        /// </remarks>
        public DiscordOptional<string> CustomMessage { get; init; }
    }
}