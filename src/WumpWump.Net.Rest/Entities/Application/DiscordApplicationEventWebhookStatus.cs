namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Status indicating whether event webhooks are enabled or disabled for an application.
    /// </summary>
    public enum DiscordApplicationEventWebhookStatus
    {
        /// <summary>
        /// Webhook events are disabled by developer
        /// </summary>
        Disabled = 1,

        /// <summary>
        /// Webhook events are enabled by developer
        /// </summary>
        Enabled = 2,

        /// <summary>
        /// Webhook events are disabled by Discord, usually due to inactivity
        /// </summary>
        DisabledByDiscord = 3
    }
}
