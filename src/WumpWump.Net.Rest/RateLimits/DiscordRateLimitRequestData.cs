using System;
using System.Net.Http;

namespace WumpWump.Net.Rest.RateLimits
{
    public record DiscordRateLimitRequestData
    {
        public required Ulid Id { get; init; }
        public required int TokenHash { get; init; }
        public required HttpMethod Method { get; init; }
        public required string Key { get; init; }
        public required Uri Url { get; init; }

        public string BucketHashKey => $"{TokenHash}:{Key}:{Method}";
    }
}
