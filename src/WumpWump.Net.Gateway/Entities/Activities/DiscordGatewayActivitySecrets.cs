using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Entities
{
    public record DiscordGatewayActivitySecrets
    {
        /// <summary>
        /// Secret for joining a party
        /// </summary>
        public DiscordOptional<string> Join { get; init; }

        /// <summary>
        /// Secret for spectating a game
        /// </summary>
        public DiscordOptional<string> Spectate { get; init; }

        /// <summary>
        /// Secret for a specific instanced match
        /// </summary>
        public DiscordOptional<string> Match { get; init; }
    }
}
