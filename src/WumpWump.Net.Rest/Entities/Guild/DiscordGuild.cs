using System;
using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Guilds in Discord represent an isolated collection of users and channels, and are often referred to as "servers" in the UI.
    /// </summary>
    public record DiscordGuild : DiscordUnavailableGuild
    {
        /// <summary>
        /// guild name (2-100 characters, excluding trailing and leading whitespace)
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/reference#image-formatting">icon hash</a>
        /// </summary>
        public required string? Icon { get; init; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/reference#image-formatting">icon hash</a>, returned when in the template object.
        /// </summary>
        public DiscordOptional<string?> IconHash { get; init; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/reference#image-formatting">splash hash</a>
        /// </summary>
        public required string? Splash { get; init; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/reference#image-formatting">discovery splash hash</a>; only present for guilds with the "DISCOVERABLE" feature
        /// </summary>
        public required string? DiscoverySplash { get; init; }

        /// <summary>
        /// true if the user is the owner of the guild
        /// </summary>
        public DiscordOptional<bool> Owner { get; init; }

        /// <summary>
        /// id of owner
        /// </summary>
        public required DiscordSnowflake OwnerId { get; init; }

        /// <summary>
        /// total permissions for the user in the guild (excludes overwrites and <a href="https://discord.com/developers/docs/topics/permissions#implicit-permissions">implicit permissions</a>)
        /// </summary>
        public DiscordOptional<DiscordPermissionContainer> Permissions { get; init; }

        /// <summary>
        /// voice region id for the guild (deprecated)
        /// </summary>
        [Obsolete("This field is deprecated by Discord and will be removed in a future version.")]
        public DiscordOptional<string?> Region { get; init; }

        /// <summary>
        /// id of afk channel
        /// </summary>
        public required DiscordSnowflake? AfkChannelId { get; init; }

        /// <summary>
        /// afk timeout in seconds
        /// </summary>
        public required int AfkTimeout { get; init; }

        /// <summary>
        /// true if the server widget is enabled
        /// </summary>
        public DiscordOptional<bool> WidgetEnabled { get; init; }

        /// <summary>
        /// the channel id that the widget will generate an invite to, or null if set to no invite
        /// </summary>
        public DiscordOptional<DiscordSnowflake?> WidgetChannelId { get; init; }

        /// <summary>
        /// verification level required for the guild
        /// </summary>
        public required DiscordVerificationLevel VerificationLevel { get; init; }

        /// <summary>
        /// default message notifications level
        /// </summary>
        public required DiscordDefaultMessageNotificationLevel DefaultMessageNotifications { get; init; }

        /// <summary>
        /// explicit content filter level
        /// </summary>
        public required DiscordExplicitContentFilterLevel ExplicitContentFilter { get; init; }

        /// <summary>
        /// roles in the guild
        /// </summary>
        public required IReadOnlyList<DiscordRole> Roles { get; init; }

        /// <summary>
        /// custom guild emojis
        /// </summary>
        public required IReadOnlyList<DiscordEmoji> Emojis { get; init; }

        /// <summary>
        /// enabled guild features
        /// </summary>
        public required IReadOnlyList<string> Features { get; init; }

        /// <summary>
        /// required MFA level for the guild
        /// </summary>
        public required DiscordMfaLevel MfaLevel { get; init; }

        /// <summary>
        /// application id of the guild creator if it is bot-created
        /// </summary>
        public required DiscordSnowflake? ApplicationId { get; init; }

        /// <summary>
        /// the id of the channel where guild notices such as welcome messages and boost events are posted
        /// </summary>
        public required DiscordSnowflake? SystemChannelId { get; init; }

        /// <summary>
        /// system channel flags
        /// </summary>
        public required DiscordSystemChannelFlags SystemChannelFlags { get; init; }

        /// <summary>
        /// the id of the channel where Community guilds can display rules and/or guidelines
        /// </summary>
        public required DiscordSnowflake? RulesChannelId { get; init; }

        /// <summary>
        /// the maximum number of presences for the guild (null is always returned, apart from the largest of guilds)
        /// </summary>
        public DiscordOptional<int?> MaxPresences { get; init; }

        /// <summary>
        /// the maximum number of members for the guild
        /// </summary>
        public DiscordOptional<int?> MaxMembers { get; init; }

        /// <summary>
        /// the vanity url code for the guild
        /// </summary>
        public required string? VanityUrlCode { get; init; }

        /// <summary>
        /// the description of a guild
        /// </summary>
        public required string? Description { get; init; }

        /// <summary>
        /// banner hash
        /// </summary>
        public required string? Banner { get; init; }

        /// <summary>
        /// premium tier (Server Boost level)
        /// </summary>
        public required DiscordPremiumTier PremiumTier { get; init; }

        /// <summary>
        /// the number of boosts this guild currently has
        /// </summary>
        public DiscordOptional<int> PremiumSubscriptionCount { get; init; }

        /// <summary>
        /// the preferred locale of a Community guild; used in server discovery and notices from Discord, and sent in interactions; defaults to "en-US"
        /// </summary>
        public required string PreferredLocale { get; init; }

        /// <summary>
        /// the id of the channel where admins and moderators of Community guilds receive notices from Discord
        /// </summary>
        public required DiscordSnowflake? PublicUpdatesChannelId { get; init; }

        /// <summary>
        /// the maximum amount of users in a video channel
        /// </summary>
        public DiscordOptional<int> MaxVideoChannelUsers { get; init; }

        /// <summary>
        /// the maximum amount of users in a stage video channel
        /// </summary>
        public DiscordOptional<int> MaxStageVideoChannelUsers { get; init; }

        /// <summary>
        /// approximate number of members in this guild, returned from the GET /guilds/{id} and /users/@me/guilds endpoints when with_counts is true
        /// </summary>
        public DiscordOptional<int> ApproximateMemberCount { get; init; }

        /// <summary>
        /// approximate number of non-offline members in this guild, returned from the GET /guilds/{id} and /users/@me/guilds endpoints when with_counts is true
        /// </summary>
        public DiscordOptional<int> ApproximatePresenceCount { get; init; }

        /// <summary>
        /// the welcome screen of a Community guild, shown to new members, returned in an Invite's guild object
        /// </summary>
        public DiscordOptional<DiscordWelcomeScreen> WelcomeScreen { get; init; }

        /// <summary>
        /// guild NSFW level
        /// </summary>
        public required DiscordGuildNsfwLevel NsfwLevel { get; init; }

        /// <summary>
        /// custom guild stickers
        /// </summary>
        public DiscordOptional<IReadOnlyList<DiscordSticker>> Stickers { get; init; }

        /// <summary>
        /// whether the guild has the boost progress bar enabled
        /// </summary>
        public required bool PremiumProgressBarEnabled { get; init; }

        /// <summary>
        /// the id of the channel where admins and moderators of Community guilds receive safety alerts from Discord
        /// </summary>
        public required DiscordSnowflake? SafetyAlertsChannelId { get; init; }

        /// <summary>
        /// the incidents data for this guild
        /// </summary>
        public required DiscordIncidentsData? IncidentsData { get; init; }
    }
}
