using WumpWump.Net.Entities;

namespace WumpWump.Net.Rest.PostModels
{
    public class DiscordDmCreateModel
    {
        /// <summary>
        /// the recipient to open a DM channel with
        /// </summary>
        public required DiscordSnowflake RecipientId { get; init; }
    }
}
