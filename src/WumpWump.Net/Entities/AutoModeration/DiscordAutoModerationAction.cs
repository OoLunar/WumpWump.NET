namespace WumpWump.Net.Entities
{
    /// <summary>
    /// An action which will execute whenever a rule is triggered.
    /// </summary>
    public record DiscordAutoModerationAction
    {
        /// <summary>
        /// the type of action
        /// </summary>
        public required DiscordAutoModerationActionType Type { get; init; }

        /// <summary>
        /// additional metadata needed during execution for this specific action type
        /// </summary>
        /// <remarks>
        /// Can be omitted based on <see cref="Type"/>. See the Associated Action Types column in
        /// <a href="https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-action-object-action-metadata">action metadata</a>
        /// to understand which type values require metadata to be set.
        /// </remarks>
        public DiscordOptional<DiscordAutoModerationActionMetadata> Metadata { get; init; }
    }
}