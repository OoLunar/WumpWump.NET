using System;
using System.Collections.Generic;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Rest.PostModels
{
    /// <summary>
    /// Only properties that are passed will be updated.
    /// </summary>
    public class DiscordApplicationEditModel
    {
        /// <summary>
        /// Default custom authorization URL for the app, if enabled
        /// </summary>
        public DiscordOptional<Uri> CustomInstallUrl { get; set; }

        /// <summary>
        /// Description of the app
        /// </summary>
        public DiscordOptional<string> Description { get; set; }

        /// <summary>
        /// Role connection verification URL for the app
        /// </summary>
        public DiscordOptional<Uri> RoleConnectionsVerificationUrl { get; set; }

        /// <summary>
        /// Settings for the app's default in-app authorization link, if enabled
        /// </summary>
        public DiscordOptional<DiscordApplicationInstallParams> InstallParams { get; set; }

        /// <summary>
        /// Default scopes and permissions for each supported installation context. Value for each key is an <see cref="DiscordApplicationInstallParams"/> object.
        /// </summary>
        public DiscordOptional<IReadOnlyDictionary<DiscordApplicationIntegrationTypes, DiscordIntegrationConfiguration>> IntegrationConfigurations { get; set; }

        /// <summary>
        /// App's public <see cref="DiscordApplicationFlags"/>
        /// </summary>
        /// <remarks>
        /// Only limited intent flags (<see cref="DiscordApplicationFlags.GatewayPresenceLimited"/>, <see cref="DiscordApplicationFlags.GatewayGuildMembersLimited"/>, and <see cref="DiscordApplicationFlags.GatewayMessageContentLimited"/>) can be updated via the API.
        /// </remarks>
        public DiscordOptional<DiscordApplicationFlags> Flags { get; set; }

        /// <summary>
        /// Icon for the app
        /// </summary>
        public DiscordOptional<string?> Icon { get; set; }

        /// <summary>
        /// Default rich presence invite cover image for the app
        /// </summary>
        public DiscordOptional<string?> CoverImage { get; set; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/interactions/receiving-and-responding#receiving-an-interaction">Interactions endpoint URL</a> for the app
        /// </summary>
        /// <remarks>
        /// To update an Interactions endpoint URL via the API, the URL must be valid according to the <a href="https://discord.com/developers/docs/interactions/receiving-and-responding#receiving-an-interaction">Receiving an Interaction</a> documentation.
        /// </remarks>
        public DiscordOptional<Uri> InteractionsEndpointUrl { get; set; }

        /// <summary>
        /// List of tags describing the content and functionality of the app (max of 20 characters per tag). Max of 5 tags.
        /// </summary>
        public DiscordOptional<IReadOnlyList<string>> Tags { get; set; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/events/webhook-events#preparing-for-events">Event webhooks URL</a> for the app to receive webhook events
        /// </summary>
        public DiscordOptional<Uri> EventWebhooksUrl { get; set; }

        /// <summary>
        /// If <a href="https://discord.com/developers/docs/events/webhook-events">webhook events</a> are enabled for the app. <see cref="DiscordApplicationEventWebhookStatus.Disabled"/> to disable, and <see cref="DiscordApplicationEventWebhookStatus.Enabled"/> to enable
        /// </summary>
        public DiscordOptional<DiscordApplicationEventWebhookStatus> EventWebhooksStatus { get; set; }

        /// <summary>
        /// List of <a href="https://discord.com/developers/docs/events/webhook-events#event-types">Webhook event types</a> to subscribe to
        /// </summary>
        public DiscordOptional<IReadOnlyList<string>> EventWebhookTypes { get; set; }
    }
}
