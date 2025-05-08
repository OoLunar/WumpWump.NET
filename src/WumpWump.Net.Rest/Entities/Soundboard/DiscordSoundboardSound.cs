namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Users can play soundboard sounds in voice channels, triggering a
    /// <a href="https://discord.com/developers/docs/events/gateway-events#voice-channel-effect-send">Voice Channel Effect Send</a>
    /// Gateway event for users connected to the voice channel.
    /// <br/>
    /// There is a set of
    /// <a href="https://discord.com/developers/docs/resources/soundboard#list-default-soundboard-sounds">default sounds</a>
    /// available to all users. Soundboard sounds can also be <a href="https://discord.com/developers/docs/resources/soundboard#create-guild-soundboard-sound">created in a guild</a>;
    /// users will be able to use the sounds in the guild, and Nitro subscribers can use them in all guilds.
    /// <br>
    /// Soundboard sounds in a set of guilds can be retrieved over the Gateway using <a href="https://discord.com/developers/docs/events/gateway-events#request-soundboard-sounds">Request Soundboard Sounds</a>.
    /// </summary>
    public record DiscordSoundboardSound
    {
        /// <summary>
        /// the name of this sound
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// the id of this sound
        /// </summary>
        public required DiscordSnowflake SoundId { get; init; }

        /// <summary>
        /// the volume of this sound, from 0 to 1
        /// </summary>
        public required double Volume { get; init; }

        /// <summary>
        /// the id of this sound's custom emoji
        /// </summary>
        public required DiscordSnowflake? EmojiId { get; init; }

        /// <summary>
        /// the unicode character of this sound's standard emoji
        /// </summary>
        public required string? EmojiName { get; init; }

        /// <summary>
        /// the id of the guild this sound is in
        /// </summary>
        public DiscordOptional<DiscordSnowflake> GuildId { get; init; }

        /// <summary>
        /// whether this sound can be used, may be false due to loss of Server Boosts
        /// </summary>
        public required bool Available { get; init; }

        /// <summary>
        /// the user who created this sound
        /// </summary>
        public DiscordOptional<DiscordUser> User { get; init; }
    }
}
