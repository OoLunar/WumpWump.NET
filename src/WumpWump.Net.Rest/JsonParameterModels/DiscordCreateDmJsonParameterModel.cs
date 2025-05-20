using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.JsonParameterModels
{
    public class DiscordCreateDmJsonParameterModel
    {
        /// <summary>
        /// the recipient to open a DM channel with
        /// </summary>
        public required DiscordSnowflake RecipientId { get; init; }
    }
}
