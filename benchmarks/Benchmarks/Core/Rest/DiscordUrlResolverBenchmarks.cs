using System.Net.Http;
using System.Text;
using BenchmarkDotNet.Attributes;
using WumpWump.Net.Rest;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Benchmarks
{
    public class DiscordUrlResolverBenchmarks
    {
        private readonly DiscordUrlResolver _resolver;
        private readonly DiscordApiEndpointKey _gatewayBotEndpointKey;
        private readonly DiscordApiEndpointKey _endpointKey;
        private readonly DiscordApiEndpoint _endpoint;

        public DiscordUrlResolverBenchmarks()
        {
            _resolver = new DiscordUrlResolver();
            _gatewayBotEndpointKey = DiscordApiRoutes.GetGatewayBot;
            _endpointKey = new DiscordApiEndpointKey(HttpMethod.Get, CompositeFormat.Parse("/channels/{0}/messages/{1}"), CompositeFormat.Parse("/channels/{0}/messages/{1}"));
            _endpoint = _resolver.GetEndpoint(_gatewayBotEndpointKey);
        }

        [Benchmark]
        public string GetBaseUrl() => _resolver.GetBaseUrl();

        [Benchmark]
        public DiscordApiEndpoint GetEndpoint_NoArgs() => _resolver.GetEndpoint(_gatewayBotEndpointKey);

        [Benchmark]
        // Using a hypothetical endpoint that takes arguments
        public DiscordApiEndpoint GetEndpoint_WithArgs() => _resolver.GetEndpoint(_endpointKey, 123456789, 987654321);

        [Benchmark]
        public DiscordApiEndpoint GetEndpoint_WithQueryParams() => (_endpoint with { })
            .WithQueryParameter("limit", new DiscordOptional<int>(10))
            .WithQueryParameter("sort", new DiscordOptional<string>("desc"))
            .WithQueryParameter("filter", new DiscordOptional<string>("recent"));

        [Benchmark]
        public DiscordApiEndpoint GetEndpoint_WithMultipleQueryParams() => (_endpoint with { }).WithQueryParameters(
            ("limit", new DiscordOptional<int>(10)),
            ("sort", new DiscordOptional<string>("desc")),
            ("filter", new DiscordOptional<string>("recent"))
        );
    }
}