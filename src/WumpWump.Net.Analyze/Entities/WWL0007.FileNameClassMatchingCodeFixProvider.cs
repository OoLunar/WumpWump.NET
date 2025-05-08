using System.Collections.Immutable;
using System.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace WumpWump.Net.Analyze.Entities
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(FileNameClassMatchingCodeFixProvider)), Shared]
    public class FileNameClassMatchingCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => [DiscordEntitiesRequireInitAccessorsAnalyzer.DiagnosticId];

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root is null)
            {
                return;
            }

            Diagnostic diagnostic = context.Diagnostics.First();
            TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

            BaseTypeDeclarationSyntax? typeDecl = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf()
                .OfType<BaseTypeDeclarationSyntax>().FirstOrDefault();

            if (typeDecl is null)
            {
                return;
            }

            context.RegisterCodeFix(CodeAction.Create(
                title: "Rename file to match class name",
                createChangedSolution: c => RenameFileToMatchClassNameAsync(context.Document, typeDecl, c),
                equivalenceKey: "RenameFileToMatchClassName"
            ), diagnostic);
        }

        private async Task<Solution> RenameFileToMatchClassNameAsync(Document document, BaseTypeDeclarationSyntax typeDecl, CancellationToken cancellationToken = default)
        {
            SemanticModel? semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            if (semanticModel is null)
            {
                return document.Project.Solution;
            }

            INamedTypeSymbol? typeSymbol = semanticModel.GetDeclaredSymbol(typeDecl, cancellationToken);
            if (typeSymbol is null)
            {
                return document.Project.Solution;
            }

            string? expectedFileName = FileNameClassMatchingAnalyzer.GetExpectedFileName(typeSymbol);
            if (expectedFileName is null)
            {
                // Skip nested types
                return document.Project.Solution;
            }

            Solution solution = document.Project.Solution;
            DocumentId documentId = document.Id;
            string? filePath = document.FilePath;
            string directory = Path.GetDirectoryName(filePath);
            string newFilePath = Path.Combine(directory, expectedFileName + ".cs");

            // Handle case where file already exists
            if (document.Project.Documents.Any(otherDoc => Path.GetFullPath(otherDoc.FilePath) == Path.GetFullPath(newFilePath)))
            {
                // If the file already exists, skip the rename.
                // The user will be frustrated enough that the error
                // isn't going away, eventually they'll see the already
                // existing file and feel stupid. "Oh that's why"
                return solution;
            }

            return solution.WithDocumentFilePath(documentId, newFilePath);
        }
    }
}