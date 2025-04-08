namespace WumpWump.Net.Entities
{
    public enum DiscordAutoModerationKeywordPresetType
    {
        /// <summary>
        /// words that may be considered forms of swearing or cursing
        /// </summary>
        Profanity = 1,

        /// <summary>
        /// words that refer to sexually explicit behavior or activity
        /// </summary>
        SexualContent = 2,

        /// <summary>
        /// personal insults or words that may be considered hate speech
        /// </summary>
        Slurs = 3
    }
}