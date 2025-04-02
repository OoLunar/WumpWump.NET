using BenchmarkDotNet.Attributes;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Benchmarks.Core.Entities
{
    public class DiscordPermissionsBenchmarks
    {
        private readonly DiscordPermissions _adminPerm = DiscordPermissions.Administrator;
        private readonly DiscordPermissions _manageChannels = DiscordPermissions.ManageChannels;
        private readonly DiscordPermissions _allPerms = DiscordPermissions.All;
        private readonly DiscordPermissions _customPerms;

        public DiscordPermissionsBenchmarks() => _customPerms = DiscordPermissions.Create(64) | DiscordPermissions.Create(127);

        [Benchmark]
        public DiscordPermissions CreateDefault() => new();

        [Benchmark]
        public DiscordPermissions CreateWithBit() => DiscordPermissions.Create(42);

        [Benchmark]
        public DiscordPermissions BitwiseOr() => _adminPerm | _manageChannels;

        [Benchmark]
        public DiscordPermissions BitwiseAnd() => _allPerms & _adminPerm;

        [Benchmark]
        public DiscordPermissions BitwiseNot() => ~_adminPerm;

        [Benchmark]
        public bool HasFlag() => _allPerms.HasFlag(_adminPerm);

        [Benchmark]
        public void SetFlag()
        {
            DiscordPermissions perms = _adminPerm;
            perms.SetFlag(_manageChannels, true);
        }

        [Benchmark]
        public bool EqualityCheck() => _adminPerm == _manageChannels;

        [Benchmark]
        public string ToStringBenchmark() => _customPerms.ToString();
    }
}
