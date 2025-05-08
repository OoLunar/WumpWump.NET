using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WumpWump.Net.Rest.Json;
using WumpWump.Net.Rest.RateLimits;

namespace WumpWump.Net.Rest
{
    public partial class DiscordRestClient : IDiscordRestClient
    {
        public static readonly JsonSerializerOptions DefaultSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            TypeInfoResolver = DiscordJsonTypeInfoResolver.Default,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Converters = { new DiscordColorJsonConverter() }
        };

        public DiscordRestClientOptions RestClientOptions { get; init; }
        protected readonly HttpClient _httpClient;
        protected readonly IDiscordRateLimiter _rateLimiter;
        protected readonly ILogger<DiscordRestClient> _logger;

        public DiscordRestClient(string discordToken, HttpClient? httpClient = null, IDiscordRateLimiter? rateLimiter = null, ILogger<DiscordRestClient>? logger = null)
            : this(new DiscordRestClientOptions() { DiscordToken = discordToken }, httpClient ?? new(), rateLimiter ?? new DiscordRateLimiter(), logger ?? NullLogger<DiscordRestClient>.Instance) { }

        [ActivatorUtilitiesConstructor]
        public DiscordRestClient(DiscordRestClientOptions restClientOptions, HttpClient httpClient, IDiscordRateLimiter rateLimiter, ILogger<DiscordRestClient> logger)
        {
            RestClientOptions = restClientOptions ?? throw new ArgumentNullException(nameof(restClientOptions));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _rateLimiter = rateLimiter ?? throw new ArgumentNullException(nameof(rateLimiter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask<DiscordApiResponse<T>> SendAsync<T>(DiscordApiRequest request, CancellationToken cancellationToken = default)
        {
            using HttpRequestMessage httpRequest = new(request.Method, request.Url);
            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            {
                if (!httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value))
                {
                    _logger.LogWarning("Request [{Id}]: Failed to add header {HeaderKey} to request", request.Id, header.Key);
                }
            }

            if (!httpRequest.Headers.Contains("Authorization"))
            {
                httpRequest.Headers.Add("Authorization", $"Bot {RestClientOptions.DiscordToken}");
            }

            if (request.Body is not null)
            {
                httpRequest.Content = JsonContent.Create(request.Body, null, DefaultSerializerOptions);
            }

            DiscordRateLimitRequestData rateLimitData = await _rateLimiter.GetDiscordRateLimitDataAsync(request, cancellationToken);
            using IDisposable rateLimitLock = await _rateLimiter.ReserveAsync(rateLimitData, cancellationToken);
            using HttpResponseMessage httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);
            await _rateLimiter.UpdateAsync(rateLimitData, httpResponse.Headers, cancellationToken);

            if (httpResponse.StatusCode is HttpStatusCode.TooManyRequests)
            {
                // Try again
                return await SendAsync<T>(request, cancellationToken);
            }
            else if (httpResponse.StatusCode is HttpStatusCode.NotFound)
            {
                // This *should* always be okay, as the API should return a 404 with an empty body
                // and our entities should always be reference types.
                // I'm doing this because I don't want to throw a 404 exception when it's entirely expected.
                return new DiscordApiResponse<T>
                {
                    Id = request.Id,
                    StatusCode = httpResponse.StatusCode,
                    Headers = httpResponse.Headers,
                    Error = null,
                    Data = default!
                };
            }
            else if (httpResponse.StatusCode is HttpStatusCode.NoContent)
            {
                return new DiscordApiResponse<T>
                {
                    Id = request.Id,
                    StatusCode = httpResponse.StatusCode,
                    Headers = httpResponse.Headers,
                    Error = null,
                    Data = default!
                };
            }
            else if (httpResponse.IsSuccessStatusCode)
            {
                return new DiscordApiResponse<T>
                {
                    Id = request.Id,
                    StatusCode = httpResponse.StatusCode,
                    Headers = httpResponse.Headers,
                    Error = null,
                    Data = await httpResponse.Content.ReadFromJsonAsync<T>(DefaultSerializerOptions, cancellationToken) ?? throw new JsonException("Failed to deserialize response.")
                };
            }

            return new DiscordApiResponse<T>
            {
                Id = request.Id,
                StatusCode = httpResponse.StatusCode,
                Headers = httpResponse.Headers,
                Error = await httpResponse.Content.ReadAsStringAsync(cancellationToken),
                Data = default!
            };
        }
    }
}
