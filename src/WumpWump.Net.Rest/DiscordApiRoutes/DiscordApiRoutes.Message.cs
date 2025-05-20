using System.Net.Http;
using System.Text;

namespace WumpWump.Net.Rest
{
    public static partial class DiscordApiRoutes
    {
        // Messages
        public static readonly DiscordApiEndpointKey GetChannelMessages = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/messages"), CompositeFormat.Parse("/channels/{0}/messages"));
        public static readonly DiscordApiEndpointKey GetChannelMessage = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/messages/{1}"), CompositeFormat.Parse("/channels/{0}/messages/{1}"));
        public static readonly DiscordApiEndpointKey CreateMessage = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/messages"), CompositeFormat.Parse("/channels/{0}/messages"));
        public static readonly DiscordApiEndpointKey CrosspostMessage = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/messages/{1}/crosspost"), CompositeFormat.Parse("/channels/{0}/messages/{1}/crosspost"));
        public static readonly DiscordApiEndpointKey CreateReaction = new(HttpMethod.Put, CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions/{2}/@me"), CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions/{2}/@me"));
        public static readonly DiscordApiEndpointKey DeleteOwnReaction = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions/{2}/@me"), CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions/{2}/@me"));
        public static readonly DiscordApiEndpointKey DeleteUserReaction = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions/{2}/{3}"), CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions/{2}/{3}"));
        public static readonly DiscordApiEndpointKey GetReactions = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions/{2}"), CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions/{2}"));
        public static readonly DiscordApiEndpointKey DeleteAllReactions = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions"), CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions"));
        public static readonly DiscordApiEndpointKey DeleteAllReactionsForEmoji = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions/{2}"), CompositeFormat.Parse("/channels/{0}/messages/{1}/reactions/{2}"));
        public static readonly DiscordApiEndpointKey EditMessage = new(HttpMethod.Patch, CompositeFormat.Parse("/channels/{0}/messages/{1}"), CompositeFormat.Parse("/channels/{0}/messages/{1}"));
        public static readonly DiscordApiEndpointKey DeleteMessage = new(HttpMethod.Delete, CompositeFormat.Parse("/channels/{0}/messages/{1}"), CompositeFormat.Parse("/channels/{0}/messages/{1}"));
        public static readonly DiscordApiEndpointKey BulkDeleteMessages = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/messages/bulk-delete"), CompositeFormat.Parse("/channels/{0}/messages/bulk-delete"));

        // Polls
        public static readonly DiscordApiEndpointKey GetAnswerVoters = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/polls/{1}/answers/{2}"), CompositeFormat.Parse("/channels/{0}/messages/{1}/polls/{2}/answers/{3}"));
        public static readonly DiscordApiEndpointKey EndPoll = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/polls/{1}/expire"), CompositeFormat.Parse("/channels/{0}/messages/{1}/polls/{2}/expire"));
    }
}