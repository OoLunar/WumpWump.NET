namespace WumpWump.Net.Entities
{
    public enum DiscordVerificationLevel
    {
        /// <summary>
        /// unrestricted
        /// </summary>
        None = 0,

        /// <summary>
        /// must have verified email on account
        /// </summary>
        Low = 1,

        /// <summary>
        /// must be registered on Discord for longer than 5 minutes
        /// </summary>
        Medium = 2,

        /// <summary>
        /// must be a member of the server for longer than 10 minutes
        /// </summary>
        High = 3,

        /// <summary>
        /// must have a verified phone number
        /// </summary>
        VeryHigh = 4
    }
}