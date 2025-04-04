using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Benchmarks.Benchmarks.Core.Json
{
    public class DiscordPermissionContainerJsonConverterBenchmarks
    {
        private static readonly UInt128 _uInt128MinValue = UInt128.One + ulong.MaxValue; // One after ulong.MaxValue
        private static readonly UInt128 _uInt128MaxValue = UInt128.MaxValue << Math.Clamp(DiscordPermissionContainer.MAXIMUM_BIT_COUNT, 0, Unsafe.SizeOf<UInt128>()); // Max UInt128
        private static readonly BigInteger _bigIntMinValue = BigInteger.One + _uInt128MaxValue; // One after UInt128.MaxValue (ish)
        private static readonly BigInteger _bigIntMaxValue = BigInteger.One << (DiscordPermissionContainer.MAXIMUM_BIT_COUNT - 1); // BigInt case

        private static readonly DiscordPermissionContainer _ulongMaxValuePermissions = JsonSerializer.Deserialize<DiscordPermissionContainer>($"\"{ulong.MaxValue}\"")!;
        private static readonly DiscordPermissionContainer _uInt128MaxValuePermissions = JsonSerializer.Deserialize<DiscordPermissionContainer>($"\"{_uInt128MaxValue}\"")!;

        private static readonly string _uint128MinValueJson = $"\"{_uInt128MinValue}\""; // One after ulong.MaxValue
        private static readonly string _uInt128MaxValueJson = $"\"{_uInt128MaxValue}\""; // Max UInt128
        private static readonly string _bigIntMinValueJson = $"\"{_bigIntMinValue}\""; // One after UInt128.MaxValue (ish)
        private static readonly string _bigIntMaxValueJson = $"\"{_bigIntMaxValue}\""; // BigInt case

        [Benchmark]
        public void Read_UlongPermission_Min() => JsonSerializer.Deserialize<DiscordPermissionContainer>("\"0\"");

        [Benchmark]
        public void Read_UlongPermission_Max() => JsonSerializer.Deserialize<DiscordPermissionContainer>("\"18446744073709551615\"");

#if ENABLE_LARGE_PERMISSIONS
        [Benchmark]
        public void Read_UInt128Permission_Min() => JsonSerializer.Deserialize<DiscordPermissionContainer>(_uint128MinValueJson);

        [Benchmark]
        public void Read_UInt128Permission_Max() => JsonSerializer.Deserialize<DiscordPermissionContainer>(_uInt128MaxValueJson);

        [Benchmark]
        public void Read_BigIntegerPermission_Min() => JsonSerializer.Deserialize<DiscordPermissionContainer>(_bigIntMinValueJson);

        [Benchmark]
        public void Read_BigIntegerPermission_Max() => JsonSerializer.Deserialize<DiscordPermissionContainer>(_bigIntMaxValueJson);
#endif

        [Benchmark]
        public void Write_NonePermission() => JsonSerializer.Serialize(DiscordPermissionContainer.None);

        [Benchmark]
        public void Write_UlongPermission_Max() => JsonSerializer.Serialize(_ulongMaxValuePermissions);

#if ENABLE_LARGE_PERMISSIONS
        [Benchmark]
        public void Write_UInt128Permission_Max() => JsonSerializer.Serialize(_uInt128MaxValuePermissions);

        [Benchmark]
        public void Write_BigIntegerPermission_Max() => JsonSerializer.Serialize(DiscordPermissionContainer.All);
#endif
    }
}