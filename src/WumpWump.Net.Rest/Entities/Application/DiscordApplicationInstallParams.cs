using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    public record DiscordApplicationInstallParams
    {
        /// <summary>
        /// <a href="https://discord.com/developers/docs/topics/oauth2#shared-resources-oauth2-scopes">Scopes</a> to add the application to the server with.
        /// </summary>
        public required IReadOnlyList<string> Scopes { get; init; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/topics/permissions"/> to request for the bot role
        /// </summary>
        public required DiscordPermissionContainer Permissions { get; init; }
    }
}
