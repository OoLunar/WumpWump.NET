using System.Collections.Generic;

namespace WumpWump.Net.Entities
{
    public record DiscordWelcomeScreen
    {
        public required string? Description { get; init; }
        public required IReadOnlyList<DiscordWelcomeScreenChannel> Channels { get; init; }
    }
}