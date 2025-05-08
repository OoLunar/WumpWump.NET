using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Rest
{
    public partial class DiscordRestClient : IDiscordRestClient
    {
        /// <summary>
        /// Returns a list of partial <see cref="DiscordGuild"/> objects the current user is a member of. For OAuth2, requires the <c>guilds</c> scope.
        /// </summary>
        /// <remarks>
        /// This endpoint returns 200 guilds by default, which is the maximum number of guilds a non-bot user can join.
        /// Therefore, pagination is <b>not needed</b> for integrations that need to get a list of the users' guilds.
        /// </remarks>
        /// <param name="before">get guilds before this guild ID</param>
        /// <param name="after">get guilds after this guild ID</param>
        /// <param name="limit">max number of guilds to return (1-200)</param>
        /// <param name="withCounts">include approximate member and presence counts in response</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<IReadOnlyList<DiscordGuild>>> GetCurrentUserGuildsAsync(DiscordSnowflake? before = null, DiscordSnowflake? after = null, int limit = 200, bool withCounts = false, CancellationToken cancellationToken = default)
        {
            UriBuilder uriBuilder = new("https://discord.com/api/v10/users/@me/guilds");
            if (before is not null)
            {
                uriBuilder.Query += $"before={before}&";
            }

            if (after is not null)
            {
                uriBuilder.Query += $"after={after}&";
            }

            uriBuilder.Query += $"limit={limit}&with_counts={withCounts}";
            return await SendAsync<IReadOnlyList<DiscordGuild>>(new()
            {
                Method = HttpMethod.Get,
                Route = new Uri("https://discord.com/api/v10/users/@me/guilds"),
                Url = new Uri("https://discord.com/api/v10/users/@me/guilds"),
            }, cancellationToken);
        }

        /// <summary>
        /// Returns a <see cref="DiscordMember"/> object for the current user. Requires the <c>guilds.members.read</c> OAuth2 scope.
        /// </summary>
        /// <param name="guildId">The guild ID to get the member from.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordMember?>> GetCurrentUserGuildMemberAsync(DiscordSnowflake guildId, CancellationToken cancellationToken = default) => await SendAsync<DiscordMember?>(new()
        {
            Method = HttpMethod.Get,
            Route = new Uri("https://discord.com/api/v10/guilds/:guild_id/members/@me"),
            Url = new Uri($"https://discord.com/api/v10/guilds/{guildId}/members/@me"),
        }, cancellationToken);

        /// <summary>
        /// Leave a guild. Returns <see langword="null"/> on success. Fires a <a href="https://discord.com/developers/docs/events/gateway-events#guild-delete">Guild Delete</a> Gateway event and a <a href="https://discord.com/developers/docs/events/gateway-events#guild-member-remove">Guild Member Remove</a> Gateway event.
        /// </summary>
        /// <param name="guildId">The guild ID to leave.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<object?>> LeaveGuildAsync(DiscordSnowflake guildId, CancellationToken cancellationToken = default) => await SendAsync<object?>(new()
        {
            Method = HttpMethod.Delete,
            Route = new Uri("https://discord.com/api/v10/users/@me/guilds/:guild_id"),
            Url = new Uri($"https://discord.com/api/v10/users/@me/guilds/{guildId}"),
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<DiscordGuild?>> GetGuildAsync(DiscordSnowflake guildId, bool withCounts = false, CancellationToken cancellationToken = default) => await SendAsync<DiscordGuild?>(new()
        {
            Method = HttpMethod.Get,
            Route = new Uri("https://discord.com/api/v10/guilds/:guild_id"),
            Url = new Uri($"https://discord.com/api/v10/guilds/{guildId}?with_counts={withCounts}"),
        });
    }
}
