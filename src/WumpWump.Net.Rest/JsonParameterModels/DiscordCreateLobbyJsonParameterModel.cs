using System.Collections.Generic;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.JsonParameterModels
{
    public record DiscordCreateLobbyJsonParameterModel
    {
        /// <summary>
        /// optional dictionary of string key/value pairs. The max total length is 1000.
        /// </summary>
        public DiscordOptional<IDictionary<string, string>?> Metadata { get; init; }

        /// <summary>
        /// optional array of up to 25 users to be added to the lobby
        /// </summary>
        public DiscordOptional<IList<DiscordLobbyMember>> Members { get; init; }

        /// <summary>
        /// seconds to wait before shutting down a lobby after it becomes idle. Value can be between 5 and 604800 (7 days). See LobbyHandle for more details on this behavior.
        /// </summary>
        public DiscordOptional<int> IdleTimeoutSeconds { get; init; }
    }
}