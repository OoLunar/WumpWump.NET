using System.Net.Http;
using System.Text;

namespace WumpWump.Net.Rest
{
    public static partial class DiscordApiRoutes
    {
        // Application
        public static readonly DiscordApiEndpointKey GetCurrentApplication = new(HttpMethod.Get, CompositeFormat.Parse("/applications/@me"), CompositeFormat.Parse("/applications/@me"));
        public static readonly DiscordApiEndpointKey EditCurrentApplication = new(HttpMethod.Patch, CompositeFormat.Parse("/applications/@me"), CompositeFormat.Parse("/applications/@me"));
        public static readonly DiscordApiEndpointKey GetApplicationActivityInstance = new(HttpMethod.Get, CompositeFormat.Parse("/applications/{0}/activity-instances/{1}"), CompositeFormat.Parse("/applications/{0}/activity-instances/"));

        // Application Role Connection Metadata
        public static readonly DiscordApiEndpointKey GetCurrentUserApplicationRoleConnectionMetadata = new(HttpMethod.Get, CompositeFormat.Parse("/applications/{0}/role-connections/metadata"), CompositeFormat.Parse("/applications/{0}/role-connections/metadata"));
        public static readonly DiscordApiEndpointKey UpdateCurrentUserApplicationRoleConnectionMetadata = new(HttpMethod.Put, CompositeFormat.Parse("/applications/{0}/role-connections/metadata"), CompositeFormat.Parse("/applications/{0}/role-connections/metadata"));

        // Entitlement
        public static readonly DiscordApiEndpointKey ListEntitlements = new(HttpMethod.Get, CompositeFormat.Parse("/applications/{0}/entitlements"), CompositeFormat.Parse("/applications/{0}/entitlements"));
        public static readonly DiscordApiEndpointKey GetEntitlement = new(HttpMethod.Get, CompositeFormat.Parse("/applications/{0}/entitlements/{1}"), CompositeFormat.Parse("/applications/{0}/entitlements/{1}"));
        public static readonly DiscordApiEndpointKey ConsumeEntitlement = new(HttpMethod.Post, CompositeFormat.Parse("/applications/{0}/entitlements/{1}/consume"), CompositeFormat.Parse("/applications/{0}/entitlements/{1}/consume"));
        public static readonly DiscordApiEndpointKey CreateTestEntitlement = new(HttpMethod.Post, CompositeFormat.Parse("/applications/{0}/entitlements"), CompositeFormat.Parse("/applications/{0}/entitlements"));
        public static readonly DiscordApiEndpointKey DeleteTestEntitlement = new(HttpMethod.Delete, CompositeFormat.Parse("/applications/{0}/entitlements/{1}"), CompositeFormat.Parse("/applications/{0}/entitlements/{1}"));

        // SKU
        public static readonly DiscordApiEndpointKey ListSkus = new(HttpMethod.Get, CompositeFormat.Parse("/applications/{0}/skus"), CompositeFormat.Parse("/applications/{0}/skus"));

        // Subscription
        public static readonly DiscordApiEndpointKey ListSkuSubscriptions = new(HttpMethod.Get, CompositeFormat.Parse("/skus/{0}/subscriptions"), CompositeFormat.Parse("/skus/{0}/subscriptions"));
        public static readonly DiscordApiEndpointKey GetSkuSubscription = new(HttpMethod.Get, CompositeFormat.Parse("/skus/{0}/subscriptions/{1}"), CompositeFormat.Parse("/skus/{0}/subscriptions/{1}"));

        // Gateway
        public static readonly DiscordApiEndpointKey GetGateway = new(HttpMethod.Get, CompositeFormat.Parse("/gateway"), CompositeFormat.Parse("/gateway"));
        public static readonly DiscordApiEndpointKey GetGatewayBot = new(HttpMethod.Get, CompositeFormat.Parse("/gateway/bot"), CompositeFormat.Parse("/gateway/bot"));
    }
}