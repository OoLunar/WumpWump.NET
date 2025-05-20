namespace WumpWump.Net.Rest.Entities
{
    public record DiscordGatewaySessionStartLimit
    {
        /// <summary>
        /// Total number of session starts the current user is allowed
        /// </summary>
        public required int Total { get; init; }

        /// <summary>
        /// Remaining number of session starts the current user is allowed
        /// </summary>
        public required int Remaining { get; init; }

        /// <summary>
        /// Number of milliseconds after which the limit resets
        /// </summary>
        public required int ResetAfter { get; init; }

        /// <summary>
        /// Number of identify requests allowed per 5 seconds
        /// </summary>
        public required int MaxConcurrency { get; init; }
    }
}
