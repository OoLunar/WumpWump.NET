namespace WumpWump.Net.Gateway.Entities
{
    public enum DiscordGatewayIntents
    {
        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.GuildCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildDeleteEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildRoleCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildRoleUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildRoleDeleteEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ChannelCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ChannelUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ChannelDeleteEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ChannelPinsUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ThreadCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ThreadUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ThreadDeleteEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ThreaditemstSyncEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ThreadMemberUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ThreadMembersUpdateEvent"/>*</item>
        /// <item><see cref="DiscordGatewayClient.StageInstanceCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.StageInstanceUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.StageInstanceDeleteEvent"/></item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// <see cref="DiscordGatewayClient.ThreadMembersUpdateEvent"/> contains different data depending on which intents are used.
        /// </remarks>
        Guilds = 1 << 0,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.GuildMemberAddEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildMemberUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildMemberRemoveEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.ThreadMembersUpdateEvent"/></item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// <see cref="DiscordGatewayClient.ThreadMembersUpdateEvent"/> contains different data depending on which intents are used.
        /// <br/>
        /// Events under the <see cref="GuildPresences"/> and <see cref="GuildMembers"/> intents are turned off by default on all API versions.
        /// If you are using API v6, you will receive those events if you are authorized to receive them and have enabled the intents in the Developer Portal.
        /// You do not need to use intents on API v6 to receive these events; you just need to enable the flags. If you are using API v8 or above, intents are
        /// mandatory and must be specified when identifying.
        /// </remarks>
        GuildMembers = 1 << 1,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.GuildAuditLogEntryCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildBanAddEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildBanRemoveEvent"/></item>
        /// </list>
        /// </summary>
        GuildModeration = 1 << 2,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.GuildEmojisUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildStickersUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildSoundboardSoundCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildSoundboardSoundUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildSoundboardSoundDeleteEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildSoundboardSoundsUpdateEvent"/></item>
        /// </list>
        /// </summary>
        GuildExpressions = 1 << 3,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.GuildIntegrationsUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.IntegrationCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.IntegrationUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.IntegrationDeleteEvent"/></item>
        /// </list>
        /// </summary>
        GuildIntegrations = 1 << 4,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.WebhooksUpdateEvent"/></item>
        /// </list>
        /// </summary>
        GuildWebhooks = 1 << 5,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.InviteCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.InviteDeleteEvent"/></item>
        /// </list>
        /// </summary>
        GuildInvites = 1 << 6,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.VoiceChannelEffectSendEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.VoiceStateUpdateEvent"/></item>
        /// </list>
        /// </summary>
        GuildVoiceStates = 1 << 7,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.PresenceUpdateEvent"/></item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// Events under the <see cref="GuildPresences"/> and <see cref="GuildMembers"/> intents are turned off by default on all API versions.
        /// If you are using API v6, you will receive those events if you are authorized to receive them and have enabled the intents in the Developer Portal.
        /// You do not need to use intents on API v6 to receive these events; you just need to enable the flags. If you are using API v8 or above, intents are
        /// mandatory and must be specified when identifying.
        /// </remarks>
        GuildPresences = 1 << 8,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.MessageCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageDeleteEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageDeleteBulkEvent"/></item>
        /// </list>
        /// </summary>
        GuildMessages = 1 << 9,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.MessageReactionAddEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageReactionRemoveEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageReactionRemoveAllEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageReactionRemoveEmojiEvent"/></item>
        /// </list>
        /// </summary>
        GuildMessageReactions = 1 << 10,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.TypingStartEvent"/></item>
        /// </list>
        /// </summary>
        GuildMessageTyping = 1 << 11,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.MessageCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageDeleteEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageDeleteBulkEvent"/></item>
        /// </list>
        /// </summary>
        DirectMessages = 1 << 12,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.MessageReactionAddEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageReactionRemoveEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageReactionRemoveAllEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessageReactionRemoveEmojiEvent"/></item>
        /// </list>
        /// </summary>
        DirectMessageReactions = 1 << 13,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.TypingStartEvent"/></item>
        /// </list>
        /// </summary>
        DirectMessageTyping = 1 << 14,

        /// <summary>
        /// <see cref="MessageContent"/> does not represent individual events, but rather affects what data is present for events that could contain message content fields.
        /// More information is in the <a href="https://discord.com/developers/docs/events/gateway#message-content-intent">message content intent</a> section.
        /// </summary>
        MessageContent = 1 << 15,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.GuildScheduledEventCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildScheduledEventUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildScheduledEventDeleteEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildScheduledEventUserAddEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.GuildScheduledEventUserRemoveEvent"/></item>
        /// </list>
        /// </summary>
        GuildScheduledEvents = 1 << 16,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.AutoModerationRuleCreateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.AutoModerationRuleUpdateEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.AutoModerationRuleDeleteEvent"/></item>
        /// </list>
        /// </summary>
        AutoModerationConfiguration = 1 << 20,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.AutoModerationActionExecutionEvent"/></item>
        /// </list>
        /// </summary>
        AutoModerationExecution = 1 << 21,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.MessagePollVoteAddEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessagePollVoteRemoveEvent"/></item>
        /// </list>
        /// </summary>
        GuildMessagePolls = 1 << 24,

        /// <summary>
        /// Contains the following events:
        /// <list type="bullet">
        /// <item><see cref="DiscordGatewayClient.MessagePollVoteAddEvent"/></item>
        /// <item><see cref="DiscordGatewayClient.MessagePollVoteRemoveEvent"/></item>
        /// </list>
        /// </summary>
        DirectMessagePolls = 1 << 25,

        All = Guilds | GuildMembers | GuildModeration | GuildExpressions | GuildIntegrations | GuildWebhooks | GuildInvites | GuildVoiceStates | GuildPresences | GuildMessages | GuildMessageReactions | GuildMessageTyping | DirectMessages | DirectMessageReactions | DirectMessageTyping | MessageContent | GuildScheduledEvents | AutoModerationConfiguration | AutoModerationExecution | GuildMessagePolls | DirectMessagePolls
    }
}
