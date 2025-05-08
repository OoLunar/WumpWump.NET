using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events;

namespace WumpWump.Net.Gateway.Payloads
{
    [DiscordGatewayEvent((DiscordGatewayOpCode)(-1), "Zombied")]
    public record DiscordGatewayZombiedPayload
    {
        public required ulong? LastSequenceReceived { get; init; }
        public required int MissedHeartbeats { get; init; }
    }
}
