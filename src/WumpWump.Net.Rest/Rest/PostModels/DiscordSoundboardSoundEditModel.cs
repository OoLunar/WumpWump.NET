using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.PostModels
{
    public record DiscordSoundboardSoundEditModel
    {
        /// <summary>
        /// name of the soundboard sound (2-32 characters)
        /// </summary>
        public DiscordOptional<string> Name { get; init; }

        /// <summary>
        /// the volume of the soundboard sound, from 0 to 1
        /// </summary>
        public DiscordOptional<double?> Volume { get; init; }

        /// <summary>
        /// the id of the custom emoji for the soundboard sound
        /// </summary>
        public DiscordOptional<DiscordSnowflake?> EmojiId { get; init; }

        /// <summary>
        /// the unicode character of a standard emoji for the soundboard sound
        /// </summary>
        public DiscordOptional<string?> EmojiName { get; init; }
    }
}
