using System;

namespace WumpWump.Net.Rest.Entities
{
    public record DiscordGatewayInformation
    {
        /// <summary>
        /// WSS URL that can be used for connecting to the Gateway
        /// </summary>
        public required Uri Url { get; init; }
    }
}
