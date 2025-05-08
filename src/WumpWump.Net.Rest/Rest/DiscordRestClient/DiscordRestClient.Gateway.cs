using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WumpWump.Net.Rest.Entities.Gateway;

namespace WumpWump.Net.Rest
{
    public partial class DiscordRestClient : IDiscordRestClient
    {
        public async ValueTask<DiscordApiResponse<DiscordGatewayInformation>> GetGatewayBotInformationAsync(CancellationToken cancellationToken = default) => await SendAsync<DiscordGatewayInformation>(new()
        {
            Method = HttpMethod.Get,
            Route = new Uri("https://discord.com/api/v10/gateway/"),
            Url = new Uri("https://discord.com/api/v10/gateway/bot"),
        }, cancellationToken);
    }
}
