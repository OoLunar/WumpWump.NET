using System;
using System.Collections;
using System.Net.Http;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest
{
    public class DiscordApiEndpoint
    {
        public required HttpMethod Method { get; init; }
        public required Uri Url { get => _url; init => _url = value; }
        public required string EndpointKey { get; init; }

        protected Uri _url = null!;

        public DiscordApiEndpoint WithQueryParameter<T>(string key, DiscordOptional<T> value)
        {
            if (!value.HasValue)
            {
                return this;
            }

            UriBuilder builder = new(_url);
            string queryValue = value.Value is IEnumerable enumerable ? string.Join(',', enumerable) : value.Value?.ToString() ?? "null";
            builder.Query = string.IsNullOrEmpty(builder.Query) ? $"{key}={queryValue}" : $"{builder.Query}&{key}={queryValue}";
            _url = builder.Uri;

            return this;
        }
    }
}