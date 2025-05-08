using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace WumpWump.Net.Analyze.Entities
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FileNameClassMatchingAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "WWL0007";
        private const string Category = "Naming";

        private static readonly string Title = "File name should match type name";
        private static readonly string MessageFormat = "Type '{0}' should be in a file named '{1}.cs'";
        private static readonly string Description = "File names should match the name of the primary type they contain.";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Description,
            customTags: [WellKnownDiagnosticTags.CustomSeverityConfigurable]
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeCompilationUnit, SyntaxKind.CompilationUnit);
        }

        private static void AnalyzeCompilationUnit(SyntaxNodeAnalysisContext context)
        {
            CompilationUnitSyntax compilationUnit = (CompilationUnitSyntax)context.Node;

            // Get all type declarations in the compilation unit
            IEnumerable<BaseTypeDeclarationSyntax> typeDeclarations = compilationUnit.DescendantNodes().OfType<BaseTypeDeclarationSyntax>();

            // Group the type declarations by their file names
            Dictionary<string, List<BaseTypeDeclarationSyntax>> typeDeclarationsByFileName = [];
            foreach (BaseTypeDeclarationSyntax typeDeclaration in typeDeclarations)
            {
                string fileName = Path.GetFileNameWithoutExtension(typeDeclaration.SyntaxTree.FilePath);
                if (!typeDeclarationsByFileName.TryGetValue(fileName, out List<BaseTypeDeclarationSyntax>? declarations))
                {
                    declarations = [];
                    typeDeclarationsByFileName[fileName] = declarations;
                }

                declarations.Add(typeDeclaration);
            }

            // Iterate through each file name and check if it matches the type name
            foreach (KeyValuePair<string, List<BaseTypeDeclarationSyntax>> kvp in typeDeclarationsByFileName)
            {
                string fileName = kvp.Key;
                List<BaseTypeDeclarationSyntax> declarations = kvp.Value;
                if (declarations.Count > 1)
                {
                    // Skip files with multiple type declarations
                    continue;
                }

                BaseTypeDeclarationSyntax typeDeclaration = declarations[0];
                INamedTypeSymbol? typeSymbol = context.SemanticModel.GetDeclaredSymbol(typeDeclaration);
                if (typeSymbol is null)
                {
                    return;
                }

                string? expectedFileName = GetExpectedFileName(typeSymbol);
                if (expectedFileName is null)
                {
                    // Skip nested types
                    return;
                }

                string? actualFileName = context.Compilation.SyntaxTrees.FirstOrDefault()?.FilePath;
                if (string.IsNullOrEmpty(actualFileName))
                {
                    return;
                }

                // Handle partial class pattern matching
                if (expectedFileName.EndsWith(".*"))
                {
                    string baseName = expectedFileName.Substring(0, expectedFileName.Length - 2);
                    if (!fileName.StartsWith(baseName))
                    {
                        Diagnostic diagnostic = Diagnostic.Create(Rule, typeDeclaration.Identifier.GetLocation(), typeSymbol.Name, $"{baseName}.*");
                        context.ReportDiagnostic(diagnostic);
                    }
                }
                // Handle exact match for non-partial classes
                else if (!fileName.Equals(expectedFileName))
                {
                    Diagnostic diagnostic = Diagnostic.Create(Rule, typeDeclaration.Identifier.GetLocation(), typeSymbol.Name, expectedFileName);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        protected internal static string? GetExpectedFileName(INamedTypeSymbol typeSymbol)
        {
            // Skip nested types entirely
            if (typeSymbol.ContainingType != null)
            {
                return null;
            }

            string baseName = typeSymbol.Name;

            // Handle generic types
            if (typeSymbol.IsGenericType)
            {
                int arity = typeSymbol.TypeParameters.Length;
                baseName = $"{baseName}`{arity}";
            }

            // Handle partial classes
            if (typeSymbol.DeclaringSyntaxReferences.Any(syntaxReference => syntaxReference.GetSyntax() is BaseTypeDeclarationSyntax typeSyntax
                && typeSyntax.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PartialKeyword))))
            {
                // For partial classes, the filename must START with the class name
                // but can have additional text after it
                return $"{baseName}.*";
            }

            return baseName;
        }
    }
}