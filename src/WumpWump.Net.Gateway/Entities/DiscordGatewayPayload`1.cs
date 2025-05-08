using System.Text.Json.Serialization;

namespace WumpWump.Net.Gateway.Entities
{
    /// <inheritdoc />
    public record DiscordGatewayPayload<T> : IDiscordGatewayPayload<T>
    {
        /// <inheritdoc />
        [JsonPropertyName("op")]
        public required DiscordGatewayOpCode OpCode { get; init; }

        /// <inheritdoc />
        [JsonPropertyName("d")]
        public required T Data { get; init; }

        /// <inheritdoc />
        [JsonPropertyName("s")]
        public required ulong? Sequence { get; init; }

        /// <inheritdoc />
        [JsonPropertyName("t")]
        public required string? EventName { get; init; }
    }
}
