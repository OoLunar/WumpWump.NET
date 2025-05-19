using System.Buffers;
using OoLunar.AsyncEvents;

namespace WumpWump.Net.Gateway.Events.EventArgs
{
    public class DiscordWebsocketMessageEventArgs : AsyncEventArgs
    {
        public required DiscordGatewayClient Client { get; init; }
        public required ReadOnlySequence<byte> Message { get; init; }
    }
}