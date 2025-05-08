using System.Collections.Generic;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Entities
{
    public record DiscordGatewayActivityParty
    {
        /// <summary>
        /// ID of the party
        /// </summary>
        public DiscordOptional<string> Id { get; init; }

        /// <summary>
        /// Used to show the party's current and maximum size
        /// </summary>
        /// <remarks>
        /// array of two integers (current_size, max_size)
        /// </remarks>
        public DiscordOptional<IReadOnlyList<int>> Size { get; init; }
    }
}
