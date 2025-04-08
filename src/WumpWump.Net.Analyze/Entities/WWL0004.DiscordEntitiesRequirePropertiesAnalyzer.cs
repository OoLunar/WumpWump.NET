using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using WumpWump.Net.Analyze.Entities;

namespace WumpWump.Net.Analyze
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiscordEntitiesRequirePropertiesAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "WWL0004";
        private const string Category = "Design";

        private static readonly string Title = "Discord entities must mark their properties as required";
        private static readonly string MessageFormat = "The Discord entity property '{0}' must be marked as required";
        private static readonly string Description = "All properties in Discord entities - except of type DiscordOptional<T> - must be marked as required.";

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
            context.RegisterSyntaxNodeAction(AnalyzeProperty, SyntaxKind.PropertyDeclaration);
        }

        private void AnalyzeProperty(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is not PropertyDeclarationSyntax propertyDecl)
            {
                return;
            }

            // If the property isn't found, isn't public, is static, is required, is a DiscordOptional<T>, or has an expression body, skip it
            // Has a constructor
            IPropertySymbol? propertySymbol = context.SemanticModel.GetDeclaredSymbol(propertyDecl);
            if (propertySymbol is null
                || !DiscordEntityUtilities.IsInEntityNamespace(propertySymbol.ContainingNamespace)
                || propertySymbol.DeclaredAccessibility != Accessibility.Public
                || propertySymbol.IsStatic
                || propertySymbol.IsRequired
                || DiscordEntityUtilities.IsDiscordOptional(propertySymbol.Type)
                || propertySymbol.ContainingType.IsAbstract
                || propertySymbol.ContainingType.Constructors.Length > 0)
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, propertyDecl.Identifier.GetLocation(), propertyDecl.Identifier.Text));
        }
    }
}
