using System;
using Microsoft.CodeAnalysis;

namespace WumpWump.Net.Analyze.Entities
{
    public static class DiscordEntityUtilities
    {
        private const string EntityNamespace = "WumpWump.Net.Entities";
        private const string GatewayCommandsNamespace = "WumpWump.Net.Gateway.Commands";
        private const string GatewayEntityNamespace = "WumpWump.Net.Gateway.Entities";
        private const string GatewayPayloadNamespace = "WumpWump.Net.Gateway.Payloads";

        private const string DiscordIOptionalType = "WumpWump.Net.Entities.IDiscordOptional";
        private const string DiscordOptionalType = "WumpWump.Net.Entities.DiscordOptional";

        public static bool IsInEntityNamespace(INamespaceSymbol symbol) => IsInRestEntityNamespace(symbol) || IsInGatewayEntityNamespace(symbol);

        public static bool IsInRestEntityNamespace(INamespaceSymbol symbol)
        {
            string containingNamespace = symbol.ToDisplayString();
            return containingNamespace.StartsWith(EntityNamespace, StringComparison.Ordinal);
        }

        public static bool IsInGatewayEntityNamespace(INamespaceSymbol symbol)
        {
            string containingNamespace = symbol.ToDisplayString();
            return containingNamespace.StartsWith(GatewayCommandsNamespace, StringComparison.Ordinal)
                || containingNamespace.StartsWith(GatewayEntityNamespace, StringComparison.Ordinal)
                || containingNamespace.StartsWith(GatewayPayloadNamespace, StringComparison.Ordinal);
        }

        public static bool IsDiscordOptional(ITypeSymbol typeSymbol)
        {
            string typeDisplayString = typeSymbol.ToDisplayString();
            return typeDisplayString.StartsWith(DiscordOptionalType, StringComparison.Ordinal) || typeDisplayString.StartsWith(DiscordIOptionalType, StringComparison.Ordinal);
        }
    }
}
