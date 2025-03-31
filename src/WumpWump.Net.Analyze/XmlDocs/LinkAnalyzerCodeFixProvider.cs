using System.Collections.Immutable;
using System.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace WumpWump.Net.Analyze
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(LinkCodeFixProvider)), Shared]
    public class LinkCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => [LinkAnalyzer.DiagnosticId];
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

                // Find the XML element that triggered the diagnostic
                XmlElementSyntax? element = root?.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<XmlElementSyntax>().FirstOrDefault();
                if (element != null)
                {
                    context.RegisterCodeFix(
                        CodeAction.Create(
                            title: "Convert to anchor tag",
                            createChangedDocument: c => ConvertSeeToAnchorAsync(context.Document, element, c),
                            equivalenceKey: "ConvertToAnchorTag"),
                        diagnostic);
                }
            }
        }

        private async Task<Document> ConvertSeeToAnchorAsync(Document document, XmlElementSyntax seeElement, CancellationToken cancellationToken)
        {
            // Get the original text of the see element
            string originalText = seeElement.ToString();

            // Parse the XML content
            (string openingTag, string content, string closingTag) = ParseXmlElement(originalText);

            // Create new anchor tags
            string newOpeningTag = openingTag.Replace("<see ", "<a ").Replace("<see>", "<a>");
            string newClosingTag = closingTag?.Replace("</see>", "</a>") ?? "";

            // Reconstruct the element
            string newText = newOpeningTag + content + newClosingTag;

            // Parse as XML element syntax
            XmlElementSyntax newElement = SyntaxFactory.XmlElement(
                SyntaxFactory.XmlElementStartTag(
                    SyntaxFactory.XmlName("a"),
                    ParseXmlAttributes(openingTag)),
                new SyntaxList<XmlNodeSyntax>(content.Split('\n').Select(SyntaxFactory.XmlText)),
                SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName("a")));

            // Replace in document
            SyntaxNode? root = await document.GetSyntaxRootAsync(cancellationToken);
            if (root == null)
            {
                return document;
            }

            SyntaxNode newRoot = root.ReplaceNode(seeElement, newElement);
            return document.WithSyntaxRoot(newRoot);
        }

        private (string openingTag, string content, string closingTag) ParseXmlElement(string xmlText)
        {
            using XmlReader reader = XmlReader.Create(new StringReader(xmlText), new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment
            });

            reader.Read();
            bool isSelfClosing = reader.IsEmptyElement;
            string openingTag = $"<{reader.Name}";

            // Add attributes
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    openingTag += $" {reader.Name}=\"{reader.Value}\"";
                }
            }

            openingTag += isSelfClosing ? "/>" : ">";

            string content = "";
            string closingTag = "";

            if (!isSelfClosing)
            {
                content = reader.ReadInnerXml();
                closingTag = $"</{reader.Name}>";
            }

            return (openingTag, content, closingTag);
        }

        private SyntaxList<XmlAttributeSyntax> ParseXmlAttributes(string openingTag)
        {
            SyntaxList<XmlAttributeSyntax> attributes = [];

            try
            {
                using XmlReader reader = XmlReader.Create(new StringReader(openingTag), new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Fragment
                });

                reader.Read();
                if (reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        attributes = attributes.Add(SyntaxFactory.XmlTextAttribute(
                            reader.Name,
                            SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken),
                            SyntaxFactory.XmlTextLiteral(reader.Value),
                            SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken)));
                    }
                }
            }
            catch
            {
                // Fallback if XML parsing fails
            }

            return attributes;
        }
    }
}
