namespace WumpWump.Net.Gateway.Entities.Commands
{
    public record DiscordGatewayIdentifyCommandProperties
    {
        public const string LIBRARY_NAME = ThisAssembly.Project.AssemblyName;
        public const string LIBRARY_REPOSITORY_URL = ThisAssembly.Project.RepositoryUrl;

#if DEBUG
        public const string LIBRARY_VERSION = $"{ThisAssembly.Project.Version}+{ThisAssembly.Project.RepositoryCommit}";
#else
        public const string LIBRARY_VERSION = ThisAssembly.Project.Version;
#endif

        public const string DISCORD_USER_AGENT = $"DiscordBot ({LIBRARY_REPOSITORY_URL}, v{LIBRARY_VERSION})";

        protected static readonly string _operatingSystem = System.Runtime.InteropServices.RuntimeInformation.OSDescription;

        /// <summary>
        /// Your operating system
        /// </summary>
        public string Os { get; init; } = _operatingSystem;

        /// <summary>
        /// Your library name
        /// </summary>
        public string Browser { get; init; } = DISCORD_USER_AGENT;

        /// <summary>
        /// Your library name
        /// </summary>
        public string Device { get; init; } = DISCORD_USER_AGENT;

        /// <summary>
        /// An explicit constructor for the <see cref="DiscordGatewayIdentifyCommandProperties"/> struct.
        /// </summary>
        public DiscordGatewayIdentifyCommandProperties() { }
    }
}
