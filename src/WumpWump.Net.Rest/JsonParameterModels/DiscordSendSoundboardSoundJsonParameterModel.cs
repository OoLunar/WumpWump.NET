using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.JsonParameterModels
{
    public record DiscordSendSoundboardSoundJsonParameterModel
    {
        public required DiscordSnowflake SoundId { get; init; }
        public DiscordOptional<DiscordSnowflake> SourceGuildId { get; init; }
    }
}
