using System.Collections.Generic;
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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DiscordGatewayEntitiesMustBeReadOnlyRecordStructsCodeFixProvider)), Shared]
    public class DiscordGatewayEntitiesMustBeReadOnlyRecordStructsCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => [DiscordGatewayEntitiesMustBeReadOnlyRecordStructsAnalyzer.DiagnosticId];

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
                    title: "Convert to readonly record struct",
                    createChangedDocument: ct => ConvertToReadOnlyRecordStructAsync(context.Document, declaration, ct),
                    equivalenceKey: "ConvertToReadOnlyRecordStruct"),
                diagnostic);
        }

        private async Task<Document> ConvertToReadOnlyRecordStructAsync(Document document, BaseTypeDeclarationSyntax typeDecl, CancellationToken cancellationToken = default)
        {
            DocumentEditor editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
            SyntaxGenerator generator = editor.Generator;

            // Exclude the below modifiers from the list of modifiers to keep
            SyntaxKind[] ignoredModifiers = [SyntaxKind.ClassKeyword, SyntaxKind.StructKeyword, SyntaxKind.ReadOnlyKeyword, SyntaxKind.RecordKeyword, SyntaxKind.SealedKeyword, SyntaxKind.AbstractKeyword];
            List<SyntaxToken> modifiers = typeDecl.Modifiers
                .Where(modifier => !ignoredModifiers.Contains(modifier.Kind()))
                .ToList();

            // Find the index to insert 'readonly' and 'record' (after access modifiers)
            int insertPosition = 0;
            SyntaxKind[] accessModifiers = [SyntaxKind.PublicKeyword, SyntaxKind.PrivateKeyword, SyntaxKind.InternalKeyword, SyntaxKind.ProtectedKeyword];

            for (int i = 0; i < modifiers.Count; i++)
            {
                if (accessModifiers.Contains(modifiers[i].Kind()))
                {
                    insertPosition = i + 1;
                }
            }

            // Insert in correct order: 'readonly' first, then 'record'
            modifiers.Insert(insertPosition, SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));
            modifiers.Insert(insertPosition + 1, SyntaxFactory.Token(SyntaxKind.RecordKeyword));

            // Create the new type declaration
            BaseTypeDeclarationSyntax newDeclaration = typeDecl switch
            {
                ClassDeclarationSyntax classDecl => SyntaxFactory.StructDeclaration(
                    attributeLists: classDecl.AttributeLists,
                    modifiers: new SyntaxTokenList(modifiers),
                    keyword: SyntaxFactory.Token(SyntaxKind.StructKeyword),
                    identifier: classDecl.Identifier,
                    typeParameterList: classDecl.TypeParameterList,
                    parameterList: null, // No primary constructor for converted classes
                    baseList: classDecl.BaseList,
                    constraintClauses: classDecl.ConstraintClauses,
                    openBraceToken: classDecl.OpenBraceToken,
                    members: classDecl.Members,
                    closeBraceToken: classDecl.CloseBraceToken,
                    semicolonToken: classDecl.SemicolonToken),
                StructDeclarationSyntax structDecl => structDecl.WithModifiers(new SyntaxTokenList(modifiers)).WithKeyword(SyntaxFactory.Token(SyntaxKind.StructKeyword)),
                _ => typeDecl.WithModifiers(new SyntaxTokenList(modifiers))
            };

            editor.ReplaceNode(typeDecl, newDeclaration);
            return editor.GetChangedDocument();
        }
    }
}