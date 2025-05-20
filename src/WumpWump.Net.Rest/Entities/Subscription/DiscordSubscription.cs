using System;
using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Subscriptions in Discord represent a user making recurring payments for at least one SKU over an
    /// ongoing period. Successful payments grant the user access to entitlements associated with the SKU.
    /// </summary>
    public record DiscordSubscription
    {
        /// <summary>
        /// ID of the subscription
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// ID of the user who is subscribed
        /// </summary>
        public required DiscordSnowflake UserId { get; init; }

        /// <summary>
        /// List of SKUs subscribed to
        /// </summary>
        public required IReadOnlyList<DiscordSnowflake> SkuIds { get; init; }

        /// <summary>
        /// List of entitlements granted for this subscription
        /// </summary>
        public required IReadOnlyList<DiscordSnowflake> EntitlementIds { get; init; }

        /// <summary>
        /// List of SKUs that this user will be subscribed to at renewal
        /// </summary>
        public required IReadOnlyList<DiscordSnowflake>? RenewalSkuIds { get; init; }

        /// <summary>
        /// Start of the current subscription period
        /// </summary>
        public required DateTimeOffset CurrentPeriodStart { get; init; }

        /// <summary>
        /// End of the current subscription period
        /// </summary>
        public required DateTimeOffset CurrentPeriodEnd { get; init; }

        /// <summary>
        /// Current status of the subscription
        /// </summary>
        public required DiscordSubscriptionStatus Status { get; init; }

        /// <summary>
        /// When the subscription was canceled
        /// </summary>
        public required DateTimeOffset? CancelledAt { get; init; }

        /// <summary>
        /// ISO3166-1 alpha-2 country code of the payment source used to purchase
        /// the subscription. Missing unless queried with a private OAuth scope.
        /// </summary>
        public DiscordOptional<string?> Country { get; init; }
    }
}