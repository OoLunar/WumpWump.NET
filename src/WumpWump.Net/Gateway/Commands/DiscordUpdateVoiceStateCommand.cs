using WumpWump.Net.Entities;

namespace WumpWump.Net.Gateway.Commands
{
    /// <summary>
    /// Sent when a client wants to join, move, or disconnect from a voice channel.
    /// </summary>
    public readonly record struct DiscordUpdateVoiceStateCommand
    {
        /// <summary>
        /// ID of the guild
        /// </summary>
        public required DiscordSnowflake GuildId { get; init; }

        /// <summary>
        /// ID of the voice channel client wants to join (<see cref="null"/> if disconnecting)
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