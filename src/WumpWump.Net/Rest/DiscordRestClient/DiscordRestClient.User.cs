using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WumpWump.Net.Entities;
using WumpWump.Net.Rest.PostModels;

namespace WumpWump.Net.Rest
{
    public partial class DiscordRestClient : IDiscordRestClient
    {
        /// <summary>
        /// Returns the <see cref="DiscordUser"/> object of the requester's account. For OAuth2, this requires the <c>identify</c> scope, which will return the
        /// object without an email, and optionally the <c>email</c> scope, which returns the object with an email if the user has one.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordUser>> GetCurrentUserAsync(CancellationToken cancellationToken = default) => await SendAsync<DiscordUser>(new()
        {
            Method = HttpMethod.Get,
            Route = new Uri("https://discord.com/api/v10/users/"),
            Url = new Uri("https://discord.com/api/v10/users/@me"),
        }, cancellationToken);

        /// <summary>
        /// Returns a <see cref="DiscordUser"/> object for a given user ID.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordUser?>> GetUserAsync(DiscordSnowflake userId, CancellationToken cancellationToken = default) => await SendAsync<DiscordUser?>(new()
        {
            Method = HttpMethod.Get,
            Route = new Uri("https://discord.com/api/v10/users/:user_id"),
            Url = new Uri($"https://discord.com/api/v10/users/{userId}"),
        }, cancellationToken);

        /// <summary>
        /// Modify the requester's user account settings. Returns a <see cref="DiscordUser"/> object on success.
        /// Fires a <a href="https://discord.com/developers/docs/events/gateway-events#user-update">User Update</a> Gateway event.
        /// </summary>
        /// <param name="editModel">The model to edit the user with.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordUser>> ModifyCurrentUserAsync(DiscordUserEditModel editModel, CancellationToken cancellationToken = default) => await SendAsync<DiscordUser>(new()
        {
            Method = HttpMethod.Patch,
            Route = new Uri("https://discord.com/api/v10/users/"),
            Url = new Uri("https://discord.com/api/v10/users/@me"),
            Body = editModel
        }, cancellationToken);

        /// <summary>
        /// Create a new DM channel with a user. Returns a <see cref="DiscordDmChannel"/> object (if one already exists, it will be returned instead).
        /// </summary>
        /// <remarks>
        /// You should not use this endpoint to DM everyone in a server about something. DMs should generally be initiated by a user action.
        /// If you open a significant amount of DMs too quickly, your bot may be rate limited or blocked from opening new ones.
        /// </remarks>
        /// <param name="recipientId">The recipient to open a DM channel with.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordDmChannel>> CreateDmAsync(DiscordSnowflake recipientId, CancellationToken cancellationToken = default) => await SendAsync<DiscordDmChannel>(new()
        {
            Method = HttpMethod.Post,
            Route = new Uri("https://discord.com/api/v10/users/@me/channels"),
            Url = new Uri("https://discord.com/api/v10/users/@me/channels"),
            Body = new DiscordDmCreateModel()
            {
                RecipientId = recipientId
            }
        }, cancellationToken);

        /// <summary>
        /// Create a new group DM channel with multiple users. Returns a <see cref="DiscordDmChannel"/> object. This endpoint was intended to be used with the now-deprecated GameBridge SDK. Fires a <a href="https://discord.com/developers/docs/events/gateway-events#channel-create">Channel Create</a> Gateway event.
        /// </summary>
        /// <remarks>
        /// This endpoint is limited to 10 active group DMs.
        /// </remarks>
        /// <param name="accessTokens">access tokens of users that have granted your app the gdm.join scope</param>
        /// <param name="nicks">a dictionary of user ids to their respective nicknames</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordDmChannel>> CreateGroupDmAsync(IEnumerable<string> accessTokens, IReadOnlyDictionary<DiscordSnowflake, string?> nicks, CancellationToken cancellationToken = default) => await SendAsync<DiscordDmChannel>(new()
        {
            Method = HttpMethod.Post,
            Route = new Uri("https://discord.com/api/v10/users/@me/channels"),
            Url = new Uri("https://discord.com/api/v10/users/@me/channels"),
            Body = new DiscordGroupDmCreateModel()
            {
                AccessTokens = accessTokens,
                Nicks = nicks
            }
        }, cancellationToken);

        /// <summary>
        /// Returns a list of connection objects. Requires the <c>connections</c> OAuth2 scope.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<IReadOnlyList<DiscordUserConnection>>> GetUserConnectionsAsync(CancellationToken cancellationToken = default) => await SendAsync<IReadOnlyList<DiscordUserConnection>>(new()
        {
            Method = HttpMethod.Get,
            Route = new Uri("https://discord.com/api/v10/users/@me/connections"),
            Url = new Uri("https://discord.com/api/v10/users/@me/connections")
        }, cancellationToken);
    }
}
