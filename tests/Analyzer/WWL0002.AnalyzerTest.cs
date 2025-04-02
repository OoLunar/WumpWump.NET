using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = WumpWump.Net.Tests.Analyze.CSharpCodeFixVerifier<
    WumpWump.Net.Analyze.DiscordEntitiesMustBeRecordsAnalyzer,
    Microsoft.CodeAnalysis.Testing.EmptyCodeFixProvider>;

namespace WumpWump.Net.Tests.Analyze
{
    [TestClass]
    public class DiscordEntitiesMustBeRecordsAnalyzerTests
    {
        // No diagnostics expected for empty code
        [TestMethod]
        public async Task EmptyCodeNoDiagnosticsAsync()
        {
            string test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Valid case - record in Discord namespace
        [TestMethod]
        public async Task RecordInDiscordNamespaceNoDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public record TestEntity;
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Error case - class in Discord namespace
        [TestMethod]
        public async Task ClassInDiscordNamespaceReportsDiagnosticAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public class {|#0:TestEntity|};
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0002")
                .WithLocation(0)
                .WithArguments("TestEntity", "WumpWump.Net.Entities");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // Error case - struct in Discord namespace
        [TestMethod]
        public async Task StructInDiscordNamespaceReportsDiagnosticAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public struct {|#0:TestEntity|};
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0002")
                .WithLocation(0)
                .WithArguments("TestEntity", "WumpWump.Net.Entities");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // No diagnostic for class not in Discord namespace
        [TestMethod]
        public async Task ClassNotInDiscordNamespaceNoDiagnosticsAsync()
        {
            string test = @"
namespace MyApplication.Entities
{
    public class TestEntity;
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // No diagnostic for static class in Discord namespace
        [TestMethod]
        public async Task StaticClassInDiscordNamespaceNoDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public static class TestUtility;
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Nested class scenario
        [TestMethod]
        public async Task NestedClassInDiscordNamespaceReportsDiagnosticAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public record OuterEntity
    {
        public class {|#0:InnerEntity|};
    }
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0002")
                .WithLocation(0)
                .WithArguments("InnerEntity", "WumpWump.Net.Entities");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // Multiple entities scenario
        [TestMethod]
        public async Task MultipleEntitiesMixedResultsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities
{
    public record ValidEntity;
    public class {|#0:InvalidEntity|};
}

namespace Other.Namespace
{
    public class RegularClass;
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0002").WithLocation(0).WithArguments("InvalidEntity", "WumpWump.Net.Entities");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // Different Discord namespace variations
        [TestMethod]
        public async Task DifferentDiscordNamespaceVariationsAsync()
        {
            string test = @"
namespace WumpWump.Net.Entities.Sub
{
    public class {|#0:SubEntity|};
}

namespace WumpWump.Net.EntitiesV2
{
    public class {|#1:V2Entity|};
}
";

            DiagnosticResult[] expected =
            [
                VerifyCS.Diagnostic("WWL0002").WithLocation(0).WithArguments("SubEntity", "WumpWump.Net.Entities.Sub"),
                VerifyCS.Diagnostic("WWL0002").WithLocation(1).WithArguments("V2Entity", "WumpWump.Net.EntitiesV2")
            ];

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}