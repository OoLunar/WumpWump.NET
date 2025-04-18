namespace WumpWump.Net.Gateway.Entities
{
    /// <inheritdoc />
    public readonly record struct DiscordGatewayPayload<T> : IDiscordGatewayPayload<T>
    {
        /// <inheritdoc />
        public required DiscordGatewayOpCode OpCode { get; init; }

        /// <inheritdoc />
        public required T Data { get; init; }

        /// <inheritdoc />
        public required int? Sequence { get; init; }

        /// <inheritdoc />
        public required string? EventName { get; init; }
    }
}