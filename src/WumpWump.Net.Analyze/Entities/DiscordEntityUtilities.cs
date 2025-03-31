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

        public static bool IsInEntityNamespace(ISymbol symbol) => IsInRestEntityNamespace(symbol) || IsInGatewayEntityNamespace(symbol);

        public static bool IsInRestEntityNamespace(ISymbol symbol)
        {
            string containingNamespace = symbol.ContainingNamespace.ToDisplayString();
            return containingNamespace.StartsWith(EntityNamespace);
        }

        public static bool IsInGatewayEntityNamespace(ISymbol symbol)
        {
            string containingNamespace = symbol.ContainingNamespace.ToDisplayString();
            return containingNamespace.StartsWith(GatewayCommandsNamespace)
                || containingNamespace.StartsWith(GatewayEntityNamespace)
                || containingNamespace.StartsWith(GatewayPayloadNamespace);
        }

        public static bool IsDiscordOptional(ITypeSymbol typeSymbol)
        {
            string typeDisplayString = typeSymbol.ToDisplayString();
            return typeDisplayString.StartsWith(DiscordOptionalType) || typeDisplayString.StartsWith(DiscordIOptionalType);
        }
    }
}
