using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;

namespace WumpWump.Net.Rest
{
    public record DiscordApiEndpointKey
    {
        public required HttpMethod Method { get; init; }
        public required CompositeFormat Path { get; init; }
        public required CompositeFormat Endpoint { get; init; }

        [SetsRequiredMembers]
        public DiscordApiEndpointKey(HttpMethod method, CompositeFormat path, CompositeFormat endpoint)
        {
            Method = method;
            Path = path;
            Endpoint = endpoint;
        }
    }
}
