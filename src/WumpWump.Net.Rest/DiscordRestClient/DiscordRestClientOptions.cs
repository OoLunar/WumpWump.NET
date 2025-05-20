using System;

namespace WumpWump.Net.Rest
{
    public class DiscordRestClientOptions
    {
        public required string DiscordToken { get; init; }
        public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
    }
}
