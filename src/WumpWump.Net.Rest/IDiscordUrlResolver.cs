using System;

namespace WumpWump.Net.Rest
{
    public interface IDiscordUrlResolver
    {
        string GetDomain();
        string GetApiPath();
        string GetApiVersion();
        string GetBaseUrl();
        DiscordApiEndpoint GetEndpoint(DiscordApiEndpointKey key, params ReadOnlySpan<object?> arguments);
    }
}
