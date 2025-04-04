using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Benchmarks.Benchmarks.Core.Entities
{
    public class DiscordPermissionContainerBenchmarks
    {
        private DiscordPermissionContainer _container;
        private DiscordPermission _testPermission;

        // Interesting values
        private static readonly ulong _uLongMinValue = ulong.MinValue; // 0
        private static readonly ulong _uLongMaxValue = ulong.MaxValue; // Max ulong
        private static readonly UInt128 _uInt128MinValue = UInt128.One + ulong.MaxValue; // One after ulong.MaxValue
        private static readonly UInt128 _uInt128MaxValue = UInt128.MaxValue << Math.Clamp(DiscordPermissionContainer.MAXIMUM_BIT_COUNT, 0, Unsafe.SizeOf<UInt128>()); // Max UInt128
        private static readonly BigInteger _bigIntMinValue = BigInteger.One + _uInt128MaxValue; // One after UInt128.MaxValue (ish)
        private static readonly BigInteger _bigIntMaxValue = BigInteger.One << (DiscordPermissionContainer.MAXIMUM_BIT_COUNT - 1); // BigInt case

        public IEnumerable<object> ContainerValues()
        {
            yield return new DiscordPermissionContainer(_uLongMinValue);
            yield return new DiscordPermissionContainer(_uLongMaxValue);
#if ENABLE_LARGE_PERMISSIONS
            yield return new DiscordPermissionContainer(_uInt128MinValue);
            yield return new DiscordPermissionContainer(_uInt128MaxValue);
            yield return new DiscordPermissionContainer(_bigIntMinValue);
            yield return new DiscordPermissionContainer(_bigIntMaxValue);
#endif
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