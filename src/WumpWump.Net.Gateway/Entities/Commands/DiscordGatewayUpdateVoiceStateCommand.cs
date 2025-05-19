using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Entities.Commands
{
    /// <summary>
    /// Sent when a client wants to join, move, or disconnect from a voice channel.
    /// </summary>
    [DiscordGatewayEvent(DiscordGatewayOpCode.VoiceStateUpdate, null)]
    public record DiscordGatewayUpdateVoiceStateCommand
    {
        /// <summary>
        /// ID of the guild
        /// </summary>
        public required DiscordSnowflake GuildId { get; init; }

        /// <summary>
        /// ID of the voice channel client wants to join (<see langword="null"/> if disconnecting)
        /// </summary>
        public required DiscordSnowflake? ChannelId { get; init; }

        /// <summary>
        /// Whether the client is muted
        /// </summary>
        public required bool SelfMute { get; init; }

        /// <summary>
        /// Whether the client deafened
        /// </summary>
        public required bool SelfDeaf { get; init; }
    }
}
