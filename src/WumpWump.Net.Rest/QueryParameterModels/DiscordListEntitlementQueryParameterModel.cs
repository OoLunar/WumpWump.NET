using System.Collections.Generic;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.QueryParameterModels
{
    public class DiscordListEntitlementQueryParameterModel
    {
        /// <summary>
        /// User ID to look up entitlements for
        /// </summary>
        public DiscordOptional<DiscordSnowflake> UserId { get; set; }

        /// <summary>
        /// Optional list of SKU IDs to check entitlements for
        /// </summary>
        public DiscordOptional<IList<DiscordSnowflake>> SkuIds { get; set; }

        /// <summary>
        /// Retrieve entitlements before this entitlement ID
        /// </summary>
        public DiscordOptional<DiscordSnowflake> Before { get; set; }

        /// <summary>
        /// Retrieve entitlements after this entitlement ID
        /// </summary>
        public DiscordOptional<DiscordSnowflake> After { get; set; }

        /// <summary>
        /// Number of entitlements to return, 1-100, default 100
        /// </summary>
        public DiscordOptional<int> Limit { get; set; }

        /// <summary>
        /// Guild ID to look up entitlements for
        /// </summary>
        public DiscordOptional<DiscordSnowflake> GuildId { get; set; }

        /// <summary>
        /// Whether or not ended entitlements should be omitted. Defaults to false, ended entitlements are included by default.
        /// </summary>
        public DiscordOptional<bool> ExcludeEnded { get; set; }

        /// <summary>
        /// Whether or not deleted entitlements should be omitted. Defaults to true, deleted entitlements are not included by default.
        /// </summary>
        public DiscordOptional<bool> ExcludeDeleted { get; set; }
    }
}