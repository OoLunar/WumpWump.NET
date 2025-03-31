using System.Text.Json.Serialization;

namespace WumpWump.Net.Gateway.Entities
{
    /// <summary>
    /// Gateway event payloads have a common structure, but the contents of the associated data (<see cref="Data"/>) varies between the different events.
    /// </summary>
    /// <remarks>
    /// <see cref="Sequence"/> and <see cref="EventName"/> are <see langword="null"/> when <see cref="OpCode"/> is not <see cref="DiscordGatewayOpCode.Dispatch"/>.
    /// </remarks>
    public readonly record struct DiscordGatewayPayload
    {
        /// <summary>
        /// <see cref="DiscordGatewayOpCode"/>, which indicates the payload type
        /// </summary>
        [JsonPropertyName("op")]
        public required DiscordGatewayOpCode OpCode { get; init; }

        /// <summary>
        /// Event data
        /// </summary>
        [JsonPropertyName("d")]
        public required object? Data { get; init; }

        /// <summary>
        /// Sequence number of event used for <a href="https://discord.com/developers/docs/events/gateway#resuming">resuming sessions</a> and <a href="https://discord.com/developers/docs/events/gateway#sending-heartbeats">heartbeating</a>
        /// </summary>
        [JsonPropertyName("s")]
        public required int? Sequence { get; init; }

        /// <summary>
        /// Event name
        /// </summary>
        [JsonPropertyName("t")]
        public required string? EventName { get; init; }
    }
}