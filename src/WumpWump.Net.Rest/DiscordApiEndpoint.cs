using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest
{
    public record DiscordApiEndpoint
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

        public DiscordApiEndpoint WithQueryParameters(params ReadOnlySpan<(string, DiscordOptional<object?>)> queryParameters)
        {
            UriBuilder builder = new(_url);
            StringBuilder queryBuilder = new(builder.Query);
            if (queryBuilder.Length > 0)
            {
                queryBuilder.Append('&');
            }

            foreach ((string, DiscordOptional<object?>) queryParameter in queryParameters)
            {
                if (!queryParameter.Item2.HasValue)
                {
                    continue;
                }
                else if (queryParameter.Item2.Value is IEnumerable enumerable)
                {
                    queryBuilder.Append($"{queryParameter.Item1}={string.Join(',', enumerable)}&");
                }
                else
                {
                    queryBuilder.Append($"{queryParameter.Item1}={queryParameter.Item2.Value ?? "null"}&");
                }
            }

            if (queryBuilder.Length > 0)
            {
                queryBuilder.Length -= 1; // Remove the trailing '&'
            }

            builder.Query = queryBuilder.ToString();
            _url = builder.Uri;
            return this;
        }
    }
}