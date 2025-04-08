using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace WumpWump.Net.Analyze
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PrivateToProtectedMemberAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "WWL0001";
        private const string Category = "Design";

        private static readonly string Title = "Unsealed classes should use protected members instead of private";
        private static readonly string MessageFormat = "Member '{0}' in unsealed class should be protected rather than private";
        private static readonly string Description = "To properly support inheritance, unsealed classes should use protected visibility for members.";

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
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.StructDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.RecordDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.RecordStructDeclaration);
        }

        private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is not TypeDeclarationSyntax typeDeclaration)
            {
                return;
            }

            // Skip if the class is effectively sealed (static or private in a sealed parent)
            INamedTypeSymbol? symbol = context.SemanticModel.GetDeclaredSymbol(typeDeclaration);
            if (symbol is null || symbol.IsSealed || symbol.IsStatic || HasSealedParents(symbol))
            {
                return;
            }

            // Check all members in the class
            foreach (MemberDeclarationSyntax member in typeDeclaration.Members)
            {
                // Skip non-private members
                SyntaxToken privateKeyword = member.Modifiers.FirstOrDefault(modifier => modifier.IsKind(SyntaxKind.PrivateKeyword));
                if (privateKeyword.IsKind(SyntaxKind.None))
                {
                    continue;
                }

                // Skip members that can't be protected (explicit interface implementations etc.)
                if (!CanBeProtected(member))
                {
                    continue;
                }

                // Get the member's identifier
                SyntaxToken identifier = member switch
                {
                    MethodDeclarationSyntax method => method.Identifier,
                    PropertyDeclarationSyntax property => property.Identifier,
                    EventDeclarationSyntax eventDecl => eventDecl.Identifier,
                    FieldDeclarationSyntax field => field.Declaration.Variables.FirstOrDefault()?.Identifier ?? default,
                    EventFieldDeclarationSyntax eventField => eventField.Declaration.Variables.FirstOrDefault()?.Identifier ?? default,
                    _ => default
                };

                Diagnostic diagnostic = Diagnostic.Create(Rule, identifier.GetLocation(), member.GetName());
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static bool HasSealedParents(INamedTypeSymbol symbol)
        {
            INamedTypeSymbol? parent = symbol.BaseType;
            while (parent is not null)
            {
                if (parent.IsSealed)
                {
                    return true;
                }

                parent = parent.BaseType;
            }

            return false;
        }

        private static bool CanBeProtected(MemberDeclarationSyntax member) => member switch
        {
            // These member types can't be protected
            InterfaceDeclarationSyntax => false,
            EnumDeclarationSyntax => false,
            DelegateDeclarationSyntax => false,
            FieldDeclarationSyntax => true,
            EventFieldDeclarationSyntax => true,
            MethodDeclarationSyntax methodMember => !IsExplicitInterfaceImplementation(methodMember),
            PropertyDeclarationSyntax propertyMember => !IsExplicitInterfaceImplementation(propertyMember),
            EventDeclarationSyntax eventMember => !IsExplicitInterfaceImplementation(eventMember),
            _ => true
        };

        private static bool IsExplicitInterfaceImplementation(MethodDeclarationSyntax method) => method.ExplicitInterfaceSpecifier is not null;
        private static bool IsExplicitInterfaceImplementation(PropertyDeclarationSyntax property) => property.ExplicitInterfaceSpecifier is not null;
        private static bool IsExplicitInterfaceImplementation(EventDeclarationSyntax eventDecl) => eventDecl.ExplicitInterfaceSpecifier is not null;
    }
}
