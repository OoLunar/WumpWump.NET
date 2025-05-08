using System;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events.EventArgs;

namespace WumpWump.Net.Gateway.Events
{
    public record DiscordGatewayEventTicket
    {
        public DiscordGatewayOpCode OpCode { get; init; }
        public string? EventName { get; init; }
        public Type EventType { get; init; }
        public DiscordGatewayEventArgsFactory CreateGatewayEventArgs { get; init; }

        // If you're using this constructor, you hopefully know what you're doing
        protected DiscordGatewayEventTicket()
        {
            EventType = null!;
            CreateGatewayEventArgs = null!;
        }

        public DiscordGatewayEventTicket(DiscordGatewayOpCode opCode, string? eventName, Type payloadType)
        {
            OpCode = opCode;
            EventName = eventName;
            EventType = payloadType;
            CreateGatewayEventArgs = CreateGatewayEventArgsFunction;
        }

        protected static DiscordGatewayAsyncEventArgs CreateGatewayEventArgsFunction(DiscordGatewayClient client, IDiscordGatewayPayload payload) => new()
        {
            Client = client,
            Payload = payload
        };
    }
}
