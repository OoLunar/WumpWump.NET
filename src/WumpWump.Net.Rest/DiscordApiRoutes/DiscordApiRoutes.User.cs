using System.Net.Http;
using System.Text;

namespace WumpWump.Net.Rest
{
    public static partial class DiscordApiRoutes
    {
        // User
        public static readonly DiscordApiEndpointKey GetCurrentUser = new(HttpMethod.Get, CompositeFormat.Parse("/users/@me"), CompositeFormat.Parse("/users/@me"));
        public static readonly DiscordApiEndpointKey GetUser = new(HttpMethod.Get, CompositeFormat.Parse("/users/{0}"), CompositeFormat.Parse("/users/{0}"));
        public static readonly DiscordApiEndpointKey ModifyCurrentUser = new(HttpMethod.Patch, CompositeFormat.Parse("/users/@me"), CompositeFormat.Parse("/users/@me"));
        public static readonly DiscordApiEndpointKey GetCurrentUserGuilds = new(HttpMethod.Get, CompositeFormat.Parse("/users/@me/guilds"), CompositeFormat.Parse("/users/@me/guilds"));
        public static readonly DiscordApiEndpointKey LeaveGuild = new(HttpMethod.Delete, CompositeFormat.Parse("/users/@me/guilds/{0}"), CompositeFormat.Parse("/users/@me/guilds/{0}"));
        public static readonly DiscordApiEndpointKey CreateDm = new(HttpMethod.Post, CompositeFormat.Parse("/users/@me/channels"), CompositeFormat.Parse("/users/@me/channels"));
        public static readonly DiscordApiEndpointKey GetUserConnections = new(HttpMethod.Get, CompositeFormat.Parse("/users/@me/connections"), CompositeFormat.Parse("/users/@me/connections"));
        public static readonly DiscordApiEndpointKey GetCurrentUserApplicationRoleConnection = new(HttpMethod.Get, CompositeFormat.Parse("/users/@me/applications/{0}/role-connection"), CompositeFormat.Parse("/users/@me/applications/{0}/role-connection"));
        public static readonly DiscordApiEndpointKey UpdateCurrentUserApplicationRoleConnection = new(HttpMethod.Put, CompositeFormat.Parse("/users/@me/applications/{0}/role-connection"), CompositeFormat.Parse("/users/@me/applications/{0}/role-connection"));

        // Voice
        public static readonly DiscordApiEndpointKey ListVoiceRegions = new(HttpMethod.Get, CompositeFormat.Parse("/voice/regions"), CompositeFormat.Parse("/voice/regions"));
        public static readonly DiscordApiEndpointKey GetCurrentUserVoiceState = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/voice-states/@me"), CompositeFormat.Parse("/guilds/{0}/voice-states/@me"));
        public static readonly DiscordApiEndpointKey GetUserVoiceState = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/voice-states/{1}"), CompositeFormat.Parse("/guilds/{0}/voice-states/{1}"));
        public static readonly DiscordApiEndpointKey ModifyCurrentUserVoiceState = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/voice-states/@me"), CompositeFormat.Parse("/guilds/{0}/voice-states/@me"));
        public static readonly DiscordApiEndpointKey ModifyUserVoiceState = new(HttpMethod.Patch, CompositeFormat.Parse("/guilds/{0}/voice-states/{1}"), CompositeFormat.Parse("/guilds/{0}/voice-states/{1}"));
    }
}