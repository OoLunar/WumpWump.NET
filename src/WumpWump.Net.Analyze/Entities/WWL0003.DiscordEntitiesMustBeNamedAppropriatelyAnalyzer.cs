using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace WumpWump.Net.Analyze.Entities
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiscordEntitiesMustBeNamedAppropriatelyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "WWL0003";
        private const string Category = "Design";

        private static readonly string Title = "Discord entities must be prefixed with their proper Discord namespace";
        private static readonly string Description = "All Discord entities must be prefixed with their proper Discord namespace for clarity.";
        private static readonly string MessageFormat = "Type '{0}' in namespace '{1}' should be called '{2}'";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Error, // Make this an error for enforcement
            isEnabledByDefault: true,
            description: Description,
            customTags: [WellKnownDiagnosticTags.CustomSeverityConfigurable]
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeTypeDeclaration, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeTypeDeclaration, SyntaxKind.StructDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeTypeDeclaration, SyntaxKind.RecordDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeTypeDeclaration, SyntaxKind.RecordStructDeclaration);
        }

        private static void AnalyzeTypeDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is not BaseTypeDeclarationSyntax typeDeclaration)
            {
                return;
            }

            INamedTypeSymbol? symbol = context.SemanticModel.GetDeclaredSymbol(typeDeclaration);
            if (symbol is null || !DiscordEntityUtilities.IsInEntityNamespace(symbol.ContainingNamespace) || symbol.IsStatic)
            {
                return;
            }

            string? expectedPrefix = GetExpectedPrefix(symbol.ContainingNamespace);
            if (expectedPrefix is null || !TryGetExpectedPrefix(expectedPrefix.AsSpan(), typeDeclaration.Identifier.Text, out string? newName))
            {
                // The name is already correct
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, typeDeclaration.Identifier.GetLocation(), typeDeclaration.Identifier.Text, symbol.ContainingNamespace, newName));
        }

        protected internal static string? GetExpectedPrefix(INamespaceSymbol namespaceSymbol)
        {
            if (DiscordEntityUtilities.IsInRestEntityNamespace(namespaceSymbol))
            {
                return "Discord";
            }
            else if (DiscordEntityUtilities.IsInGatewayEntityNamespace(namespaceSymbol))
            {
                return "DiscordGateway";
            }

            // There's a new Discord entity namespace that's unimplemented.
            return null;
        }

        protected internal static bool TryGetExpectedPrefix(ReadOnlySpan<char> prefix, string input, out string? newName)
        {
            // Check if the name starts with the letters of Discord in any order and finish the missing letters
            // I.E 'DiscUser' -> 'DiscordUser'
            Span<char> name = stackalloc char[input.Length];
            input.AsSpan().CopyTo(name);
            int index = 0;
            for (int i = 0; i < prefix.Length && i < name.Length; i++)
            {
                if (char.ToLower(name[i]) == char.ToLower(prefix[index]))
                {
                    index++;

                    // Also fix the capitalization just in case
                    name[i] = index == 1 ? char.ToUpper(name[i]) : char.ToLower(name[i]);
                }
            }

            if (index == prefix.Length)
            {
                // The name is already correct
                newName = null;
                return false;
            }

            // The name is not correct, we need to fix it
            newName = $"{prefix.ToString()}{name.Slice(index).ToString()}";
            return true;
        }
    }
}
