using System;

namespace WumpWump.Net.Rest.RateLimits
{
    public record DiscordRateLimitBucket
    {
        public required int TokenHashCode { get; init; }
        public required string Key { get; init; }
        public required int Limit { get; set; }
        public required int Remaining { get; set; }
        public required DateTimeOffset Reset { get; set; }
        public required DateTimeOffset SharedReset { get; set; }
        public string? Hash { get; set; }
        public int Reserved { get; set; }

        public bool TryReserve()
        {
            // If the SharedReset is in the future, we are rate limited.
            if (SharedReset > DateTimeOffset.UtcNow)
            {
                return false;
            }
            // If the Reset is in the future and we have no remaining requests, we are rate limited.
            else if (Reset > DateTimeOffset.UtcNow && Remaining == 0)
            {
                return false;
            }
            // If we cannot reserve any more requests, we are rate limited.
            else if (Reserved >= Limit)
            {
                return false;
            }

            Reserved++;
            return true;
        }
    }
}
