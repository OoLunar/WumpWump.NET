using System.Collections.Generic;

namespace WumpWump.Net.Entities
{
    public record DiscordApplicationInstallParams
    {
        /// <summary>
        /// <a href="https://discord.com/developers/docs/topics/oauth2#shared-resources-oauth2-scopes">Scopes</a> to add the application to the server with.
        /// </summary>
        public required IReadOnlyList<string> Scopes { get; init; }

        /// <summary>
        /// <see cref="DiscordPermissions"/> to request for the bot role
        /// </summary>
        public required DiscordPermissions Permissions { get; init; }
    }
}
