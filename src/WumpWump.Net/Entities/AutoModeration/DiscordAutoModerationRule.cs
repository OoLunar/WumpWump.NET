using System.Collections.Generic;

namespace WumpWump.Net.Entities
{
    /// <summary>
    /// Auto Moderation is a feature which allows each guild to set up rules that trigger based on some criteria.
    /// For example, a rule can trigger whenever a message contains a specific keyword.
    /// <br/>
    /// Rules can be configured to automatically execute actions whenever they trigger. For example, if a user tries
    /// to send a message which contains a certain keyword, a rule can trigger and block the message before it is sent.
    /// </summary>
    public record DiscordAutoModerationRule
    {
        /// <summary>
        /// the id of this rule
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// the id of the guild which this rule belongs to
        /// </summary>
        public required DiscordSnowflake GuildId { get; init; }

        /// <summary>
        /// the rule name
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// the user which first created this rule
        /// </summary>
        public required DiscordSnowflake CreatorId { get; init; }

        /// <summary>
        /// the rule event type
        /// </summary>
        public required DiscordAutoModerationEventType EventType { get; init; }

        /// <summary>
        /// the rule trigger type
        /// </summary>
        public required DiscordAutoModerationTriggerType TriggerType { get; init; }

        /// <summary>
        /// the rule trigger metadata
        /// </summary>
        public required DiscordAutoModerationTriggerMetadata TriggerMetadata { get; init; }

        /// <summary>
        /// the actions which will execute when the rule is triggered
        /// </summary>
        public required IReadOnlyList<DiscordAutoModerationAction> Actions { get; init; }

        /// <summary>
        /// whether the rule is enabled
        /// </summary>
        public required bool Enabled { get; init; }

        /// <summary>
        /// the role ids that should not be affected by the rule (Maximum of 20)
        /// </summary>
        public required IReadOnlyList<DiscordSnowflake> ExcemptRoles { get; init; }

        /// <summary>
        /// the channel ids that should not be affected by the rule (Maximum of 50)
        /// </summary>
        public required IReadOnlyList<DiscordSnowflake> ExcemptChannels { get; init; }
    }
}