using System;
using BenchmarkDotNet.Attributes;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Benchmarks.Benchmarks.Core.Entities
{
    public class DiscordPermissionContainerBenchmarks
    {
        private DiscordPermissionContainer _container1;
        private DiscordPermissionContainer _container2;
        private DiscordPermission _testPermission;

        [GlobalSetup]
        public void Setup()
        {
            _container1 = new DiscordPermissionContainer();
            _container2 = new DiscordPermissionContainer();

            // Set some random flags
            for (int i = 0; i < 10; i++)
            {
                _container1.SetFlag(i, true);
                _container2.SetFlag(i + 5, true);
            }

            _testPermission = DiscordPermission.ManageGuild;
        }

        [Benchmark]
        public void SetFlag() => _container1.SetFlag(10, true);

        [Benchmark]
        public bool HasFlag() => _container1.HasFlag(_testPermission);

        [Benchmark]
        public bool HasPermission() => _container1.HasPermission(_testPermission);

        [Benchmark]
        public DiscordPermissionContainer BitwiseOr() => _container1 | _container2;

        [Benchmark]
        public DiscordPermissionContainer BitwiseAnd() => _container1 & _container2;

        [Benchmark]
        public DiscordPermissionContainer BitwiseXor() => _container1 ^ _container2;

        [Benchmark]
        public DiscordPermissionContainer BitwiseNot() => ~_container1;

        [Benchmark]
        public bool EqualityOperator() => _container1 == _container2;

        [Benchmark]
        public Span<byte> AsSpan() => _container1.AsSpan();

        [Benchmark]
        public int GetHashCodeBenchmark() => _container1.GetHashCode();

        [Benchmark]
        public string ToStringBenchmark() => _container1.ToString();

        [Benchmark]
        public string ToHexString() => _container1.ToHexString();

        [Benchmark]
        public string ToPermissionsString() => _container1.ToPermissionsString();
    }
}