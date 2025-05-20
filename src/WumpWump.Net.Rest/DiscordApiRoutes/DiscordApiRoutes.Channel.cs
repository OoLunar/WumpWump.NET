using System.Net.Http;
using System.Text;

namespace WumpWump.Net.Rest
{
    public static partial class DiscordApiRoutes
    {
        // Channel
        public static readonly DiscordApiEndpointKey GetChannel = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}"), CompositeFormat.Parse("/channels/{0}"));
        public static readonly DiscordApiEndpointKey ModifyChannel = new(HttpMethod.Patch, CompositeFormat.Parse("/channels/{0}"), CompositeFormat.Parse("/channels/{0}"));
        public static readonly DiscordApiEndpointKey DeleteChannel = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}"), CompositeFormat.Parse("/channels/{0}"));
        public static readonly DiscordApiEndpointKey EditChannelPermissions = new(HttpMethod.Put, CompositeFormat.Parse("/channels/{0}/permissions/{1}"), CompositeFormat.Parse("/channels/{0}/permissions/{1}"));
        public static readonly DiscordApiEndpointKey GetChannelInvites = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/invites"), CompositeFormat.Parse("/channels/{0}/invites"));
        public static readonly DiscordApiEndpointKey CreateChannelInvite = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/invites"), CompositeFormat.Parse("/channels/{0}/invites"));
        public static readonly DiscordApiEndpointKey DeleteChannelPermission = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}/permissions/{1}"), CompositeFormat.Parse("/channels/{0}/permissions/{1}"));
        public static readonly DiscordApiEndpointKey FollowAnnouncementChannel = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/followers"), CompositeFormat.Parse("/channels/{0}/followers"));
        public static readonly DiscordApiEndpointKey TriggerTypingIndicator = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/typing"), CompositeFormat.Parse("/channels/{0}/typing"));
        public static readonly DiscordApiEndpointKey GetPinnedMessages = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/pins"), CompositeFormat.Parse("/channels/{0}/pins"));
        public static readonly DiscordApiEndpointKey PinMessage = new(HttpMethod.Put, CompositeFormat.Parse("/channels/{0}/pins/{1}"), CompositeFormat.Parse("/channels/{0}/pins/{1}"));
        public static readonly DiscordApiEndpointKey UnpinMessage = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}/pins/{1}"), CompositeFormat.Parse("/channels/{0}/pins/{1}"));
        public static readonly DiscordApiEndpointKey GroupDmAddRecipient = new(HttpMethod.Put, CompositeFormat.Parse("/channels/{0}/recipients/{1}"), CompositeFormat.Parse("/channels/{0}/recipients/{1}"));
        public static readonly DiscordApiEndpointKey GroupDmRemoveRecipient = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}/recipients/{1}"), CompositeFormat.Parse("/channels/{0}/recipients/{1}"));
        public static readonly DiscordApiEndpointKey StartThreadFromMessage = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/messages/{1}/threads"), CompositeFormat.Parse("/channels/{0}/messages/{1}/threads"));
        public static readonly DiscordApiEndpointKey StartThreadWithoutMessage = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/threads"), CompositeFormat.Parse("/channels/{0}/threads"));
        public static readonly DiscordApiEndpointKey JoinThread = new(HttpMethod.Put, CompositeFormat.Parse("/channels/{0}/thread-members/@me"), CompositeFormat.Parse("/channels/{0}/thread-members/@me"));
        public static readonly DiscordApiEndpointKey AddThreadMember = new(HttpMethod.Put, CompositeFormat.Parse("/channels/{0}/thread-members/{1}"), CompositeFormat.Parse("/channels/{0}/thread-members/{1}"));
        public static readonly DiscordApiEndpointKey LeaveThread = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}/thread-members/@me"), CompositeFormat.Parse("/channels/{0}/thread-members/@me"));
        public static readonly DiscordApiEndpointKey RemoveThreadMember = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}/thread-members/{1}"), CompositeFormat.Parse("/channels/{0}/thread-members/{1}"));
        public static readonly DiscordApiEndpointKey GetThreadMember = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/thread-members/{1}"), CompositeFormat.Parse("/channels/{0}/thread-members/{1}"));
        public static readonly DiscordApiEndpointKey ListThreadMembers = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/thread-members"), CompositeFormat.Parse("/channels/{0}/thread-members"));
        public static readonly DiscordApiEndpointKey ListPublicArchivedThreads = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/threads/archived/public"), CompositeFormat.Parse("/channels/{0}/threads/archived/public"));
        public static readonly DiscordApiEndpointKey ListPrivateArchivedThreads = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/threads/archived/private"), CompositeFormat.Parse("/channels/{0}/threads/archived/private"));
        public static readonly DiscordApiEndpointKey ListJoinedPrivateArchivedThreads = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/users/@me/threads/archived/private"), CompositeFormat.Parse("/channels/{0}/users/@me/threads/archived/private"));

        // Stage Instances
        public static readonly DiscordApiEndpointKey CreateStageInstance = new(HttpMethod.Post, CompositeFormat.Parse("/stage-instances"), CompositeFormat.Parse("/stage-instances"));
        public static readonly DiscordApiEndpointKey GetStageInstance = new(HttpMethod.Get, CompositeFormat.Parse("/stage-instances/{0}"), CompositeFormat.Parse("/stage-instances/{0}"));
        public static readonly DiscordApiEndpointKey ModifyStageInstance = new(HttpMethod.Patch, CompositeFormat.Parse("/stage-instances/{0}"), CompositeFormat.Parse("/stage-instances/{0}"));
        public static readonly DiscordApiEndpointKey DeleteStageInstance = new(HttpMethod.Delete, CompositeFormat.Parse("/stage-instances/{0}"), CompositeFormat.Parse("/stage-instances/{0}"));
    }
}