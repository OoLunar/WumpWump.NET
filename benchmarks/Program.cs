using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

namespace WumpWump.Net.Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Locate all the benchmarks
            Type[] types = FindBenchmarkTypes(typeof(Program).Assembly.GetTypes()).Distinct().ToArray();

            // Run the benchmarks
            IConfig config = ManualConfig
                .CreateMinimumViable()
                .AddColumn([StatisticColumn.Max, StatisticColumn.Min])
                .AddDiagnoser([new MemoryDiagnoser(new())])
                .AddExporter([MarkdownExporter.GitHub])
                .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));

#if DEBUG
            config = config.WithOptions(ConfigOptions.DisableOptimizationsValidator).AddJob(Job.Dry).StopOnFirstError(false);
#endif

            BenchmarkRunner.Run(types, config, args);
        }

        private static IEnumerable<Type> FindBenchmarkTypes(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                if (type.GetCustomAttribute<BenchmarkAttribute>() is not null)
                {
                    yield return type;
                }

                foreach (Type nestedType in FindBenchmarkTypes(type.GetNestedTypes()))
                {
                    yield return nestedType;
                }

                foreach (Type nestedType in FindBenchmarkTypes(type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)))
                {
                    yield return nestedType;
                }
            }
        }

        private static IEnumerable<Type> FindBenchmarkTypes(IEnumerable<MethodInfo> methods)
        {
            foreach (MethodInfo method in methods)
            {
                if (method.GetCustomAttribute<BenchmarkAttribute>() is not null)
                {
                    yield return method.DeclaringType!;
                }
            }
        }
    }
}
