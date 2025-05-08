using System;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Flags for a <see cref="DiscordApplication"/>
    /// </summary>
    [Flags]
    public enum DiscordApplicationFlags
    {
        /// <summary>
        /// Default value for <see cref="DiscordApplicationFlags"/>
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates if an app uses the <a href="https://discord.com/developers/docs/resources/auto-moderation">Auto Moderation API</a>
        /// </summary>
        ApplicationAutoModerationRuleCreateBadge = 1 << 6,

        /// <summary>
        /// Intent required for bots in <b>100 or more servers</b> to receive <a href="https://discord.com/developers/docs/events/gateway-events#presence-update"><c>presence_update</c> events</a>
        /// </summary>
        GatewayPresence = 1 << 12,

        /// <summary>
        /// Intent required for bots in under 100 servers to receive <a href="https://discord.com/developers/docs/events/gateway-events#presence-update"><c>presence_update</c> events</a>, found on the <b>Bot</b> page in your app's settings
        /// </summary>
        GatewayPresenceLimited = 1 << 13,

        /// <summary>
        /// Intent required for bots in <b>100 or more servers</b> to receive member-related events like <c>guild_member_add</c>. See the list of member-related events <a href="https://discord.com/developers/docs/events/gateway#list-of-intents">under <c>GUILD_MEMBERS</c></a>
        /// </summary>
        GatewayGuildMembers = 1 << 14,

        /// <summary>
        /// Intent required for bots in under 100 servers to receive member-related events like <c>guild_member_add</c>, found on the <b>Bot</b> page in your app's settings. See the list of member-related events <a href="https://discord.com/developers/docs/events/gateway#list-of-intents">under <c>GUILD_MEMBERS</c></a>
        /// </summary>
        GatewayGuildMembersLimited = 1 << 15,

        /// <summary>
        /// Indicates unusual growth of an app that prevents verification
        /// </summary>
        VerificationPendingGuildLimit = 1 << 16,

        /// <summary>
        /// Indicates if an app is embedded within the Discord client (currently unavailable publicly)
        /// </summary>
        Embedded = 1 << 17,

        /// <summary>
        /// Intent required for bots in <b>100 or more servers</b> to receive <a href="https://support-dev.discord.com/hc/en-us/articles/4404772028055-Message-Content-Privileged-Intent-FAQ">message content</a>
        /// </summary>
        GatewayMessageContent = 1 << 18,

        /// <summary>
        /// Intent required for bots in under 100 servers to receive <a href="https://support-dev.discord.com/hc/en-us/articles/4404772028055-Message-Content-Privileged-Intent-FAQ">message content</a>, found on the <b>Bot</b> page in your app's settings
        /// </summary>
        GatewayMessageContentLimited = 1 << 19,

        /// <summary>
        /// Indicates if an app has registered global <a href="https://discord.com/developers/docs/interactions/application-commands">application commands</a>
        /// </summary>
        ApplicationCommandBadge = 1 << 23

    }
}
