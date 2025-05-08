using System.Collections.Generic;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest.PostModels
{
    /// <summary>
    /// Represents a model to create a group DM with a user.
    /// </summary>
    public record DiscordGroupDmCreateModel
    {
        /// <summary>
        /// access tokens of users that have granted your app the <c>gdm.join</c> scope
        /// </summary>
        public required IEnumerable<string> AccessTokens { get; init; }

        /// <summary>
        /// a dictionary of user ids to their respective nicknames
        /// </summary>
        public required IReadOnlyDictionary<DiscordSnowflake, string?> Nicks { get; init; }
    }
}
