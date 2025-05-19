using System.Collections.Generic;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Entities.Commands
{
    /// <summary>
    /// Used to request soundboard sounds for a list of guilds. The server will send
    /// <a href="https://discord.com/developers/docs/events/gateway-events#soundboard-sounds">Soundboard Sounds</a>
    /// events for each guild in response.
    /// </summary>
    [DiscordGatewayEvent(DiscordGatewayOpCode.RequestSoundboardSounds, null)]
    public record DiscordGatewayRequestSoundboardSoundsCommand
    {
        public required IReadOnlyList<DiscordSoundboardSound> SoundboardSounds { get; init; }
        public required DiscordSnowflake GuildId { get; init; }
    }
}
