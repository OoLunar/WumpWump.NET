using System.Collections.Generic;

namespace WumpWump.Net.Gateway.Events
{
    /// <summary>
    /// This class keeps track of all the gateway event metadata.
    /// </summary>
    /// <remarks>
    /// Internally this is just a pseudo class for <see cref="List{DiscordGatewayEventTicket}"/>.
    /// </remarks>
    public class DiscordGatewayEventRegistration : List<DiscordGatewayEventTicket>;
}
