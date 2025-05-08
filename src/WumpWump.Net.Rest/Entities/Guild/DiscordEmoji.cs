using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    public record DiscordEmoji
    {
        /// <summary>
        ///	emoji id
        /// </summary>
        public required DiscordSnowflake? Id { get; init; }

        /// <summary>
        /// emoji name
        /// </summary>
        public required string? Name { get; init; }

        /// <summary>
        /// roles allowed to use this emoji
        /// </summary>
        public DiscordOptional<IReadOnlyList<DiscordSnowflake>> Roles { get; init; }

        /// <summary>
        /// user that created this emoji
        /// </summary>
        public DiscordOptional<DiscordUser> User { get; init; }

        /// <summary>
        /// whether this emoji must be wrapped in colons
        /// </summary>
        public DiscordOptional<bool> RequireColons { get; init; }

        /// <summary>
        /// whether this emoji is managed
        /// </summary>
        public DiscordOptional<bool> Managed { get; init; }

        /// <summary>
        /// whether this emoji is animated
        /// </summary>
        public DiscordOptional<bool> Animated { get; init; }

        /// <summary>
        /// whether this emoji can be used, may be false due to loss of Server Boosts
        /// </summary>
        public DiscordOptional<bool> Available { get; init; }
    }
}
