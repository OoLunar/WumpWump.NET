using System.Net.Http;
using System.Text;

namespace WumpWump.Net.Rest
{
    public static partial class DiscordApiRoutes
    {
        // Webhook
        public static readonly DiscordApiEndpointKey CreateWebhook = new(HttpMethod.Post, CompositeFormat.Parse("/channels/{0}/webhooks"), CompositeFormat.Parse("/channels/{0}/webhooks"));
        public static readonly DiscordApiEndpointKey GetChannelWebhooks = new(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/webhooks"), CompositeFormat.Parse("/channels/{0}/webhooks"));
        public static readonly DiscordApiEndpointKey GetGuildWebhooks = new(HttpMethod.Get, CompositeFormat.Parse("/guilds/{0}/webhooks"), CompositeFormat.Parse("/guilds/{0}/webhooks"));
        public static readonly DiscordApiEndpointKey GetWebhook = new(HttpMethod.Get, CompositeFormat.Parse("/webhooks/{0}"), CompositeFormat.Parse("/webhooks/{0}"));
        public static readonly DiscordApiEndpointKey GetWebhookWithToken = new(HttpMethod.Get, CompositeFormat.Parse("/webhooks/{0}/{1}"), CompositeFormat.Parse("/webhooks/{0}/{1}"));
        public static readonly DiscordApiEndpointKey ModifyWebhook = new(HttpMethod.Patch, CompositeFormat.Parse("/webhooks/{0}"), CompositeFormat.Parse("/webhooks/{0}"));
        public static readonly DiscordApiEndpointKey ModifyWebhookWithToken = new(HttpMethod.Patch, CompositeFormat.Parse("/webhooks/{0}/{1}"), CompositeFormat.Parse("/webhooks/{0}/{1}"));
        public static readonly DiscordApiEndpointKey DeleteWebhook = new(HttpMethod.Delete, CompositeFormat.Parse("/webhooks/{0}"), CompositeFormat.Parse("/webhooks/{0}"));
        public static readonly DiscordApiEndpointKey DeleteWebhookWithToken = new(HttpMethod.Delete, CompositeFormat.Parse("/webhooks/{0}/{1}"), CompositeFormat.Parse("/webhooks/{0}/{1}"));
        public static readonly DiscordApiEndpointKey ExecuteWebhook = new(HttpMethod.Post, CompositeFormat.Parse("/webhooks/{0}/{1}"), CompositeFormat.Parse("/webhooks/{0}/{1}"));
        public static readonly DiscordApiEndpointKey ExecuteSlackCompatibleWebhook = new(HttpMethod.Post, CompositeFormat.Parse("/webhooks/{0}/{1}/slack"), CompositeFormat.Parse("/webhooks/{0}/{1}"));
        public static readonly DiscordApiEndpointKey ExecuteGitHubCompatibleWebhook = new(HttpMethod.Post, CompositeFormat.Parse("/webhooks/{0}/{1}/github"), CompositeFormat.Parse("/webhooks/{0}/{1}"));
        public static readonly DiscordApiEndpointKey GetWebhookMessage = new(HttpMethod.Get, CompositeFormat.Parse("/webhooks/{0}/{1}/messages/{2}"), CompositeFormat.Parse("/webhooks/{0}/{1}/messages/{2}"));
        public static readonly DiscordApiEndpointKey EditWebhookMessage = new(HttpMethod.Patch, CompositeFormat.Parse("/webhooks/{0}/{1}/messages/{2}"), CompositeFormat.Parse("/webhooks/{0}/{1}/messages/{2}"));
        public static readonly DiscordApiEndpointKey DeleteWebhookMessage = new(HttpMethod.Delete, CompositeFormat.Parse("/webhooks/{0}/{1}/messages/{2}"), CompositeFormat.Parse("/webhooks/{0}/{1}/messages/{2}"));
    }
}