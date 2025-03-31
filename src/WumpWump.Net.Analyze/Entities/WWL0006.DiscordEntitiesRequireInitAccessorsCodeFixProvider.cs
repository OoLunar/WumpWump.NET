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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DiscordEntitiesRequireInitAccessorsCodeFixProvider))]
public class DiscordEntitiesRequireInitAccessorsCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => [DiscordEntitiesRequireInitAccessorsAnalyzer.DiagnosticId];

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

        CodeAction codeAction = CodeAction.Create("Add required modifier", ct => AddInitAccessorAsync(context.Document, propertyDecl, ct), "AddRequiredModifier");
        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private async Task<Document> AddInitAccessorAsync(Document document, PropertyDeclarationSyntax propertyDecl, CancellationToken cancellationToken)
    {
        SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken);
        if (root == null)
        {
            return document;
        }

        // Handle auto-properties without accessor list
        SyntaxNode newRoot;
        PropertyDeclarationSyntax newProperty;
        if (propertyDecl.AccessorList == null)
        {
            SyntaxToken initToken = SyntaxFactory.Token(SyntaxKind.InitKeyword).WithTrailingTrivia(SyntaxFactory.Space);

            SyntaxTokenList newModifiers = propertyDecl.Modifiers.Add(initToken);
            newProperty = propertyDecl.WithModifiers(newModifiers);
            newRoot = root.ReplaceNode(propertyDecl, newProperty);

            return document.WithSyntaxRoot(newRoot);
        }

        // Handle properties with accessor list
        AccessorDeclarationSyntax initAccessor = SyntaxFactory.AccessorDeclaration(SyntaxKind.InitAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

        AccessorListSyntax newAccessors = propertyDecl.AccessorList.AddAccessors(initAccessor);
        newProperty = propertyDecl.WithAccessorList(newAccessors);
        newRoot = root.ReplaceNode(propertyDecl, newProperty);

        return document.WithSyntaxRoot(newRoot);
    }
}
