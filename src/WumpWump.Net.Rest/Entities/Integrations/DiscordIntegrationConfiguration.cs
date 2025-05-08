namespace WumpWump.Net.Rest.Entities
{
    public record DiscordIntegrationConfiguration
    {
        /// <summary>
        /// Install params for each installation context's default in-app authorization link
        /// </summary>
        public DiscordOptional<DiscordApplicationInstallParams> OAuth2InstallParams { get; init; }
    }
}
