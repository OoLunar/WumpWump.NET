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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DiscordEntitiesMustBeRecordsCodeFixProvider))]
public class DiscordEntitiesMustBeRecordsCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => [DiscordEntitiesMustBeRecordsAnalyzer.DiagnosticId];

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

        TypeDeclarationSyntax? classDecl = root.FindToken(span.Start).Parent?.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();
        if (classDecl is null)
        {
            return;
        }

        context.RegisterCodeFix(CodeAction.Create(
            "Convert to record",
            createChangedDocument: c => ConvertToRecordAsync(context.Document, classDecl, c),
            equivalenceKey: "ConvertToRecord"
        ), diagnostic);
    }

    private async Task<Document> ConvertToRecordAsync(Document document, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken = default)
    {
        SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken);
        if (root is null)
        {
            return document;
        }

        // Preserve the struct keyword if it exists
        TypeDeclarationSyntax recordDecl;
        if (typeDecl.IsKind(SyntaxKind.StructDeclaration))
        {
            // Add the record modifier to the struct
            recordDecl = typeDecl.AddModifiers(SyntaxFactory.Token(SyntaxKind.RecordKeyword));
        }
        else
        {
            // Create record with same members and modifiers
            recordDecl = SyntaxFactory.RecordDeclaration(
               attributeLists: typeDecl.AttributeLists,
               modifiers: typeDecl.Modifiers,
               keyword: SyntaxFactory.Token(SyntaxKind.RecordKeyword),
               identifier: typeDecl.Identifier,
               typeParameterList: typeDecl.TypeParameterList,
               parameterList: null, // No parameters for regular record
               baseList: typeDecl.BaseList,
               constraintClauses: typeDecl.ConstraintClauses,
               openBraceToken: typeDecl.OpenBraceToken,
               members: typeDecl.Members,
               closeBraceToken: typeDecl.CloseBraceToken,
               semicolonToken: typeDecl.SemicolonToken
           ).WithLeadingTrivia(typeDecl.GetLeadingTrivia());
        }

        SyntaxNode newRoot = root.ReplaceNode(typeDecl, recordDecl);
        return document.WithSyntaxRoot(newRoot);
    }
}
