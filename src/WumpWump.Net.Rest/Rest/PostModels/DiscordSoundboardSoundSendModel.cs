using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.PostModels
{
    public record DiscordSoundboardSoundSendModel
    {
        public required DiscordSnowflake SoundId { get; init; }
        public DiscordOptional<DiscordSnowflake> SourceGuildId { get; init; }
    }
}
