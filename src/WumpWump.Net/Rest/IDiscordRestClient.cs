using System.Threading;
using System.Threading.Tasks;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Rest
{
    public interface IDiscordRestClient
    {
        ValueTask<DiscordApiResponse<DiscordApplication>> GetCurrentApplicationAsync(CancellationToken cancellationToken = default);

        ValueTask<DiscordApiResponse<T>> SendAsync<T>(DiscordApiRequest request, CancellationToken cancellationToken = default);
    }
}
