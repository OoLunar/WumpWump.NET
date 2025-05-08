using System;
using WumpWump.Net.Gateway.Entities;

namespace WumpWump.Net.Gateway.Events
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
    public class DiscordGatewayEventAttribute : Attribute
    {
        public DiscordGatewayOpCode OpCode { get; init; }
        public string? EventName { get; init; }

        public DiscordGatewayEventAttribute(DiscordGatewayOpCode opCode, string? eventName)
        {
            OpCode = opCode;
            EventName = eventName?.ToUpperInvariant();
        }
    }
}
