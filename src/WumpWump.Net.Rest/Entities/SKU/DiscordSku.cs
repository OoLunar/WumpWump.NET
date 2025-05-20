namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// SKUs (stock-keeping units) in Discord represent premium offerings that can be made available to your application's users or guilds.
    /// </summary>
    public record DiscordSku
    {
        /// <summary>
        /// ID of SKU
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// Type of SKU
        /// </summary>
        public required DiscordSkuType Type { get; init; }

        /// <summary>
        /// ID of the parent application
        /// </summary>
        public required DiscordSnowflake ApplicationId { get; init; }

        /// <summary>
        /// Customer-facing name of your premium offering
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// System-generated URL slug based on the SKU's name
        /// </summary>
        public required string Slug { get; init; }

        /// <summary>
        /// SKU flags combined as a bitfield
        /// </summary>
        public required DiscordSkuFlags Flags { get; init; }
    }
}