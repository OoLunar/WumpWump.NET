using System;
using System.Collections.Generic;

namespace WumpWump.Net.Entities
{
    /// <summary>
    /// <a href="https://discord.com/developers/docs/quick-start/overview-of-apps">Applications</a> (or "apps") are containers for developer platform features, and can be installed to Discord servers and/or user accounts.
    /// </summary>
    public record DiscordApplication
    {
        /// <summary>
        /// ID of the app
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// Name of the app
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/reference#image-formatting"/>Icon hash</a> of the app
        /// </summary>
        public required string? Icon { get; init; }

        /// <summary>
        /// Description of the app
        /// </summary>
        public required string Description { get; init; }

        /// <summary>
        /// List of RPC origin URLs, if RPC is enabled
        /// </summary>
        public DiscordOptional<IReadOnlyList<Uri>> RpcOrigins { get; init; }

        /// <summary>
        /// When <see langword="false"/>, only the app owner can add the app to guilds
        /// </summary>
        public required bool BotPublic { get; init; }

        /// <summary>
        /// When <see langword="true"/>, the app's bot will only join upon completion of the full OAuth2 code grant flow
        /// </summary>
        public required bool BotRequireCodeGrant { get; init; }

        /// <summary>
        /// Partial user object for the bot user associated with the app
        /// </summary>
        public DiscordOptional<DiscordUser> Bot { get; init; }

        /// <summary>
        /// URL of the app's Terms of Service
        /// </summary>
        public DiscordOptional<Uri> TermsOfServiceUrl { get; init; }

        /// <summary>
        /// URL of the app's Privacy Policy
        /// </summary>
        public DiscordOptional<Uri> PrivacyPolicyUrl { get; init; }

        /// <summary>
        /// Partial user object for the owner of the app
        /// </summary>
        public DiscordOptional<DiscordUser> Owner { get; init; }

        /// <summary>
        /// Hex encoded key for verification in interactions and the <a href="https://github.com/discord/discord-api-docs/blob/legacy-gamesdk/docs/game_sdk/Applications.md#getticket">GameSDK's GetTicket</a>
        /// </summary>
        public required string VerifyKey { get; init; }

        /// <summary>
        /// If the app belongs to a team, this will be a list of the members of that team
        /// </summary>
        public required DiscordTeam? Team { get; init; }

        /// <summary>
        /// Guild associated with the app. For example, a developer support server.
        /// </summary>
        public DiscordOptional<DiscordSnowflake> GuildId { get; init; }

        /// <summary>
        /// Partial object of the associated guild
        /// </summary>
        public DiscordOptional<DiscordGuild> Guild { get; init; }

        /// <summary>
        /// If this app is a game sold on Discord, this field will be the id of the "Game SKU" that is created, if exists
        /// </summary>
        public DiscordOptional<DiscordSnowflake> PrimarySkuId { get; init; }

        /// <summary>
        /// If this app is a game sold on Discord, this field will be the URL slug that links to the store page
        /// </summary>
        public DiscordOptional<Uri> Slug { get; init; }

        /// <summary>
        /// App's default rich presence invite <a href="https://discord.com/developers/docs/reference#image-formatting">cover image hash</a>
        /// </summary>
        public DiscordOptional<string> CoverImage { get; init; }

        /// <summary>
        /// App's public <see cref="DiscordApplicationFlags"/>
        /// </summary>
        public DiscordOptional<DiscordApplicationFlags> Flags { get; init; }

        /// <summary>
        /// Approximate count of guilds the app has been added to
        /// </summary>
        public DiscordOptional<int> ApproximateGuildCount { get; init; }

        /// <summary>
        /// Approximate count of users that have installed the app
        /// </summary>
        public DiscordOptional<int> ApproximateUserInstallCount { get; init; }

        /// <summary>
        /// Array of redirect URIs for the app
        /// </summary>
        public DiscordOptional<IReadOnlyList<Uri>> RedirectUris { get; init; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/interactions/receiving-and-responding#receiving-an-interaction">Interactions endpoint URL</a> for the app
        /// </summary>
        public DiscordOptional<Uri?> InteractionsEndpointUri { get; init; }

        /// <summary>
        /// Role connection verification URL for the app
        /// </summary>
        public DiscordOptional<Uri?> RoleConnectionsVerificationUrl { get; init; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/events/webhook-events#preparing-for-events">Event webhooks URL</a> for the app to receive webhook events
        /// </summary>
        public DiscordOptional<Uri?> EventWebhooksUrl { get; init; }

        /// <summary>
        /// If <a href="https://discord.com/developers/docs/events/webhook-events">webhook events</a> are enabled for the app. <see cref="DiscordApplicationEventWebhookStatus.Disabled"/> (default) means disabled, <see cref="DiscordApplicationEventWebhookStatus.Enabled"/> means enabled, and <see cref="DiscordApplicationEventWebhookStatus.DisabledByDiscord"/> means disabled by Discord
        /// </summary>
        public DiscordOptional<DiscordApplicationEventWebhookStatus?> EventWebhooksStatus { get; init; }

        /// <summary>
        /// List of <a href="https://discord.com/developers/docs/events/webhook-events#event-types"/>Webhook event types</a> the app subscribes to
        /// </summary>
        public DiscordOptional<IReadOnlyList<string?>> EventWebhooksTypes { get; init; }

        /// <summary>
        /// List of tags describing the content and functionality of the app. Max of 5 tags.
        /// </summary>
        public DiscordOptional<IReadOnlyList<string?>> Tags { get; init; }

        /// <summary>
        /// Settings for the app's default in-app authorization link, if enabled
        /// </summary>
        public DiscordOptional<DiscordApplicationInstallParams> InstallParams { get; init; }

        /// <summary>
        /// Default scopes and permissions for each supported installation context. Value for each key is an <see cref="DiscordIntegrationConfiguration"/>
        /// </summary>
        public DiscordOptional<IReadOnlyDictionary<DiscordApplicationIntegrationTypes, DiscordIntegrationConfiguration>> IntegrationTypesConfig { get; init; }

        /// <summary>
        /// Default custom authorization URL for the app, if enabled
        /// </summary>
        public DiscordOptional<Uri> CustomInstallUrl { get; init; }
    }
}
