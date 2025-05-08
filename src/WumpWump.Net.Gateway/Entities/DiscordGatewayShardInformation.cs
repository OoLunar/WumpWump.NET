namespace WumpWump.Net.Gateway.Entities
{
    /// <summary>
    /// Represents the information about a shard.
    /// </summary>
    /// <param name="ShardId">The ID of the shard.</param>
    /// <param name="ShardCount">The total number of shards.</param>
    public record DiscordGatewayShardInformation
    {
        /// <summary>
        /// The ID of the shard.
        /// </summary>
        public required int ShardId { get; init; }

        /// <summary>
        /// The total number of shards.
        /// </summary>
        public required int ShardCount { get; init; }
    }
}
