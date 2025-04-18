namespace WumpWump.Net.Gateway.Payloads
{
    public readonly record struct DiscordHelloPayload
    {
        /// <summary>
        /// Interval (in milliseconds) an app should heartbeat with
        /// </summary>
        public required int HeartbeatInterval { get; init; }
    }
}