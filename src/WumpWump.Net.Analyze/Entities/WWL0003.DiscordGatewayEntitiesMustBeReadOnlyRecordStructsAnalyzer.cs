using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace WumpWump.Net.Analyze.Entities
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiscordGatewayEntitiesMustBeReadOnlyRecordStructsAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "WWL0003";
        private const string Category = "Design";

        private static readonly string Title = "Discord gateway entities must be declared as readonly record structs";
        private static readonly string Description = "All Discord gateway entities should be readonly record structs because they are short lived and immutable.";
        private static readonly string MessageFormat = "Type '{0}' in '{1}' namespace must be declared as a readonly record struct";

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
            if (symbol is null || !DiscordEntityUtilities.IsInGatewayEntityNamespace(symbol.ContainingNamespace) || symbol.IsStatic || (symbol.IsReadOnly && symbol.IsRecord && symbol.IsValueType))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, typeDeclaration.Identifier.GetLocation(), typeDeclaration.Identifier.Text, symbol.ContainingNamespace));
        }
    }
}