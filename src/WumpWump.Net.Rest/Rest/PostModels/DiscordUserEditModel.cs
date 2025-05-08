using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.PostModels
{
    public class DiscordUserEditModel
    {
        /// <summary>
        /// user's username, if changed may cause the user's discriminator to be randomized.
        /// </summary>
        public DiscordOptional<string> Username { get; set; }

        /// <summary>
        /// if passed, modifies the user's avatar
        /// </summary>
        public DiscordOptional<string> Avatar { get; set; }

        /// <summary>
        /// if passed, modifies the user's banner
        /// </summary>
        public DiscordOptional<string> Banner { get; set; }
    }
}
