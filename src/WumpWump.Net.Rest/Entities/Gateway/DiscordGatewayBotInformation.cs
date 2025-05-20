namespace WumpWump.Net.Rest.Entities.Gateway
{
    public record DiscordGatewayBotInformation : DiscordGatewayInformation
    {
        /// <summary>
        /// Recommended number of <a href="https://discord.com/developers/docs/events/gateway#sharding">shards</a> to use when connecting
        /// </summary>
        public required int Shards { get; init; }

        /// <summary>
        /// Information on the current session start limit
        /// </summary>
        public required DiscordGatewaySessionStartLimit SessionStartLimit { get; init; }
    }
}
