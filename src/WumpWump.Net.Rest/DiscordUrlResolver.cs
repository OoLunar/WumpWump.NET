using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace WumpWump.Net.Rest
{
    public class DiscordUrlResolver : IDiscordUrlResolver
    {
        protected readonly string _domain;
        protected readonly string _apiPath;
        protected readonly string _apiVersion;

        public DiscordUrlResolver() : this("discord.com", "api", "v10") { }
        public DiscordUrlResolver(string domain, string apiPath, string apiVersion)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(domain, nameof(domain));
            ArgumentException.ThrowIfNullOrWhiteSpace(apiPath, nameof(apiPath));
            ArgumentException.ThrowIfNullOrWhiteSpace(apiVersion, nameof(apiVersion));

            _domain = domain;
            _apiPath = apiPath;
            _apiVersion = apiVersion;
        }

        public string GetDomain() => _domain;
        public string GetApiPath() => _apiPath;
        public string GetApiVersion() => _apiVersion;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetBaseUrl() => $"https://{_domain}/{_apiPath}/{_apiVersion}";

        public DiscordApiEndpoint GetEndpoint(DiscordApiEndpointKey key, params ReadOnlySpan<object?> arguments) => new()
        {
            Method = key.Method,
            Url = new Uri($"{GetBaseUrl()}/{string.Format(CultureInfo.InvariantCulture, key.Path, arguments)}"),
            EndpointKey = string.Format(CultureInfo.InvariantCulture, key.Endpoint, arguments),
        };
    }
}
