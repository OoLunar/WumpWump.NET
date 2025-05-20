using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.QueryParameterModels
{
    public class DiscordListSubscriptionQueryParameterModel
    {
        /// <summary>
        /// List subscriptions before this ID
        /// </summary>
        public DiscordOptional<DiscordSnowflake> Before { get; set; }

        /// <summary>
        /// List subscriptions after this ID
        /// </summary>
        public DiscordOptional<DiscordSnowflake> After { get; set; }

        /// <summary>
        /// Number of results to return (1-100)
        /// </summary>
        public DiscordOptional<int> Limit { get; set; }

        /// <summary>
        /// User ID for which to return subscriptions. Required except for OAuth queries.
        /// </summary>
        public DiscordOptional<DiscordSnowflake> UserId { get; set; }
    }
}