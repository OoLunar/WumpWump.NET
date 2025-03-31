using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WumpWump.Net.Rest.RateLimits
{
    public interface IDiscordRateLimiter
    {
        public ValueTask<IDisposable> ReserveAsync(DiscordRateLimitRequestData rateLimitRequestData, CancellationToken cancellationToken = default);
        public ValueTask UpdateAsync(DiscordRateLimitRequestData rateLimitRequestData, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers, CancellationToken cancellationToken = default);

        public ValueTask<DiscordRateLimitRequestData> GetDiscordRateLimitDataAsync(DiscordApiRequest apiRequest, CancellationToken cancellationToken = default);
    }
}
