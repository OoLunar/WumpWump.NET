using System.Collections.Generic;

namespace WumpWump.Net.Entities
{
    public record DiscordApplicationRoleConnectionMetadata
    {
        /// <summary>
        /// type of metadata value
        /// </summary>
        public required DiscordApplicationRoleConnectionMetadataType Type { get; init; }

        /// <summary>
        /// dictionary key for the metadata field (must be <c>a-z</c>, <c>0-9</c>, or <c>_</c> characters; 1-50 characters)
        /// </summary>
        public required string Key { get; init; }

        /// <summary>
        /// name of the metadata field (1-100 characters)
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// translations of the name
        /// </summary>
        public required IReadOnlyDictionary<string, string> NameLocalizations { get; init; }

        /// <summary>
        /// description of the metadata field (1-200 characters)
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// translations of the description
        /// </summary>
        public required IReadOnlyDictionary<string, string> DescriptionLocalizations { get; init; }
    }
}
