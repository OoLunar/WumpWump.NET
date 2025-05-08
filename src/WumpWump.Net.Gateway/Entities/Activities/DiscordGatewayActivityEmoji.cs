using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Entities
{
    public record DiscordGatewayActivityEmoji
    {
        /// <summary>
        /// Name of the emoji
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// ID of the emoji
        /// </summary>
        public DiscordOptional<DiscordSnowflake> Id { get; init; }

        /// <summary>
        /// Whether the emoji is animated
        /// </summary>
        public DiscordOptional<bool> Animated { get; init; }
    }
}
