using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = WumpWump.Net.Tests.Analyze.CSharpCodeFixVerifier<WumpWump.Net.Analyze.DiscordEntitiesRequirePropertiesAnalyzer, Microsoft.CodeAnalysis.Testing.EmptyCodeFixProvider>;

namespace WumpWump.Net.Tests.Analyze
{
    [TestClass]
    public class DiscordEntitiesRequirePropertiesAnalyzerTests
    {
        // Valid case - required property in entity namespace
        [TestMethod]
        public async Task RequiredPropertyInEntityNamespaceNoDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public record TestEntity
    {
        public required string Name { get; set; }
    }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Valid case - DiscordOptional<T> property doesn't need required
        [TestMethod]
        public async Task DiscordOptionalPropertyNoDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public class DiscordOptional<T>;
    public record TestEntity
    {
        public DiscordOptional<string> Name { get; set; }
    }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Error case - non-required property in entity namespace
        [TestMethod]
        public async Task NonRequiredPropertyInEntityNamespaceReportsDiagnosticAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public record TestEntity
    {
        public string {|#0:Name|} { get; set; }
    }
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0004")
                .WithLocation(0)
                .WithArguments("Name");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // No diagnostic for private properties
        [TestMethod]
        public async Task PrivatePropertyNoDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public record TestEntity
    {
        private string Name { get; set; }
    }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // No diagnostic for static properties
        [TestMethod]
        public async Task StaticPropertyNoDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public record TestEntity
    {
        public static string Name { get; set; }
    }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // No diagnostic for expression-bodied properties
        [TestMethod]
        public async Task ExpressionBodiedPropertyNoDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public record TestEntity
    {
        public string Name => ""Default"";
    }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // No diagnostic for properties outside entity namespace
        [TestMethod]
        public async Task PropertyOutsideEntityNamespaceNoDiagnosticsAsync()
        {
            string test = @"
namespace Other.Namespace
{
    public record TestEntity
    {
        public string Name { get; set; }
    }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Multiple properties scenario
        [TestMethod]
        public async Task MultiplePropertiesMixedResultsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public class DiscordOptional<T>;
    public record TestEntity
    {
        public required string RequiredProp { get; set; }
        public string {|#0:NonRequiredProp|} { get; set; }
        public DiscordOptional<string> OptionalProp { get; set; }
    }
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0004")
                .WithLocation(0)
                .WithArguments("NonRequiredProp");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // Different entity namespace variations
        [TestMethod]
        public async Task DifferentEntityNamespaceVariationsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities.Sub
{
    public record TestEntity
    {
        public string {|#0:Name|} { get; set; }
    }
}

namespace WumpWump.Net.EntitiesV2
{
    public record TestEntity
    {
        public string {|#1:Name|} { get; set; }
    }
}
";

            DiagnosticResult[] expected =
            [
                VerifyCS.Diagnostic("WWL0004")
                    .WithLocation(0)
                    .WithArguments("Name"),
                VerifyCS.Diagnostic("WWL0004")
                    .WithLocation(1)
                    .WithArguments("Name")
            ];
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // Nested types scenario
        [TestMethod]
        public async Task NestedTypesReportsAppropriateDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public record OuterEntity
    {
        public record InnerEntity
        {
            public string {|#0:Name|} { get; set; }
        }
    }
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0004")
                .WithLocation(0)
                .WithArguments("Name");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}