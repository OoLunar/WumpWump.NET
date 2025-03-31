using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace WumpWump.Net.Rest.RateLimits
{
    public class DiscordRateLimiter : IDiscordRateLimiter
    {
        protected readonly MemoryCache _cache = new(new MemoryCacheOptions());
        protected readonly ILogger<DiscordRateLimiter> _logger = NullLogger<DiscordRateLimiter>.Instance;

        public ValueTask<DiscordRateLimitRequestData> GetDiscordRateLimitDataAsync(DiscordApiRequest apiRequest, CancellationToken cancellationToken = default)
        {
            int tokenHash = 0;
            if (apiRequest.Headers.TryGetValue("Authorization", out IEnumerable<string>? values))
            {
                foreach (string value in values)
                {
                    if (tokenHash != 0)
                    {
                        throw new InvalidOperationException("Authorization header contains multiple values. Only one is allowed.");
                    }
                    else if (value.StartsWith("Bot ", StringComparison.OrdinalIgnoreCase))
                    {
                        tokenHash = value[4..].GetHashCode();
                    }
                    else if (value.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        tokenHash = value[7..].GetHashCode();
                    }

                    throw new InvalidOperationException("Authorization header does not contain a valid token. The header value must always start with 'Bot ' or 'Bearer '.");
                }
            }

            return ValueTask.FromResult(new DiscordRateLimitRequestData()
            {
                Id = apiRequest.Id,
                Method = apiRequest.Method,
                Route = apiRequest.Route,
                TokenHash = tokenHash,
                Url = apiRequest.Url
            });
        }

        public async ValueTask<IDisposable> ReserveAsync(DiscordRateLimitRequestData rateLimitRequestData, CancellationToken cancellationToken = default)
        {
            DiscordRateLimitBucket globalbucket = GetOrCreateGlobalBucket(rateLimitRequestData.TokenHash);
            while (!globalbucket.TryReserve())
            {
                _logger.LogWarning("Global rate limit reached, waiting for reset: {ResetAt}", globalbucket.SharedReset);
                await Task.Delay((int)(globalbucket.Reset - DateTimeOffset.UtcNow).TotalMilliseconds, cancellationToken);
                globalbucket = GetOrCreateGlobalBucket(rateLimitRequestData.TokenHash);
            }

            DiscordRateLimitBucket bucket = GetOrCreateBucket(rateLimitRequestData);
            while (!bucket.TryReserve())
            {
                _logger.LogWarning("Rate limit reached for {Route}, waiting for reset: {ResetAt}", rateLimitRequestData.Route, DateTimeOffset.Compare(bucket.SharedReset, DateTimeOffset.UtcNow) > 0 ? bucket.SharedReset : bucket.Reset);
                await Task.Delay((int)(bucket.Reset - DateTimeOffset.UtcNow).TotalMilliseconds, cancellationToken);
                bucket = GetOrCreateBucket(rateLimitRequestData);
            }

            _logger.LogTrace("{Route}: {Reserved} Reserved, {Remaining}/{Limit} Remain", rateLimitRequestData.Route, bucket.Reserved, bucket.Remaining, bucket.Limit);
            return new DiscordRateLimitReservation(globalbucket, bucket);
        }

        public ValueTask UpdateAsync(DiscordRateLimitRequestData rateLimitRequestData, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers, CancellationToken cancellationToken = default)
        {
            DiscordRateLimitBucket bucket = GetOrCreateBucket(rateLimitRequestData);

            // Test if we've hit a global rate limit, which provides
            // zero information except on the global bucket.
            if (bool.TryParse(GetHeader(headers, "X-RateLimit-Global"), out bool isGlobal) && isGlobal)
            {
                DiscordRateLimitBucket globalBucket = GetOrCreateGlobalBucket(rateLimitRequestData.TokenHash);
                globalBucket.Remaining = 0;

                string? resetAfter = GetHeader(headers, "Retry-After") ?? throw new InvalidOperationException("Discord did not provide a Retry-After header.");
                globalBucket.SharedReset = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(int.Parse(resetAfter, CultureInfo.InvariantCulture));
                return default;
            }

            // Hash will be null on the first request to a bucket.
            if (bucket.Hash is null)
            {
                // Populate the bucket with the other header values.
                bucket.Hash = GetHeader(headers, "X-RateLimit-Bucket");
                bucket.Limit = int.Parse(GetHeader(headers, "X-RateLimit-Limit") ?? "1", CultureInfo.InvariantCulture);
                bucket.Remaining = int.Parse(GetHeader(headers, "X-RateLimit-Remaining") ?? "1", CultureInfo.InvariantCulture);

                string reset = GetHeader(headers, "X-RateLimit-Reset") ?? throw new InvalidOperationException("Discord did not provide a X-RateLimit-Reset header.");
                bucket.Reset = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(reset.Replace(".", string.Empty), CultureInfo.InvariantCulture));
                _logger.LogDebug("Associated bucket {Hash} with {Route}. {Remaining}/{Limit} remaining, resets at {Reset}", bucket.Hash, rateLimitRequestData.Route, bucket.Remaining, bucket.Limit, bucket.Reset);
            }

            // Test if we've hit the "shared" rate limit.
            // If we have, set the bucket remaining to 0 and change the
            // reset time to the shared reset time (Reset-After header).
            string? scope = GetHeader(headers, "X-RateLimit-Scope");
            if (scope == "shared")
            {
                bucket.Remaining = 0;

                string resetAfter = GetHeader(headers, "Retry-After") ?? throw new InvalidOperationException("Discord did not provide a Retry-After header.");
                bucket.SharedReset = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(int.Parse(resetAfter, CultureInfo.InvariantCulture));
            }

            return default;
        }

        protected DiscordRateLimitBucket GetOrCreateGlobalBucket(int tokenHashCode)
        {
            if (!_cache.TryGetValue($"{tokenHashCode}:global", out DiscordRateLimitBucket? bucket) || bucket is null)
            {
                bucket = new DiscordRateLimitBucket
                {
                    TokenHashCode = tokenHashCode,
                    Hash = "global",
                    Route = new Uri("https://discord.com/api/v10/"),
                    Limit = 50,
                    Remaining = 50,
                    Reset = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(1),
                    SharedReset = DateTimeOffset.MinValue
                };

                _logger.LogTrace("Created global rate limit bucket for {TokenHashCode}: {Bucket}", tokenHashCode, bucket);
                _cache.Set("global", bucket, bucket.Reset);
            }

            return bucket;
        }

        protected DiscordRateLimitBucket GetOrCreateBucket(DiscordRateLimitRequestData rateLimitRequestData)
        {
            if (!_cache.TryGetValue(rateLimitRequestData.Route, out DiscordRateLimitBucket? bucket) || bucket is null)
            {
                bucket = new DiscordRateLimitBucket
                {
                    TokenHashCode = rateLimitRequestData.TokenHash,
                    Route = rateLimitRequestData.Route,
                    Limit = 1,
                    Remaining = 1,
                    Reserved = 0,
                    Reset = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(30),
                    SharedReset = DateTimeOffset.MinValue
                };

                _logger.LogTrace("Created rate limit bucket for {Route}: {Bucket}", rateLimitRequestData.Route, bucket);
                _cache.Set(rateLimitRequestData.Route, bucket, bucket.Reset);
            }

            return bucket;
        }

        protected static string? GetHeader(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers, string key)
        {
            string? value = null;
            foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
            {
                if (!header.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                foreach (string headerValue in header.Value)
                {
                    if (value is not null)
                    {
                        throw new InvalidOperationException($"Discord provided multiple {key} headers.");
                    }

                    value = headerValue;
                }
            }

            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
    }
}
