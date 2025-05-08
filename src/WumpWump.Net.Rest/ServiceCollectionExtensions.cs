using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WumpWump.Net.Rest.RateLimits;

namespace WumpWump.Net.Rest
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordRestClient(this IServiceCollection services, string discordToken)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            ArgumentException.ThrowIfNullOrWhiteSpace(discordToken, nameof(discordToken));

            services.AddSingleton(new DiscordRestClientOptions
            {
                DiscordToken = discordToken
            });

            services.TryAddSingleton<HttpClient>();
            services.TryAddSingleton<IDiscordRateLimiter, DiscordRateLimiter>();
            services.TryAddSingleton<DiscordRestClient>();
            return services;
        }
    }
}
