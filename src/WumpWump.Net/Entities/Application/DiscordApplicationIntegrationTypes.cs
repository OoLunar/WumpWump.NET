namespace WumpWump.Net.Entities
{
    /// <summary>
    /// Where an app can be installed, also called its supported <a href="https://discord.com/developers/docs/resources/application#installation-context">installation contexts</a>.
    /// </summary>
    public enum DiscordApplicationIntegrationTypes
    {
        /// <summary>
        /// App is installable to servers
        /// </summary>
        GuildInstall = 0,

        /// <summary>
        /// App is installable to users
        /// </summary>
        UserInstall = 1
    }
}
