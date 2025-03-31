using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using WumpWump.Net.Analyze.Entities;

namespace WumpWump.Net.Analyze
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiscordEntitiesWithOptionalCannotBeRequiredAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "WWL0005";
        private const string Category = "Design";

        private static readonly string Title = "Discord entities with optional properties cannot be required";
        private static readonly string MessageFormat = "The Discord entity with DiscordOptional<T> property '{0}' cannot be marked as required";
        private static readonly string Description = "All properties in Discord entities - except of type DiscordOptional<T> - must be marked as required.";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            customTags: [WellKnownDiagnosticTags.CustomSeverityConfigurable]
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeProperty, SyntaxKind.PropertyDeclaration);
        }

        private void AnalyzeProperty(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is not PropertyDeclarationSyntax propertyDecl)
            {
                return;
            }

            // If the property isn't found, isn't public, is static, isn't required, isn't a DiscordOptional<T>, or has an expression body, skip it
            IPropertySymbol? propertySymbol = context.SemanticModel.GetDeclaredSymbol(propertyDecl);
            if (propertySymbol is null
                || !DiscordEntityUtilities.IsInEntityNamespace(propertySymbol.ContainingNamespace)
                || propertySymbol.DeclaredAccessibility != Accessibility.Public
                || propertySymbol.IsStatic
                || !propertySymbol.IsRequired
                || !DiscordEntityUtilities.IsDiscordOptional(propertySymbol.Type)
                || propertyDecl.ExpressionBody is not null)
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, propertyDecl.Identifier.GetLocation(), propertyDecl.Identifier.Text));
        }
    }
}
