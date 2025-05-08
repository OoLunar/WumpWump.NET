namespace WumpWump.Net.Rest.Entities
{
    public record DiscordIntegrationAccount
    {
        /// <summary>
        /// id of the account
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// name of the account
        /// </summary>
        public required string Name { get; init; }
    }
}
