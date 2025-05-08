namespace WumpWump.Net.Rest.Entities
{
    public enum DiscordMfaLevel
    {
        /// <summary>
        /// guild has no MFA/2FA requirement for moderation actions
        /// </summary>
        None = 0,

        /// <summary>
        /// guild has a 2FA requirement for moderation actions
        /// </summary>
        Elevated = 1
    }
}
