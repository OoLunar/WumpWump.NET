namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Characterizes the type of content which can trigger the rule.
    /// </summary>
    public enum DiscordAutoModerationTriggerType
    {
        /// <summary>
        /// check if content contains words from a user defined list of keywords
        /// </summary>
        Keyword = 1,

        /// <summary>
        /// check if content represents generic spam
        /// </summary>
        Spam = 3,

        /// <summary>
        /// check if content contains words from internal pre-defined wordsets
        /// </summary>
        KeywordPreset = 4,

        /// <summary>
        /// check if content contains more unique mentions than allowed
        /// </summary>
        MentionSpam = 5,

        /// <summary>
        /// check if member profile contains words from a user defined list of keywords
        /// </summary>
        MemberProfile = 6
    }
}
