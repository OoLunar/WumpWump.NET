using WumpWump.Net.Gateway.Events;

namespace WumpWump.Net.Gateway.Entities.Payloads
{
    [DiscordGatewayEvent((DiscordGatewayOpCode)(-1), "Zombied")]
    public record DiscordGatewayZombiedPayload
    {
        public required ulong? LastSequenceReceived { get; init; }
        public required int MissedHeartbeats { get; init; }
    }
}
