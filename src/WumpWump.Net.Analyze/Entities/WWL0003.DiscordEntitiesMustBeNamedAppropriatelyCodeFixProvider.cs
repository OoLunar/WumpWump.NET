using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Text;

namespace WumpWump.Net.Analyze.Entities
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DiscordEntitiesMustBeNamedAppropriatelyCodeFixProvider)), Shared]
    public class DiscordEntitiesMustBeNamedAppropriatelyCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => [DiscordEntitiesMustBeNamedAppropriatelyAnalyzer.DiagnosticId];

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root is null)
            {
                return;
            }

            Diagnostic diagnostic = context.Diagnostics[0];
            TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

            BaseTypeDeclarationSyntax? declaration = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<BaseTypeDeclarationSyntax>().FirstOrDefault();
            if (declaration is null)
            {
                return;
            }

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Fixing entity naming",
                    createChangedDocument: ct => FixEntityNamingAsync(context.Document, declaration, ct),
                    equivalenceKey: "FixEntityNaming"),
                diagnostic);
        }

        private async Task<Document> FixEntityNamingAsync(Document document, BaseTypeDeclarationSyntax typeDecl, CancellationToken cancellationToken = default)
        {
            DocumentEditor editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

            SemanticModel? semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            INamespaceSymbol? containingNamespaceSymbol = semanticModel?.GetDeclaredSymbol(typeDecl)?.ContainingNamespace;
            if (containingNamespaceSymbol is null)
            {
                return document;
            }

            string? expectedPrefix = DiscordEntitiesMustBeNamedAppropriatelyAnalyzer.GetExpectedPrefix(containingNamespaceSymbol);
            if (expectedPrefix is null || !DiscordEntitiesMustBeNamedAppropriatelyAnalyzer.TryGetExpectedPrefix(expectedPrefix.AsSpan(), typeDecl.Identifier.Text, out string? newName))
            {
                return document;
            }

            SyntaxNode newTypeDecl = typeDecl.WithIdentifier(SyntaxFactory.Identifier(newName!));
            editor.ReplaceNode(typeDecl, newTypeDecl);
            return editor.GetChangedDocument();
        }
    }
}
