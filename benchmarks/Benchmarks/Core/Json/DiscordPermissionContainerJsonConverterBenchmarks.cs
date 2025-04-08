using System.Text.Json;
using BenchmarkDotNet.Attributes;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Benchmarks.Benchmarks.Core.Json
{
    public class DiscordPermissionContainerJsonConverterBenchmarks
    {
        private static readonly DiscordPermissionContainer _ulongMaxValuePermissions = JsonSerializer.Deserialize<DiscordPermissionContainer>($"\"{ulong.MaxValue}\"")!;

        [Benchmark]
        public void Read_UlongPermission_Min() => JsonSerializer.Deserialize<DiscordPermissionContainer>("\"0\"");

        [Benchmark]
        public void Read_UlongPermission_Max() => JsonSerializer.Deserialize<DiscordPermissionContainer>("\"18446744073709551615\"");

        [Benchmark]
        public void Write_NonePermission() => JsonSerializer.Serialize(DiscordPermissionContainer.None);

        [Benchmark]
        public void Write_UlongPermission_Max() => JsonSerializer.Serialize(_ulongMaxValuePermissions);
    }
}