using System.Collections.Immutable;
using System.IO;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace WumpWump.Net.Analyze
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LinkAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "WD0002";
        private const string Category = "Documentation";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            "Use Anchor Tags Instead of See Tags",
            "Use <a href=\"...\"> instead of <see href=\"...\">",
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeDocumentation, SyntaxKind.SingleLineDocumentationCommentTrivia);
            context.RegisterSyntaxNodeAction(AnalyzeDocumentation, SyntaxKind.MultiLineDocumentationCommentTrivia);
        }

        private void AnalyzeDocumentation(SyntaxNodeAnalysisContext context)
        {
            DocumentationCommentTriviaSyntax documentation = (DocumentationCommentTriviaSyntax)context.Node;
            foreach (SyntaxNode node in documentation.ChildNodes())
            {
                if (node is XmlElementSyntax element)
                {
                    CheckXmlElement(context, element);
                }
            }
        }

        private void CheckXmlElement(SyntaxNodeAnalysisContext context, XmlElementSyntax element)
        {
            try
            {
                string text = element.ToString();
                XmlTextReader reader = new(new StringReader(text));
                IXmlLineInfo lineInfo = reader;
                while (reader.Read())
                {
                    // Check if it's an element node with name "see"
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "see")
                    {
                        // Return true if href attribute exists
                        if (reader.GetAttribute("href") != null)
                        {
                            int i = 0;
                            int index = 0;
                            while (i < lineInfo.LineNumber - 1)
                            {
                                index = text.IndexOf('\n', index + 1);
                                i++;
                            }

                            int start = index - 1 + lineInfo.LinePosition;

                            // Backwards search for the start of the element
                            while (start > 0 && text[start] != '<')
                            {
                                start--;
                            }

                            int end = text.IndexOf('>', start) + 1;
                            ReportDiagnostic(context, element, "Use <a href=\"...\"> instead of <see href=\"...\">", start, end);

                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void ReportDiagnostic(SyntaxNodeAnalysisContext context, SyntaxNode node, string message, int start, int end)
        {
            Location location = node.GetLocation();

            // Offset by start and end
            location = Location.Create(location.SourceTree!, TextSpan.FromBounds(location.SourceSpan.Start + start, location.SourceSpan.Start + end));

            Diagnostic diagnostic = Diagnostic.Create(Rule, location, message);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
