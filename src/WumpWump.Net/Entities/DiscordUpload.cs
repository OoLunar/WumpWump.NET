using System;
using System.IO;

namespace WumpWump.Net.Entities
{
    public record DiscordUpload
    {
        public Stream Data { get; init; }

        public DiscordUpload(Stream data) => Data = data;
        public DiscordUpload(ReadOnlySpan<byte> data) => Data = new MemoryStream(data.ToArray(), false);

        public static implicit operator DiscordUpload(Stream stream) => new(stream);
        public static implicit operator DiscordUpload(ReadOnlySpan<byte> span) => new(span);
    }
}