using System.Collections.Generic;

namespace WumpWump.Net.Gateway.Entities.Payloads
{
    /// <inheritdoc />
    public record DiscordGatewayPayloadComparer : IComparer<IDiscordGatewayPayload>
    {
        /// <inheritdoc />
        public int Compare(IDiscordGatewayPayload? x, IDiscordGatewayPayload? y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null)
            {
                return 1;
            }
            else if (y == null)
            {
                return -1;
            }

            bool xIsHeartbeat = x.OpCode is DiscordGatewayOpCode.Heartbeat;
            bool yIsHeartbeat = y.OpCode is DiscordGatewayOpCode.Heartbeat;

            // First compare by heartbeat status (heartbeats come first)
            if (xIsHeartbeat && !yIsHeartbeat)
            {
                return -1;
            }
            else if (!xIsHeartbeat && yIsHeartbeat)
            {
                return 1;
            }

            // If both are heartbeats or both are not heartbeats, compare by sequence number
            // Null sequence numbers are treated as being higher priority than non-null (sorted first)
            if (x.Sequence == null && y.Sequence != null)
            {
                return -1;
            }
            else if (x.Sequence != null && y.Sequence == null)
            {
                return 1;
            }
            else if (x.Sequence != null && y.Sequence != null)
            {
                return x.Sequence.Value.CompareTo(y.Sequence.Value);
            }

            return 0;
        }
    }
}
