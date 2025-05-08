using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Entities
{
    /// <remarks>
    /// For Listening and Watching activities, you can include both <see cref="Start"/> and <see cref="End"/> timestamps to display a time bar.
    /// </remarks>
    public record DiscordGatewayActivityTimestamps
    {
        /// <summary>
        /// Unix time (in milliseconds) of when the activity started
        /// </summary>
        public DiscordOptional<long> Start { get; init; }

        /// <summary>
        /// Unix time (in milliseconds) of when the activity ends
        /// </summary>
        public DiscordOptional<long> End { get; init; }
    }
}
