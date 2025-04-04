using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WumpWump.Net.Analyze
{
    public static class Extensions
    {
        public static string GetName(this MemberDeclarationSyntax member)
        {
            try
            {
                return member switch
                {
                    BaseMethodDeclarationSyntax method => GetMethodName(method),
                    BasePropertyDeclarationSyntax property => GetPropertyName(property),
                    BaseFieldDeclarationSyntax field => GetFieldName(field),
                    BaseTypeDeclarationSyntax type => type.Identifier.Text,
                    DelegateDeclarationSyntax delegateDecl => delegateDecl.Identifier.Text,
                    _ => GetNameFromIdentifier(member)
                };
            }
            catch
            {
                return "unknown";
            }
        }

        private static string GetMethodName(BaseMethodDeclarationSyntax method) => method switch
        {
            ConstructorDeclarationSyntax ctor => ctor.Identifier.Text,
            DestructorDeclarationSyntax dtor => dtor.Identifier.Text,
            MethodDeclarationSyntax normalMethod => normalMethod.Identifier.Text,
            ConversionOperatorDeclarationSyntax conversion => $"operator {conversion.Type}",
            OperatorDeclarationSyntax op => $"operator {op.OperatorToken}",
            _ => "method"
        };

        private static string GetPropertyName(BasePropertyDeclarationSyntax property) => property switch
        {
            IndexerDeclarationSyntax => "this[]",
            PropertyDeclarationSyntax prop => prop.Identifier.Text,
            EventDeclarationSyntax evt => evt.Identifier.Text,
            _ => "property"
        };

        private static string GetFieldName(BaseFieldDeclarationSyntax field) => field.Declaration.Variables.FirstOrDefault()?.Identifier.Text ?? "field";

        private static string GetNameFromIdentifier(MemberDeclarationSyntax member)
        {
            // Try to find an IdentifierToken in common positions
            SyntaxToken identifier = member.ChildTokens()
                .FirstOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken));

            return identifier.Text;
        }
    }
}
