using System;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Entities
{
    /// <summary>
    /// Represents the session information sent by the Discord Gateway.
    /// </summary>
    public record DiscordGatewaySessionInformation
    {
        /// <summary>
        /// The information to use when connecting fresh to the gateway.
        /// </summary>
        public required DiscordGatewayInformation GatewayInformation { get; init; }

        /// <summary>
        /// The URL of the gateway.
        /// </summary>
        public required Uri ResumeUrl { get; init; }

        /// <summary>
        /// The session ID.
        /// </summary>
        public string? SessionId { get; init; }

        /// <summary>
        /// The last sequence number received.
        /// </summary>
        public ulong? LastSequence { get; init; }

        /// <summary>
        /// The time taken between a <see cref="DiscordGatewayOpCode.Heartbeat"/> to be sent and a <see cref="DiscordGatewayOpCode.HeartbeatAck"/> to be received.
        /// </summary>
        public TimeSpan? Ping { get; init; }
    }
}