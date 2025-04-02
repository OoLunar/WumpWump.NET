using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = WumpWump.Net.Tests.Analyze.CSharpCodeFixVerifier<
    WumpWump.Net.Analyze.Entities.DiscordGatewayEntitiesMustBeReadOnlyRecordStructsAnalyzer,
    Microsoft.CodeAnalysis.Testing.EmptyCodeFixProvider>;

namespace WumpWump.Net.Tests.Analyze
{
    [TestClass]
    public class DiscordGatewayEntitiesMustBeReadOnlyRecordStructsAnalyzerTests
    {
        // Valid case - readonly record struct in gateway namespace
        [TestMethod]
        public async Task ReadOnlyRecordStructInGatewayNamespaceNoDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Gateway.Entities
{
    public readonly record struct TestEntity;
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Error case - class in gateway namespace
        [TestMethod]
        public async Task ClassInGatewayNamespaceReportsDiagnosticAsync()
        {
            string test = @"
namespace WumpWump.Net.Gateway.Entities
{
    public class {|#0:TestEntity|};
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0003").WithLocation(0).WithArguments("TestEntity", "WumpWump.Net.Gateway.Entities");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // Error case - non-readonly record struct in gateway namespace
        [TestMethod]
        public async Task NonReadOnlyRecordStructInGatewayNamespaceReportsDiagnosticAsync()
        {
            string test = @"
namespace WumpWump.Net.Gateway.Entities
{
    public record struct {|#0:TestEntity|};
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0003").WithLocation(0).WithArguments("TestEntity", "WumpWump.Net.Gateway.Entities");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // Error case - regular struct in gateway namespace
        [TestMethod]
        public async Task StructInGatewayNamespaceReportsDiagnosticAsync()
        {
            string test = @"
namespace WumpWump.Net.Gateway.Entities
{
    public struct {|#0:TestEntity|};
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0003").WithLocation(0).WithArguments("TestEntity", "WumpWump.Net.Gateway.Entities");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // No diagnostic for entities outside gateway namespace
        [TestMethod]
        public async Task EntityNotInGatewayNamespaceNoDiagnosticsAsync()
        {
            string test = @"
namespace Discord.API.Rest.Entities
{
    public class TestEntity;
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // No diagnostic for static classes
        [TestMethod]
        public async Task StaticClassInGatewayNamespaceNoDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Gateway.Entities
{
    public static class TestUtility;
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Nested type scenarios
        [TestMethod]
        public async Task NestedTypesInGatewayNamespaceReportsAppropriateDiagnosticsAsync()
        {
            string test = @"
namespace WumpWump.Net.Gateway.Entities
{
    public readonly record struct OuterEntity
    {
        public struct {|#0:InnerEntity|};
    }
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0003").WithLocation(0).WithArguments("InnerEntity", "WumpWump.Net.Gateway.Entities");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // Multiple entities scenario
        [TestMethod]
        public async Task MultipleEntitiesMixedResultsAsync()
        {
            string test = @"
namespace WumpWump.Net.Gateway.Entities
{
    public readonly record struct ValidEntity;
    public class {|#0:InvalidEntity|};
}

namespace Other.Namespace
{
    public class RegularClass;
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0003").WithLocation(0).WithArguments("InvalidEntity", "WumpWump.Net.Gateway.Entities");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        // Different gateway namespace variations
        [TestMethod]
        public async Task DifferentGatewayNamespaceVariationsAsync()
        {
            string test = @"
namespace WumpWump.Net.Gateway.Entities.Sub
{
    public struct {|#0:SubEntity|};
}

namespace WumpWump.Net.Gateway.EntitiesV2
{
    public record struct {|#1:V2Entity|};
}
";

            DiagnosticResult[] expected =
            [
                VerifyCS.Diagnostic("WWL0003").WithLocation(0).WithArguments("SubEntity", "WumpWump.Net.Gateway.Entities.Sub"),
                VerifyCS.Diagnostic("WWL0003").WithLocation(1).WithArguments("V2Entity", "WumpWump.Net.Gateway.EntitiesV2")
            ];

            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}