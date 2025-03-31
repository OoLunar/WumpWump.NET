using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using WumpWump.Net.Analyze;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DiscordEntitiesRequirePropertiesCodeFixProvider))]
public class DiscordEntitiesRequirePropertiesCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => [DiscordEntitiesRequirePropertiesAnalyzer.DiagnosticId];

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root is null)
        {
            return;
        }

        Diagnostic diagnostic = context.Diagnostics[0];
        TextSpan span = diagnostic.Location.SourceSpan;

        PropertyDeclarationSyntax? propertyDecl = root.FindToken(span.Start).Parent?.AncestorsAndSelf().OfType<PropertyDeclarationSyntax>().FirstOrDefault();
        if (propertyDecl is null)
        {
            return;
        }

        CodeAction codeAction = CodeAction.Create("Add required modifier", ct => AddRequiredModifierAsync(context.Document, propertyDecl, ct), "AddRequiredModifier");
        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private async Task<Document> AddRequiredModifierAsync(Document document, PropertyDeclarationSyntax propertyDecl, CancellationToken cancellationToken = default)
    {
        SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken);
        if (root is null)
        {
            return document;
        }

        // Find the access modifier (public/private/etc.)
        SyntaxToken accessModifier = propertyDecl.Modifiers.FirstOrDefault(modifier => modifier.IsKind(SyntaxKind.PublicKeyword));

        // Create required token with no trailing space (will get space from access modifier)
        SyntaxToken requiredToken = SyntaxFactory.Token(SyntaxKind.RequiredKeyword).WithTrailingTrivia(SyntaxFactory.Space);

        SyntaxTokenList newModifiers;
        if (accessModifier != default)
        {
            // Case 1: Has access modifier - insert after it
            // Ensure access modifier has exactly one trailing space
            SyntaxToken fixedAccessModifier = accessModifier.WithTrailingTrivia(SyntaxFactory.Space).WithLeadingTrivia(new SyntaxTriviaList());

            // Replace original access modifier and insert required
            newModifiers = propertyDecl.Modifiers
                .Replace(accessModifier, fixedAccessModifier)
                .Insert(propertyDecl.Modifiers.IndexOf(accessModifier) + 1, requiredToken);
        }
        else
        {
            // Case 2: No access modifier - insert at start
            newModifiers = propertyDecl.Modifiers.Insert(0, requiredToken.WithLeadingTrivia(propertyDecl.GetLeadingTrivia()));
        }

        PropertyDeclarationSyntax newProperty = propertyDecl
            .WithModifiers(newModifiers)
            .WithLeadingTrivia(accessModifier != default ? propertyDecl.GetLeadingTrivia() : default);

        SyntaxNode newRoot = root.ReplaceNode(propertyDecl, newProperty);
        return document.WithSyntaxRoot(newRoot);
    }
}
