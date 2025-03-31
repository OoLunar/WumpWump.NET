using WumpWump.Net.Gateway;
using WumpWump.Net.Rest;

namespace WumpWump.Net
{
    public sealed class DiscordClient
    {
        public IDiscordGatewayClient GatewayClient { get; init; }
        public IDiscordRestClient RestClient { get; init; }

        public DiscordClient(IDiscordGatewayClient gatewayClient, IDiscordRestClient restClient)
        {
            GatewayClient = gatewayClient;
            RestClient = restClient;
        }
    }
}
