using System;
using System.Net;
using System.Net.Http.Headers;

namespace WumpWump.Net.Rest
{
    public readonly record struct DiscordApiResponse<T>
    {
        public required Ulid Id { get; init; }
        public required HttpHeaders Headers { get; init; }
        public required HttpStatusCode StatusCode { get; init; }
        public required string? Error { get; init; }
        public required T Data { get; init; }

        public bool IsSuccess => (int)StatusCode is >= 200 and <= 299;

        public void EnsureSuccess()
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException($"Request failed with status code {StatusCode}: {Error}");
            }
        }
    }
}
