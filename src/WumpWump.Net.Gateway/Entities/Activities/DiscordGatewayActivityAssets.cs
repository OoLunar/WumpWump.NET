using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Entities
{
    public record DiscordGatewayActivityAssets
    {
        /// <summary>
        /// See <a href="https://discord.com/developers/docs/events/gateway-events#activity-object-activity-asset-image">Activity Asset Image</a>
        /// </summary>
        public DiscordOptional<DiscordSnowflake> LargeImage { get; init; }

        /// <summary>
        /// Text displayed when hovering over the large image of the activity
        /// </summary>
        public DiscordOptional<string> LargeText { get; init; }

        /// <summary>
        /// See <a href="https://discord.com/developers/docs/events/gateway-events#activity-object-activity-asset-image">Activity Asset Image</a>
        /// </summary>
        public DiscordOptional<DiscordSnowflake> SmallImage { get; init; }

        /// <summary>
        /// Text displayed when hovering over the small image of the activity
        /// </summary>
        public DiscordOptional<string> SmallText { get; init; }
    }
}
