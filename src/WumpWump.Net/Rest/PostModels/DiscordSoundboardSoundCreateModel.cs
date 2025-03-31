using WumpWump.Net.Entities;

namespace WumpWump.Net.Rest.PostModels
{
    public record DiscordSoundboardSoundCreateModel
    {
        /// <summary>
        /// name of the soundboard sound (2-32 characters)
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// the mp3 or ogg sound data, base64 encoded, similar to <a href="https://discord.com/developers/docs/reference#image-data">image data</a>
        /// </summary>
        public required DiscordUpload Sound { get; init; }

        /// <summary>
        /// the volume of the soundboard sound, from 0 to 1, defaults to 1
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
