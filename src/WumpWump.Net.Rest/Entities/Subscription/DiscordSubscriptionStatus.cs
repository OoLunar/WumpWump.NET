namespace WumpWump.Net.Rest.Entities
{
    /// <remarks>
    /// Subscription status should not be used to grant perks. Use entitlements as an indication of
    /// whether a user should have access to a specific SKU. See our guide on Implementing App
    /// Subscriptions for more information.
    /// </remarks>
    public enum DiscordSubscriptionStatus
    {
        /// <summary>
        /// Subscription is active and scheduled to renew.
        /// </summary>
        Active = 0,

        /// <summary>
        /// Subscription is active but will not renew.
        /// </summary>
        Ending = 1,

        /// <summary>
        /// Subscription is inactive and not being charged.
        /// </summary>
        Inactive = 2
    }
}