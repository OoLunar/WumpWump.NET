using System.Collections.Generic;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Gateway.Commands
{
    /// <summary>
    /// Used to request soundboard sounds for a list of guilds. The server will send
    /// <a href="https://discord.com/developers/docs/events/gateway-events#soundboard-sounds">Soundboard Sounds</a>
    /// events for each guild in response.
    /// </summary>
    public readonly record struct DiscordRequestSoundboardSoundsCommand
    {
        public required IReadOnlyList<DiscordSoundboardSound> SoundboardSounds { get; init; }
        public required DiscordSnowflake GuildId { get; init; }
    }
}