using System;

namespace WumpWump.Net.Gateway.Entities
{
    /// <summary>
    /// When received over the gateway, the buttons field is an array of strings, which are the button labels.
    /// Bots cannot access a user's activity button URLs.
    /// </summary>
    public record DiscordGatewayActivityButton
    {
        /// <summary>
        /// Text shown on the button (1-32 characters)
        /// </summary>
        public required string Label { get; init; }

        /// <summary>
        /// URL opened when clicking the button (1-512 characters)
        /// </summary>
        public required Uri Url { get; init; }
    }
}
