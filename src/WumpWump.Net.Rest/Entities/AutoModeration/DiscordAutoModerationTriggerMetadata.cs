using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Additional data used to determine whether a rule should be triggered. Different fields are relevant based on the value of <see cref="DiscordAutoModerationRule.TriggerType"/>.
    /// </summary>
    public record DiscordAutoModerationTriggerMetadata
    {
        /// <summary>
        /// substrings which will be searched for in content (Maximum of 1000)
        /// </summary>
        public required IReadOnlyList<string> KeywordFilter { get; init; }

        /// <summary>
        /// regular expression patterns which will be matched against content (Maximum of 10)
        /// </summary>
        public required IReadOnlyList<string> RegexPatterns { get; init; }

        /// <summary>
        /// the internally pre-defined wordsets which will be searched for in content
        /// </summary>
        public required IReadOnlyList<DiscordAutoModerationKeywordPresetType> Presets { get; init; }

        /// <summary>
        /// substrings which should not trigger the rule (Maximum of 100 or 1000)
        /// </summary>
        public required IReadOnlyList<string> AllowList { get; init; }

        /// <summary>
        /// total number of unique role and user mentions allowed per message (Maximum of 50)
        /// </summary>
        public required int MentionTotalLimit { get; init; }

        /// <summary>
        /// whether to automatically detect mention raids
        /// </summary>
        public required bool MentionRaidProtectionEnabled { get; init; }
    }
}
