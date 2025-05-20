namespace WumpWump.Net.Rest.JsonParameterModels
{
    public class DiscordCreateTestEntitlementJsonParameterModel
    {
        /// <summary>
        /// ID of the SKU to grant the entitlement to
        /// </summary>
        public required string SkuId { get; set; }

        /// <summary>
        /// ID of the guild or user to grant the entitlement to
        /// </summary>
        public required string OwnerId { get; set; }

        /// <summary>
        /// <see cref="DiscordCreateTestEntitlementOwnerType.GuildSubscription"/> for a guild subscription, <see cref="DiscordCreateTestEntitlementOwnerType.UserSubscription"/> for a user subscription
        /// </summary>
        public required DiscordCreateTestEntitlementOwnerType OwnerType { get; set; }
    }

    public enum DiscordCreateTestEntitlementOwnerType
    {
        GuildSubscription = 1,
        UserSubscription = 2
    }
}