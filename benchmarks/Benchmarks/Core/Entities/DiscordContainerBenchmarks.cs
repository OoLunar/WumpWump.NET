using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Benchmarks.Benchmarks.Core.Entities
{
    public class DiscordPermissionContainerBenchmarks
    {
        private DiscordPermissionContainer _container;
        private DiscordPermission _testPermission;

        // "Interesting values"
        public IEnumerable<object> ContainerValues()
        {
            yield return new DiscordPermissionContainer(ulong.MinValue);
            yield return new DiscordPermissionContainer(1024);             // 000000000000000000000000000000000000000010000000000
            yield return new DiscordPermissionContainer(1116855673222);    // 000000000010000010000001001110010010010000110000110
            yield return new DiscordPermissionContainer(26398748180536);   // 000000110000000001001110000000000100000000000111000
            yield return new DiscordPermissionContainer(2222085186636353); // 111111001001111100110000110001101011100101001000001
            yield return new DiscordPermissionContainer(ulong.MaxValue);
        }

        [GlobalSetup]
        public void Setup()
        {
            _container = new DiscordPermissionContainer();

            // Set some random flags
            for (int i = 0; i < 10; i++)
            {
                _container.SetFlag(i, true);
            }

            _testPermission = DiscordPermission.ManageGuild;
        }

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public void SetFlag(DiscordPermissionContainer container) => container.SetFlag(10, true);

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public bool HasFlag(DiscordPermissionContainer container) => container.HasFlag(_testPermission);

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public bool HasPermission(DiscordPermissionContainer container) => container.HasPermission(_testPermission);

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public DiscordPermissionContainer BitwiseOr(DiscordPermissionContainer container) => container | _container;

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public DiscordPermissionContainer BitwiseAnd(DiscordPermissionContainer container) => container & _container;

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public DiscordPermissionContainer BitwiseXor(DiscordPermissionContainer container) => container ^ _container;

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public DiscordPermissionContainer BitwiseNot(DiscordPermissionContainer container) => ~container;

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public bool EqualityOperator(DiscordPermissionContainer container) => container == _container;

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public Span<byte> AsSpan(DiscordPermissionContainer container) => container.AsSpan();

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public int GetHashCodeBenchmark(DiscordPermissionContainer container) => container.GetHashCode();

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public string ToStringBenchmark(DiscordPermissionContainer container) => container.ToString();

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public string ToHexString(DiscordPermissionContainer container) => container.ToHexString();

        [Benchmark]
        [ArgumentsSource(nameof(ContainerValues))]
        public string ToPermissionsString(DiscordPermissionContainer container) => container.ToPermissionsString();
    }
}