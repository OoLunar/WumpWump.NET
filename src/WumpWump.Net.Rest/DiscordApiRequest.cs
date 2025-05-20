using System;
using System.Collections.Generic;

namespace WumpWump.Net.Rest
{
    public record DiscordApiRequest
    {
        public required DiscordApiEndpoint Endpoint { get; init; }

        public Ulid Id { get; init; } = Ulid.NewUlid();
        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; init; } = new Dictionary<string, IEnumerable<string>>();
        public object? Body { get; init; }
    }
}
