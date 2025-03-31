using System;

namespace WumpWump.Net.Rest.RateLimits
{
    public class DiscordRateLimitReservation : IDisposable
    {
        protected readonly DiscordRateLimitBucket _globalBucket;
        protected readonly DiscordRateLimitBucket _bucket;
        protected object? _state;

        public DiscordRateLimitReservation(DiscordRateLimitBucket globalBucket, DiscordRateLimitBucket bucket)
        {
            _globalBucket = globalBucket;
            _bucket = bucket;
        }

        public void Dispose()
        {
            if (_state is not null)
            {
                return;
            }

            _state = new();
            _globalBucket.Reserved--;
            _globalBucket.Remaining--;
            _bucket.Reserved--;
            _bucket.Remaining--;
            GC.SuppressFinalize(this);
        }
    }
}
