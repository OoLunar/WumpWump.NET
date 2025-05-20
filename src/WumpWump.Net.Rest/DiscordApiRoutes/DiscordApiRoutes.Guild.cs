using System.Net.Http;
using System.Text;

namespace WumpWump.Net.Rest
{
    public static partial class DiscordApiRoutes
    {
        // Guild
        public static readonly DiscordApiEndpointKey CreateGuild = new(HttpMethod.Post, CompositeFormat.Parse("/guilds"), CompositeFormat.Parse("/guilds"));
        public static readonly DiscordApiEndpointKey GetGuild = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}"), CompositeFormat.Parse("/guilds/{0}"));
        public static readonly DiscordApiEndpointKey GetGuildPreview = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/preview"), CompositeFormat.Parse("/guilds/{0}"));
        public static readonly DiscordApiEndpointKey ModifyGuild = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}"), CompositeFormat.Parse("/guilds/{0}"));
        public static readonly DiscordApiEndpointKey DeleteGuild = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}"), CompositeFormat.Parse("/guilds/{0}"));
        public static readonly DiscordApiEndpointKey GetGuildChannels = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/channels"), CompositeFormat.Parse("/guilds/{0}/channels"));
        public static readonly DiscordApiEndpointKey CreateGuildChannel = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/{0}/channels"), CompositeFormat.Parse("/guilds/{0}/channels"));
        public static readonly DiscordApiEndpointKey ModifyGuildChannelPositions = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/channels"), CompositeFormat.Parse("/guilds/{0}/channels"));
        public static readonly DiscordApiEndpointKey ListActiveGuildThreads = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/threads/active"), CompositeFormat.Parse("/guilds/{0}/threads/active"));
        public static readonly DiscordApiEndpointKey GetGuildMember = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/members/{1}"), CompositeFormat.Parse("/guilds/{0}/members/{1}"));
        public static readonly DiscordApiEndpointKey ListGuildMembers = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/members"), CompositeFormat.Parse("/guilds/{0}/members"));
        public static readonly DiscordApiEndpointKey SearchGuildMembers = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/members/search"), CompositeFormat.Parse("/guilds/{0}/members/search"));
        public static readonly DiscordApiEndpointKey AddGuildMember = new(HttpMethod.Put, CompositeFormat.Parse("/guilds/{0}/members/{1}"), CompositeFormat.Parse("/guilds/{0}/members/{1}"));
        public static readonly DiscordApiEndpointKey ModifyGuildMember = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/members/{1}"), CompositeFormat.Parse("/guilds/{0}/members/{1}"));
        public static readonly DiscordApiEndpointKey ModifyCurrentGuildMember = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/members/@me"), CompositeFormat.Parse("/guilds/{0}/members/@me"));
        public static readonly DiscordApiEndpointKey ModifyCurrentUserNick = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/members/@me/nick"), CompositeFormat.Parse("/guilds/{0}/members/@me/nick"));
        public static readonly DiscordApiEndpointKey AddGuildMemberRole = new(HttpMethod.Put, CompositeFormat.Parse("/guilds/{0}/members/{1}/roles/{2}"), CompositeFormat.Parse("/guilds/{0}/members/{1}/roles/{2}"));
        public static readonly DiscordApiEndpointKey RemoveGuildMemberRole = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/members/{1}/roles/{2}"), CompositeFormat.Parse("/guilds/{0}/members/{1}/roles/{2}"));
        public static readonly DiscordApiEndpointKey RemoveGuildMember = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/members/{1}"), CompositeFormat.Parse("/guilds/{0}/members/{1}"));
        public static readonly DiscordApiEndpointKey GetGuildBans = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/bans"), CompositeFormat.Parse("/guilds/{0}/bans"));
        public static readonly DiscordApiEndpointKey GetGuildBan = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/bans/{1}"), CompositeFormat.Parse("/guilds/{0}/bans/{1}"));
        public static readonly DiscordApiEndpointKey CreateGuildBan = new(HttpMethod.Put, CompositeFormat.Parse("/guilds/{0}/bans/{1}"), CompositeFormat.Parse("/guilds/{0}/bans/{1}"));
        public static readonly DiscordApiEndpointKey RemoveGuildBan = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/bans/{1}"), CompositeFormat.Parse("/guilds/{0}/bans/{1}"));
        public static readonly DiscordApiEndpointKey BulkGuildBan = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/{0}/bulk-ban"), CompositeFormat.Parse("/guilds/{0}/bulk-ban"));
        public static readonly DiscordApiEndpointKey GetGuildRoles = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/roles"), CompositeFormat.Parse("/guilds/{0}/roles"));
        public static readonly DiscordApiEndpointKey GetGuildRole = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/roles/{1}"), CompositeFormat.Parse("/guilds/{0}/roles/{1}"));
        public static readonly DiscordApiEndpointKey CreateGuildRole = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/{0}/roles"), CompositeFormat.Parse("/guilds/{0}/roles"));
        public static readonly DiscordApiEndpointKey ModifyGuildRolePositions = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/roles"), CompositeFormat.Parse("/guilds/{0}/roles"));
        public static readonly DiscordApiEndpointKey ModifyGuildRole = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/roles/{1}"), CompositeFormat.Parse("/guilds/{0}/roles/{1}"));
        public static readonly DiscordApiEndpointKey ModifyGuildMfaLevel = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/mfa"), CompositeFormat.Parse("/guilds/{0}/mfa"));
        public static readonly DiscordApiEndpointKey DeleteGuildRole = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/roles/{1}"), CompositeFormat.Parse("/guilds/{0}/roles/{1}"));
        public static readonly DiscordApiEndpointKey GetGuildPruneCount = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/prune"), CompositeFormat.Parse("/guilds/{0}/prune"));
        public static readonly DiscordApiEndpointKey BeginGuildPrune = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/{0}/prune"), CompositeFormat.Parse("/guilds/{0}/prune"));
        public static readonly DiscordApiEndpointKey GetGuildVoiceRegions = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/regions"), CompositeFormat.Parse("/guilds/{0}/regions"));
        public static readonly DiscordApiEndpointKey GetGuildInvites = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/invites"), CompositeFormat.Parse("/guilds/{0}/invites"));
        public static readonly DiscordApiEndpointKey GetGuildIntegrations = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/integrations"), CompositeFormat.Parse("/guilds/{0}/integrations"));
        public static readonly DiscordApiEndpointKey DeleteGuildIntegration = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/integrations/{1}"), CompositeFormat.Parse("/guilds/{0}/integrations/{1}"));
        public static readonly DiscordApiEndpointKey GetGuildWidgetSettings = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/widget"), CompositeFormat.Parse("/guilds/{0}/widget"));
        public static readonly DiscordApiEndpointKey ModifyGuildWidget = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/widget"), CompositeFormat.Parse("/guilds/{0}/widget"));
        public static readonly DiscordApiEndpointKey GetGuildWidget = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/widget.json"), CompositeFormat.Parse("/guilds/{0}/widget.json"));
        public static readonly DiscordApiEndpointKey GetGuildVanityUrl = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/vanity-url"), CompositeFormat.Parse("/guilds/{0}/vanity-url"));
        public static readonly DiscordApiEndpointKey GetGuildWidgetImage = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/widget.png"), CompositeFormat.Parse("/guilds/{0}/widget.png"));
        public static readonly DiscordApiEndpointKey GetGuildWelcomeScreen = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/welcome-screen"), CompositeFormat.Parse("/guilds/{0}/welcome-screen"));
        public static readonly DiscordApiEndpointKey ModifyGuildWelcomeScreen = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/welcome-screen"), CompositeFormat.Parse("/guilds/{0}/welcome-screen"));
        public static readonly DiscordApiEndpointKey GetGuildOnboarding = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/onboarding"), CompositeFormat.Parse("/guilds/{0}/onboarding"));
        public static readonly DiscordApiEndpointKey ModifyGuildOnboarding = new(HttpMethod.Put, CompositeFormat.Parse("/guilds/{0}/onboarding"), CompositeFormat.Parse("/guilds/{0}/onboarding"));
        public static readonly DiscordApiEndpointKey ModifyGuildIncidentActions = new(HttpMethod.Put, CompositeFormat.Parse("/guilds/{0}/incident-actions"), CompositeFormat.Parse("/guilds/{0}/incident-actions"));

        // Guild Scheduled Event
        public static readonly DiscordApiEndpointKey ListScheduledEvents = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/scheduled-events"), CompositeFormat.Parse("/guilds/{0}/scheduled-events"));
        public static readonly DiscordApiEndpointKey CreateScheduledEvent = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/{0}/scheduled-events"), CompositeFormat.Parse("/guilds/{0}/scheduled-events"));
        public static readonly DiscordApiEndpointKey GetScheduledEvent = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/scheduled-events/{1}"), CompositeFormat.Parse("/guilds/{0}/scheduled-events/{1}"));
        public static readonly DiscordApiEndpointKey ModifyScheduledEvent = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/scheduled-events/{1}"), CompositeFormat.Parse("/guilds/{0}/scheduled-events/{1}"));
        public static readonly DiscordApiEndpointKey DeleteScheduledEvent = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/scheduled-events/{1}"), CompositeFormat.Parse("/guilds/{0}/scheduled-events/{1}"));
        public static readonly DiscordApiEndpointKey GetScheduledEventUsers = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/scheduled-events/{1}/users"), CompositeFormat.Parse("/guilds/{0}/scheduled-events/{1}/users"));

        // Guild Template
        public static readonly DiscordApiEndpointKey GetGuildTemplate = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/templates/{0}"), CompositeFormat.Parse("/guilds/templates/{0}"));
        public static readonly DiscordApiEndpointKey CreateGuildFromTemplate = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/templates/{0}"), CompositeFormat.Parse("/guilds/templates/{0}"));
        public static readonly DiscordApiEndpointKey GetGuildTemplates = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/templates"), CompositeFormat.Parse("/guilds/{0}/templates"));
        public static readonly DiscordApiEndpointKey CreateGuildTemplate = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/{0}/templates"), CompositeFormat.Parse("/guilds/{0}/templates"));
        public static readonly DiscordApiEndpointKey SyncGuildTemplate = new(HttpMethod.Put, CompositeFormat.Parse("/guilds/{0}/templates/{1}"), CompositeFormat.Parse("/guilds/{0}/templates/{1}"));
        public static readonly DiscordApiEndpointKey ModifyGuildTemplate = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/templates/{1}"), CompositeFormat.Parse("/guilds/{0}/templates/{1}"));
        public static readonly DiscordApiEndpointKey DeleteGuildTemplate = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/templates/{1}"), CompositeFormat.Parse("/guilds/{0}/templates/{1}"));

        // Invite
        public static readonly DiscordApiEndpointKey GetInvite = new(HttpMethod.Get, CompositeFormat.Parse("/invites/{0}"), CompositeFormat.Parse("/invites/{0}"));
        public static readonly DiscordApiEndpointKey DeleteInvite = new(HttpMethod.Delete, CompositeFormat.Parse("/invites/{0}"), CompositeFormat.Parse("/invites/{0}"));

        // Audit Log
        public static readonly DiscordApiEndpointKey GetGuildAuditLog = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/audit-logs"), CompositeFormat.Parse("/guilds/{0}/audit-logs"));

        // Auto Moderation
        public static readonly DiscordApiEndpointKey ListAudioModerationRules = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/auto-moderation/rules"), CompositeFormat.Parse("/guilds/{0}/auto-moderation/rules"));
        public static readonly DiscordApiEndpointKey GetAudioModerationRule = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/auto-moderation/rules/{1}"), CompositeFormat.Parse("/guilds/{0}/auto-moderation/rules/{1}"));
        public static readonly DiscordApiEndpointKey CreateAudioModerationRule = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/{0}/auto-moderation/rules"), CompositeFormat.Parse("/guilds/{0}/auto-moderation/rules"));
        public static readonly DiscordApiEndpointKey ModifyAudioModerationRule = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/auto-moderation/rules/{1}"), CompositeFormat.Parse("/guilds/{0}/auto-moderation/rules/{1}"));
        public static readonly DiscordApiEndpointKey DeleteAudioModerationRule = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/auto-moderation/rules/{1}"), CompositeFormat.Parse("/guilds/{0}/auto-moderation/rules/{1}"));

        // Stickers
        public static readonly DiscordApiEndpointKey GetSticker = new(HttpMethod.Get, CompositeFormat.Parse("/stickers/{0}"), CompositeFormat.Parse("/stickers/{0}"));
        public static readonly DiscordApiEndpointKey ListStarterPacks = new(HttpMethod.Get, CompositeFormat.Parse("/sticker-packs"), CompositeFormat.Parse("/sticker-packs"));
        public static readonly DiscordApiEndpointKey GetStickerPack = new(HttpMethod.Get, CompositeFormat.Parse("/sticker-packs/{0}"), CompositeFormat.Parse("/sticker-packs/{0}"));
        public static readonly DiscordApiEndpointKey ListGuildStickers = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/stickers"), CompositeFormat.Parse("/guilds/{0}/stickers"));
        public static readonly DiscordApiEndpointKey GetGuildSticker = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/stickers/{1}"), CompositeFormat.Parse("/guilds/{0}/stickers/{1}"));
        public static readonly DiscordApiEndpointKey CreateGuildSticker = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/{0}/stickers"), CompositeFormat.Parse("/guilds/{0}/stickers"));
        public static readonly DiscordApiEndpointKey ModifyGuildSticker = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/stickers/{1}"), CompositeFormat.Parse("/guilds/{0}/stickers/{1}"));
        public static readonly DiscordApiEndpointKey DeleteGuildSticker = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/stickers/{1}"), CompositeFormat.Parse("/guilds/{0}/stickers/{1}"));

        // Soundboard
        public static readonly DiscordApiEndpointKey SendSoundboardSound = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/send-soundboard-sound"), CompositeFormat.Parse("/channels/{0}/send-soundboard-sound"));
        public static readonly DiscordApiEndpointKey ListDefaultSoundboardSounds = new(HttpMethod.Get, CompositeFormat.Parse("/soundboard-default-sounds"), CompositeFormat.Parse("/soundboard-default-sounds"));
        public static readonly DiscordApiEndpointKey ListGuildSoundboardSounds = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/soundboard-sounds"), CompositeFormat.Parse("/guilds/{0}/soundboard-sounds"));
        public static readonly DiscordApiEndpointKey GetGuildSoundboardSound = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/soundboard-sounds/{1}"), CompositeFormat.Parse("/guilds/{0}/soundboard-sounds/{1}"));
        public static readonly DiscordApiEndpointKey CreateGuildSoundboardSound = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/{0}/soundboard-sounds"), CompositeFormat.Parse("/guilds/{0}/soundboard-sounds"));
        public static readonly DiscordApiEndpointKey ModifyGuildSoundboardSound = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/soundboard-sounds/{1}"), CompositeFormat.Parse("/guilds/{0}/soundboard-sounds/{1}"));
        public static readonly DiscordApiEndpointKey DeleteGuildSoundboardSound = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/soundboard-sounds/{1}"), CompositeFormat.Parse("/guilds/{0}/soundboard-sounds/{1}"));

        // Emojis
        public static readonly DiscordApiEndpointKey ListGuildEmojis = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/emojis"), CompositeFormat.Parse("/guilds/{0}/emojis"));
        public static readonly DiscordApiEndpointKey GetGuildEmoji = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/emojis/{1}"), CompositeFormat.Parse("/guilds/{0}/emojis/{1}"));
        public static readonly DiscordApiEndpointKey CreateGuildEmoji = new(HttpMethod.Post, CompositeFormat.Parse("/guilds/{0}/emojis"), CompositeFormat.Parse("/guilds/{0}/emojis"));
        public static readonly DiscordApiEndpointKey ModifyGuildEmoji = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/emojis/{1}"), CompositeFormat.Parse("/guilds/{0}/emojis/{1}"));
        public static readonly DiscordApiEndpointKey DeleteGuildEmoji = new(HttpMethod.Delete, CompositeFormat.Parse("/guilds/{0}/emojis/{1}"), CompositeFormat.Parse("/guilds/{0}/emojis/{1}"));
        public static readonly DiscordApiEndpointKey ListApplicationEmojis = new(HttpMethod.Get, CompositeFormat.Parse("/applications/{0}/emojis"), CompositeFormat.Parse("/applications/{0}/emojis"));
        public static readonly DiscordApiEndpointKey GetApplicationEmoji = new(HttpMethod.Get, CompositeFormat.Parse("/applications/{0}/emojis/{1}"), CompositeFormat.Parse("/applications/{0}/emojis/{1}"));
        public static readonly DiscordApiEndpointKey CreateApplicationEmoji = new(HttpMethod.Post, CompositeFormat.Parse("/applications/{0}/emojis"), CompositeFormat.Parse("/applications/{0}/emojis"));
        public static readonly DiscordApiEndpointKey ModifyApplicationEmoji = new(HttpMethod.Patch, CompositeFormat.Parse("/applications/{0}/emojis/{1}"), CompositeFormat.Parse("/applications/{0}/emojis/{1}"));
        public static readonly DiscordApiEndpointKey DeleteApplicationEmoji = new(HttpMethod.Delete, CompositeFormat.Parse("/applications/{0}/emojis/{1}"), CompositeFormat.Parse("/applications/{0}/emojis/{1}"));
    }
}