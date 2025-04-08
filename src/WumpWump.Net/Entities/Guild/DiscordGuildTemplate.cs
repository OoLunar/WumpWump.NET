using System;

namespace WumpWump.Net.Entities
{
    /// <summary>
    /// Represents a code that when used, creates a guild based on a snapshot of an existing guild.
    /// </summary>
    public record DiscordGuildTemplate
    {
        /// <summary>
        /// the template code (unique ID)
        /// </summary>
        public required string Code { get; init; }

        /// <summary>
        /// template name
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// the description for the template
        /// </summary>
        public required string? Description { get; init; }

        /// <summary>
        /// number of times this template has been used
        /// </summary>
        public required int UsageCount { get; init; }

        /// <summary>
        /// the ID of the user who created the template
        /// </summary>
        public required DiscordSnowflake CreatorId { get; init; }

        /// <summary>
        /// the user who created the template
        /// </summary>
        public required DiscordUser Creator { get; init; }

        /// <summary>
        /// when this template was created
        /// </summary>
        public required DateTimeOffset CreatedAt { get; init; }

        /// <summary>
        /// when this template was last synced to the source guild
        /// </summary>
        public required DateTimeOffset UpdatedAt { get; init; }

        /// <summary>
        /// the ID of the guild this template is based on
        /// </summary>
        public required DiscordSnowflake SourceGuildId { get; init; }

        /// <summary>
        /// the guild snapshot this template contains; placeholder IDs are given as integers
        /// </summary>
        public required DiscordGuild SerializedSourceGuild { get; init; }

        /// <summary>
        /// whether the template has unsynced changes
        /// </summary>
        public required bool? IsDirty { get; init; }

    }
}