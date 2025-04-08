using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using WumpWump.Net.Analyze.Entities;

namespace WumpWump.Net.Analyze
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiscordEntitiesMustBeRecordsAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "WWL0002";
        private const string Category = "Design";

        private static readonly string Title = "Discord entities must be declared as records";
        private static readonly string MessageFormat = "Type '{0}' in '{1}' namespace must be declared as a record";
        private static readonly string Description = "Discord entities must be declared as records for easy cloning.";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Description,
            customTags: [WellKnownDiagnosticTags.CustomSeverityConfigurable]
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.StructDeclaration);
        }

        private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is not BaseTypeDeclarationSyntax typeDeclaration)
            {
                return;
            }

            INamedTypeSymbol? symbol = context.SemanticModel.GetDeclaredSymbol(typeDeclaration);
            if (symbol is null || symbol.IsRecord || symbol.IsStatic || !DiscordEntityUtilities.IsInRestEntityNamespace(symbol.ContainingNamespace))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, typeDeclaration.Identifier.GetLocation(), typeDeclaration.Identifier.Text, symbol.ContainingNamespace));
        }
    }
}
