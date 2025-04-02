using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = WumpWump.Net.Tests.Analyze.CSharpCodeFixVerifier<
    WumpWump.Net.Analyze.PrivateToProtectedMemberAnalyzer,
    Microsoft.CodeAnalysis.Testing.EmptyCodeFixProvider
>;

namespace WumpWump.Net.Tests.Analyze
{
    [TestClass]
    public class PrivateToProtectedMemberAnalyzerTests
    {
        // No diagnostics expected to show up
        [TestMethod]
        public async Task EmptyCodeNoDiagnosticsAsync()
        {
            string test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task SealedClassPrivateMemberNoDiagnosticsAsync()
        {
            string test = @"
sealed class TestClass
{
    private int privateField;
    private void PrivateMethod() { }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task StaticClassPrivateMemberNoDiagnosticsAsync()
        {
            string test = @"
static class TestClass
{
    private static int privateField;
    private static void PrivateMethod() { }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task UnsealedClassPrivateFieldDiagnosticAsync()
        {
            string test = @"
class TestClass
{
    private int {|#0:privateField|};
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0001")
                .WithLocation(0)
                .WithArguments("privateField");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task UnsealedClassPrivateMethodDiagnosticAsync()
        {
            string test = @"
class TestClass
{
    private void {|#0:PrivateMethod|}() { }
}
";

            DiagnosticResult expected = VerifyCS.Diagnostic("WWL0001")
                .WithLocation(0)
                .WithArguments("PrivateMethod");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task UnsealedClassProtectedMemberNoDiagnosticsAsync()
        {
            string test = @"
class TestClass
{
    protected int protectedField;
    protected void ProtectedMethod() { }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task StructPrivateMemberNoDiagnosticsAsync()
        {
            string test = @"
struct TestStruct
{
    private int privateField;
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task PrivateDelegateNoDiagnosticsAsync()
        {
            string test = @"
class TestClass
{
    private delegate void PrivateDelegate();
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }
    }
}