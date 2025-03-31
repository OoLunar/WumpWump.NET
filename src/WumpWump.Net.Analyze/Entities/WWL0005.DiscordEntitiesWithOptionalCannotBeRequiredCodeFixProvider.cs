using System.Collections.Generic;
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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DiscordEntitiesWithOptionalCannotBeRequiredCodeFixProvider))]
public class DiscordEntitiesWithOptionalCannotBeRequiredCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => [DiscordEntitiesWithOptionalCannotBeRequiredAnalyzer.DiagnosticId];

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

        CodeAction codeAction = CodeAction.Create("Add required modifier", ct => RemoveRequiredModifierAsync(context.Document, propertyDecl, ct), "AddRequiredModifier");
        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private async Task<Document> RemoveRequiredModifierAsync(Document document, PropertyDeclarationSyntax propertyDecl, CancellationToken cancellationToken = default)
    {
        SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken);
        if (root is null)
        {
            return document;
        }

        IEnumerable<SyntaxToken> newModifiers = propertyDecl.Modifiers.Where(modifier => !modifier.IsKind(SyntaxKind.RequiredKeyword));

        PropertyDeclarationSyntax newProperty = propertyDecl.WithModifiers(new SyntaxTokenList(newModifiers));
        SyntaxNode newRoot = root.ReplaceNode(propertyDecl, newProperty);

        return document.WithSyntaxRoot(newRoot);
    }
}
