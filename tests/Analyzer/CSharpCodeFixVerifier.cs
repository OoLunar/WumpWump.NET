using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace WumpWump.Net.Tests.Analyze
{
    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        public static DiagnosticResult Diagnostic()
            => CodeFixVerifier<TAnalyzer, TCodeFix, CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>, DefaultVerifier>.Diagnostic();

        public static DiagnosticResult Diagnostic(string diagnosticId)
            => CodeFixVerifier<TAnalyzer, TCodeFix, CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>, DefaultVerifier>.Diagnostic(diagnosticId);

        public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
            => CodeFixVerifier<TAnalyzer, TCodeFix, CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>, DefaultVerifier>.Diagnostic(descriptor);

        public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
        {
            Test test = new()
            {
                TestCode = source,
            };

            test.ExpectedDiagnostics.AddRange(expected);
            await test.RunAsync(CancellationToken.None);
        }

        public static async Task VerifyCodeFixAsync(string source, string fixedSource)
            => await VerifyCodeFixAsync(source, DiagnosticResult.EmptyDiagnosticResults, fixedSource);

        public static async Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource)
            => await VerifyCodeFixAsync(source, [expected], fixedSource);

        public static async Task VerifyCodeFixAsync(string source, DiagnosticResult[] expected, string fixedSource)
        {
            Test test = new()
            {
                TestCode = source,
                FixedCode = fixedSource,
            };

            test.ExpectedDiagnostics.AddRange(expected);
            await test.RunAsync(CancellationToken.None);
        }

        private class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>
        {
            public Test() => SolutionTransforms.Add((solution, projectId) =>
            {
                CompilationOptions compilationOptions = solution.GetProject(projectId)?.CompilationOptions ?? new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
                compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));
                solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);
                return solution;
            });
        }
    }

    public static partial class CSharpCodeFixVerifier<TAnalyzer> where TAnalyzer : DiagnosticAnalyzer, new()
    {
        public static DiagnosticResult Diagnostic() => CSharpCodeFixVerifier<TAnalyzer, EmptyCodeFixProvider>.Diagnostic();

        public static DiagnosticResult Diagnostic(string diagnosticId) => CSharpCodeFixVerifier<TAnalyzer, EmptyCodeFixProvider>.Diagnostic(diagnosticId);

        public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor) => CSharpCodeFixVerifier<TAnalyzer, EmptyCodeFixProvider>.Diagnostic(descriptor);

        public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected) => await CSharpCodeFixVerifier<TAnalyzer, EmptyCodeFixProvider>.VerifyAnalyzerAsync(source, expected);
    }

    public static class CSharpVerifierHelper
    {
        public static readonly ImmutableDictionary<string, ReportDiagnostic> NullableWarnings = GetNullableWarningsFromCompiler();

        private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler()
        {
            string[] args = ["/warnaserror:nullable"];
            CSharpCommandLineArguments commandLineArguments = CSharpCommandLineParser.Default.Parse(args, baseDirectory: Environment.CurrentDirectory, sdkDirectory: Environment.CurrentDirectory);
            ImmutableDictionary<string, ReportDiagnostic> nullableWarnings = commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;
            return nullableWarnings;
        }
    }
}