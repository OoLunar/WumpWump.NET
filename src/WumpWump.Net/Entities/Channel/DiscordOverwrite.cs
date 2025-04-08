namespace WumpWump.Net.Entities
{
    public record DiscordOverwrite
    {
        /// <summary>
        /// role or user id
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// Either <see cref="DiscordOverwriteType.Role"/> or <see cref="DiscordOverwriteType.Member"/>
        /// </summary>
        public required DiscordOverwriteType Type { get; init; }

        /// <summary>
        /// permission bit set
        /// </summary>
        public required DiscordPermissionContainer Allow { get; init; }

        /// <summary>
        /// permission bit set
        /// </summary>
        public required DiscordPermissionContainer Deny { get; init; }
    }
}