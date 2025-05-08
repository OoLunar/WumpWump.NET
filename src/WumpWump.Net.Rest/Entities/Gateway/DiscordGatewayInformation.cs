using System;

namespace WumpWump.Net.Rest.Entities.Gateway
{
    public record DiscordGatewayInformation
    {
        /// <summary>
        /// WSS URL that can be used for connecting to the Gateway
        /// </summary>
        public required Uri Url { get; init; }

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
