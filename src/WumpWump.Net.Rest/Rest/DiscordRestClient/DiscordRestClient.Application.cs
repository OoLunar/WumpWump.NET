using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WumpWump.Net.Rest.Entities;
using WumpWump.Net.Rest.PostModels;

namespace WumpWump.Net.Rest
{
    public partial class DiscordRestClient : IDiscordRestClient
    {
        /// <summary>
        /// Returns the <see cref="DiscordApplication"/> associated with the requesting bot user.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordApplication>> GetCurrentApplicationAsync(CancellationToken cancellationToken = default) => await SendAsync<DiscordApplication>(new()
        {
            Method = HttpMethod.Get,
            Route = new Uri("https://discord.com/api/v10/applications/"),
            Url = new Uri("https://discord.com/api/v10/applications/@me"),
        }, cancellationToken);

        /// <summary>
        /// Edit properties of the app associated with the requesting bot user. Only properties that are passed will be updated. Returns the updated <see cref="DiscordApplication"/> object on success.
        /// </summary>
        /// <param name="editModel">The model to edit the application with.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordApplication>> EditCurrentApplicationAsync(DiscordApplicationEditModel editModel, CancellationToken cancellationToken = default) => await SendAsync<DiscordApplication>(new()
        {
            Method = HttpMethod.Patch,
            Route = new Uri("https://discord.com/api/v10/applications/"),
            Url = new Uri("https://discord.com/api/v10/applications/@me"),
            Body = editModel
        }, cancellationToken);

        /// <summary>
        /// Returns a serialized activity instance, if it exists. Useful for preventing unwanted activity sessions.
        /// </summary>
        /// <param name="applicationId">The application ID to get the activity instance for.</param>
        /// <param name="instanceId">The instance ID to get the activity instance for.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        /// <returns>A serialized activity instance, if it exists.</returns>
        public async ValueTask<DiscordApiResponse<DiscordApplicationActivityInstance?>> GetApplicationActivityInstanceAsync(DiscordSnowflake applicationId, DiscordSnowflake instanceId, CancellationToken cancellationToken = default) => await SendAsync<DiscordApplicationActivityInstance?>(new()
        {
            Method = HttpMethod.Get,
            Route = new Uri($"https://discord.com/api/v10/applications/{applicationId}/activity-instances/:instance_id"),
            Url = new Uri($"https://discord.com/api/v10/applications/{applicationId}/activity-instances/{instanceId}")
        }, cancellationToken);

        /// <summary>
        /// Returns the application role connection for the user. Requires an OAuth2 access token with <c>role_connections.write</c> scope for the application specified in the path.
        /// </summary>
        /// <param name="applicationId">The application ID to get the role connection for.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordApplicationRoleConnection>> GetCurrentUserApplicationRoleConnectionAsync(DiscordSnowflake applicationId, CancellationToken cancellationToken = default) => await SendAsync<DiscordApplicationRoleConnection>(new()
        {
            Method = HttpMethod.Get,
            Route = new Uri("https://discord.com/api/v10/users/@me/applications/:application_id/role-connection"),
            Url = new Uri($"https://discord.com/api/v10/users/@me/applications/{applicationId}/role-connection")
        }, cancellationToken);

        /// <summary>
        /// Updates and returns the <see cref="DiscordApplicationRoleConnection"/> for the user. Requires an OAuth2 access token with <c>role_connections.write</c> scope for the application specified in the path.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="updatedApplicationRoleConnection"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="DiscordApplicationRoleConnection"></typeparam>
        /// <returns></returns>
        public async ValueTask<DiscordApiResponse<DiscordApplicationRoleConnection>> UpdateCurrentUserApplicationRoleConnectionAsync(DiscordSnowflake applicationId, DiscordApplicationRoleConnection updatedApplicationRoleConnection, CancellationToken cancellationToken = default) => await SendAsync<DiscordApplicationRoleConnection>(new()
        {
            Method = HttpMethod.Put,
            Route = new Uri("https://discord.com/api/v10/users/@me/applications/:application_id/role-connection"),
            Url = new Uri($"https://discord.com/api/v10/users/@me/applications/{applicationId}/role-connection"),
            Body = updatedApplicationRoleConnection
        }, cancellationToken);
    }
}
