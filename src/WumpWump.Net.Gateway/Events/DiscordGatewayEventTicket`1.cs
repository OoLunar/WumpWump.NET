using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events.EventArgs;

namespace WumpWump.Net.Gateway.Events
{
    public record DiscordGatewayEventTicket<TPayload> : DiscordGatewayEventTicket
    {
        public DiscordGatewayEventTicket(DiscordGatewayOpCode opCode, string? eventName)
        {
            OpCode = opCode;
            EventName = eventName;
            EventType = typeof(TPayload);
            CreateGatewayEventArgs = CreateGatewayEventArgsGenericFunction;
        }

        protected static DiscordGatewayAsyncEventArgs CreateGatewayEventArgsGenericFunction(DiscordGatewayClient client, IDiscordGatewayPayload payload) => new DiscordGatewayAsyncEventArgs<TPayload>()
        {
            Client = client,
            Payload = (IDiscordGatewayPayload<TPayload>)payload
        };
    }
}
