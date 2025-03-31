using System;
using System.Collections.Generic;

namespace WumpWump.Net.Entities
{
    public record DiscordIntegration
    {
        /// <summary>
        /// integration id
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// integration name
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// integration type (twitch, youtube, discord, or guild_subscription)
        /// </summary>
        public required string Type { get; init; }

        /// <summary>
        /// is this integration enabled
        /// </summary>
        public required bool Enabled { get; init; }

        /// <summary>
        /// is this integration syncing
        /// </summary>
        public DiscordOptional<bool> Syncing { get; init; }

        /// <summary>
        /// id that this integration uses for "subscribers"
        /// </summary>
        public DiscordOptional<DiscordSnowflake> RoleId { get; init; }

        /// <summary>
        /// whether emoticons should be synced for this integration (twitch only currently)
        /// </summary>
        public DiscordOptional<bool> EnableEmoticons { get; init; }

        /// <summary>
        /// the behavior of expiring subscribers
        /// </summary>
        public DiscordOptional<DiscordIntegrationExpireBehavior> ExpireBehavior { get; init; }

        /// <summary>
        /// the grace period (in days) before expiring subscribers
        /// </summary>
        public DiscordOptional<int> ExpireGracePeriod { get; init; }

        /// <summary>
        /// user for this integration
        /// </summary>
        /// <remarks>
        /// Some older integrations may not have an attached user.
        /// </remarks>
        public DiscordOptional<DiscordUser> User { get; init; }

        /// <summary>
        /// integration account information
        /// </summary>
        public required DiscordIntegrationAccount Account { get; init; }

        /// <summary>
        /// when this integration was last synced
        /// </summary>
        public DiscordOptional<DateTimeOffset> SyncedAt { get; init; }

        /// <summary>
        /// how many subscribers this integration has
        /// </summary>
        public DiscordOptional<int> SubscriberCount { get; init; }

        /// <summary>
        /// has this integration been revoked
        /// </summary>
        public DiscordOptional<bool> Revoked { get; init; }

        /// <summary>
        /// The bot/OAuth2 application for discord integrations
        /// </summary>
        public DiscordOptional<DiscordIntegrationApplication> Application { get; init; }

        /// <summary>
        /// the scopes the application has been authorized for
        /// </summary>
        public DiscordOptional<IReadOnlyList<string>> Scopes { get; init; }
    }
}
