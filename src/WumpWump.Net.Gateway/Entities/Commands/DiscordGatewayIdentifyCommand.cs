using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Entities.Commands
{
    /// <summary>
    /// Used to trigger the initial handshake with the gateway.
    /// </summary>
    /// <remarks>
    /// Details about identifying is in the <a href="https://discord.com/developers/docs/events/gateway#identifying">Gateway documentation</a>.
    /// </remarks>
    [DiscordGatewayEvent(DiscordGatewayOpCode.Identify, null)]
    public record DiscordGatewayIdentifyCommand
    {
        /// <summary>
        /// Authentication token
        /// </summary>
        public required string Token { get; init; }

        /// <summary>
        /// <see cref="DiscordGatewayIdentifyCommandProperties"/>
        /// </summary>
        public required DiscordGatewayIdentifyCommandProperties Properties { get; init; }

        /// <summary>
        /// Whether this connection supports compression of packets
        /// </summary>
        /// <remarks>
        /// If this is not provided, it is assumed to be <see langword="false"/>.
        /// </remarks>
        public DiscordOptional<bool> Compress { get; init; }

        /// <summary>
        /// Value between 50 and 250, total number of members where the gateway will stop sending offline members in the guild member list.
        /// </summary>
        /// <remarks>
        /// If this is not provided, it is assumed to be 50.
        /// </remarks>
        public DiscordOptional<int> LargeThreshold { get; init; }

        /// <summary>
        /// Used for <a href="https://discord.com/developers/docs/events/gateway#sharding">Guild Sharding</a>.
        /// </summary>
        public DiscordOptional<DiscordGatewayShardInformation> Shard { get; init; }

        /// <summary>
        /// Presence structure for initial presence information.
        /// </summary>
        public DiscordOptional<DiscordGatewayUpdatePresenceCommand> Presence { get; init; }

        /// <summary>
        /// <see cref="DiscordGatewayIntents"/> you wish to receive.
        /// </summary>
        public required DiscordGatewayIntents Intents { get; init; }
    }
}
