using WumpWump.Net.Gateway.Events;

namespace WumpWump.Net.Gateway.Entities
{
    /// <summary>
    /// Used to replay missed events when a disconnected client resumes.
    /// </summary>
    /// <remarks>
    /// Details about resuming are in the <a href="https://discord.com/developers/docs/events/gateway#resuming">Gateway documentation</a>.
    /// </remarks>
    [DiscordGatewayEvent(DiscordGatewayOpCode.Resume, null)]
    public record DiscordGatewayResumeCommand
    {
        /// <summary>
        /// Session token
        /// </summary>
        public required string Token { get; init; }

        /// <summary>
        /// Session ID
        /// </summary>
        public required string SessionId { get; init; }

        /// <summary>
        /// Last sequence number received
        /// </summary>
        public required ulong? Sequence { get; init; }
    }
}
