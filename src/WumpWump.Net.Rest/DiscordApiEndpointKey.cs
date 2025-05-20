using System.Net.Http;
using System.Text;

namespace WumpWump.Net.Rest
{
    public class DiscordApiEndpointKey
    {
        public HttpMethod Method { get; init; }
        public CompositeFormat Path { get; init; }
        public CompositeFormat Endpoint { get; init; }

        public DiscordApiEndpointKey(HttpMethod method, CompositeFormat path, CompositeFormat endpoint)
        {
            Method = method;
            Path = path;
            Endpoint = endpoint;
        }
    }
}
