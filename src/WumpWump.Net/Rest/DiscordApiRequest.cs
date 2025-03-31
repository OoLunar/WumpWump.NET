using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WumpWump.Net.Rest
{
    public record DiscordApiRequest
    {
        public required Uri Url { get; init; }
        public required Uri Route { get; init; }
        public required HttpMethod Method { get; init; }

        public Ulid Id { get; init; } = Ulid.NewUlid();
        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; init; } = new Dictionary<string, IEnumerable<string>>();
        public object? Body { get; init; }
    }
}
