using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.JsonParameterModels
{
    public record DiscordModifyChannelLinkToLobbyJsonParameterModel
    {
        /// <summary>
        /// the id of the channel to link to the lobby. If not provided, will unlink any currently linked channels from the lobby.
        /// </summary>
        public DiscordOptional<DiscordSnowflake> ChannelId { get; set; }
    }
}