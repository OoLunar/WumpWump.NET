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

namespace WumpWump.Net.Analyze
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PrivateToProtectedMemberCodeFixProvider))]
    public class PrivateToProtectedMemberCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => [PrivateToProtectedMemberAnalyzer.DiagnosticId];

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
            if (root is null)
            {
                return;
            }

            Diagnostic diagnostic = context.Diagnostics[0];
            TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;
            MemberDeclarationSyntax? member = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();
            if (member is null)
            {
                return;
            }

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Change to protected",
                    createChangedDocument: c => ChangeToProtectedAsync(context.Document, member, c),
                    equivalenceKey: "ChangeToProtected"),
                diagnostic);
        }

        private async Task<Document> ChangeToProtectedAsync(Document document, MemberDeclarationSyntax member, CancellationToken cancellationToken = default)
        {
            SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken);
            if (root is null)
            {
                return document;
            }

            // Get the private modifier and its trivia
            SyntaxToken privateToken = member.Modifiers.First(m => m.IsKind(SyntaxKind.PrivateKeyword));

            // Create protected token with same trivia
            SyntaxToken protectedToken = SyntaxFactory.Token(SyntaxKind.ProtectedKeyword)
                .WithLeadingTrivia(privateToken.LeadingTrivia)
                .WithTrailingTrivia(privateToken.TrailingTrivia);

            // Replace private with protected while maintaining other modifiers
            SyntaxTokenList newModifiers = new(member.Modifiers.Select(m => m.IsKind(SyntaxKind.PrivateKeyword) ? protectedToken : m));

            // Create new member with original formatting
            MemberDeclarationSyntax newMember = member
                .WithModifiers(newModifiers)
                .WithLeadingTrivia(member.GetLeadingTrivia());

            SyntaxNode newRoot = root.ReplaceNode(member, newMember);
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
