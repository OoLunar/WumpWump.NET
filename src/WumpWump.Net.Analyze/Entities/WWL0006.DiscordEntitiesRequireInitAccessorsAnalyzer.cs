using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using WumpWump.Net.Analyze.Entities;

namespace WumpWump.Net.Analyze
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiscordEntitiesRequireInitAccessorsAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "WWL0006";
        private const string Category = "Design";

        private static readonly string Title = "Discord entity properties must use init accessors";
        private static readonly string MessageFormat = "The Discord entity property '{0}' must use a public init accessor";
        private static readonly string Description = "All Discord entity properties must use public init accessors. We enforce immutability on our entities and encourage users to modify the data through cloning and updating through whichever approach is most appropriate.";

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
                || propertyDecl.ExpressionBody is not null
                || propertySymbol.SetMethod?.IsInitOnly is not true
                || propertySymbol.SetMethod?.DeclaredAccessibility != Accessibility.NotApplicable
                || propertySymbol.ContainingType.InstanceConstructors.Length == 0)
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, propertyDecl.Identifier.GetLocation(), propertyDecl.Identifier.Text));
        }
    }
}
