using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WumpWump.Net.Rest.Entities;
using WumpWump.Net.Rest.Entities.Gateway;
using WumpWump.Net.Rest.JsonParameterModels;
using WumpWump.Net.Rest.QueryParameterModels;

namespace WumpWump.Net.Rest
{
    public partial class DiscordRestClient : IDiscordRestClient
    {
        #region Application

        /// <summary>
        /// Returns the <see cref="DiscordApplication"/> associated with the requesting bot user.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordApplication>> GetCurrentApplicationAsync(CancellationToken cancellationToken = default) => await SendAsync<DiscordApplication>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.GetCurrentApplication)
        }, cancellationToken);

        /// <summary>
        /// Edit properties of the app associated with the requesting bot user. Only properties that are passed will be updated. Returns the updated <see cref="DiscordApplication"/> object on success.
        /// </summary>
        /// <param name="editModel">The model to edit the application with.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordApplication>> EditCurrentApplicationAsync(DiscordEditApplicationJsonParameterModel editModel, CancellationToken cancellationToken = default) => await SendAsync<DiscordApplication>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.EditCurrentApplication),
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
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.GetApplicationActivityInstance, applicationId, instanceId)
        }, cancellationToken);

        #endregion
        #region Application Role Connection Metadata

        public async ValueTask<DiscordApiResponse<IReadOnlyList<DiscordApplicationRoleConnectionMetadata>>> GetApplicationRoleConnectionMetadataRecordsAsync(DiscordSnowflake applicationId, CancellationToken cancellationToken = default) => await SendAsync<IReadOnlyList<DiscordApplicationRoleConnectionMetadata>>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.GetCurrentUserApplicationRoleConnectionMetadata, applicationId)
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<IReadOnlyList<DiscordApplicationRoleConnectionMetadata>>> UpdateApplicationRoleConnectionMetadataRecordsAsync(DiscordSnowflake applicationId, CancellationToken cancellationToken = default) => await SendAsync<IReadOnlyList<DiscordApplicationRoleConnectionMetadata>>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.UpdateCurrentUserApplicationRoleConnectionMetadata, applicationId),
        }, cancellationToken);

        #endregion
        #region Entitlement

        public async ValueTask<DiscordApiResponse<IReadOnlyList<DiscordEntitlement>>> ListEntitlementsAsync(DiscordSnowflake applicationId, DiscordListEntitlementQueryParameterModel queryParameters, CancellationToken cancellationToken = default) => await SendAsync<IReadOnlyList<DiscordEntitlement>>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.ListEntitlements, applicationId)
                .WithQueryParameter("user_id", queryParameters.UserId)
                .WithQueryParameter("sku_ids", queryParameters.SkuIds)
                .WithQueryParameter("before", queryParameters.Before)
                .WithQueryParameter("after", queryParameters.After)
                .WithQueryParameter("limit", queryParameters.Limit)
                .WithQueryParameter("guild_id", queryParameters.GuildId)
                .WithQueryParameter("exclude_ended", queryParameters.ExcludeEnded)
                .WithQueryParameter("exclude_deleted", queryParameters.ExcludeDeleted)
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<DiscordEntitlement>> GetEntitlementAsync(DiscordSnowflake applicationId, DiscordSnowflake entitlementId, CancellationToken cancellationToken = default) => await SendAsync<DiscordEntitlement>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.GetEntitlement, applicationId, entitlementId)
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<object?>> ConsumeEntitlementAsync(DiscordSnowflake applicationId, DiscordSnowflake entitlementId, CancellationToken cancellationToken = default) => await SendAsync<object?>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.ConsumeEntitlement, applicationId, entitlementId)
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<DiscordEntitlement>> CreateTestEntitlementAsync(DiscordSnowflake applicationId, DiscordCreateTestEntitlementJsonParameterModel jsonParameters, CancellationToken cancellationToken = default) => await SendAsync<DiscordEntitlement>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.CreateTestEntitlement, applicationId),
            Body = jsonParameters
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<object?>> DeleteTestEntitlementAsync(DiscordSnowflake applicationId, DiscordSnowflake entitlementId, CancellationToken cancellationToken = default) => await SendAsync<object?>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.DeleteTestEntitlement, applicationId, entitlementId)
        }, cancellationToken);

        #endregion
        #region SKU

        public async ValueTask<DiscordApiResponse<IReadOnlyList<DiscordSku>>> ListSkusAsync(DiscordSnowflake applicationId, CancellationToken cancellationToken = default) => await SendAsync<IReadOnlyList<DiscordSku>>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.ListSkus, applicationId)
        }, cancellationToken);

        #endregion
        #region Subscription

        public async ValueTask<DiscordApiResponse<IReadOnlyList<DiscordSubscription>>> ListSkuSubscriptionsAsync(DiscordSnowflake skuId, DiscordListSubscriptionQueryParameterModel queryParameters, CancellationToken cancellationToken = default) => await SendAsync<IReadOnlyList<DiscordSubscription>>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.ListSkuSubscriptions, skuId)
                .WithQueryParameter("before", queryParameters.Before)
                .WithQueryParameter("after", queryParameters.After)
                .WithQueryParameter("limit", queryParameters.Limit)
                .WithQueryParameter("user_id", queryParameters.UserId)
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<DiscordSubscription>> GetSkuSubscriptionAsync(DiscordSnowflake skuId, DiscordSnowflake subscriptionId, CancellationToken cancellationToken = default) => await SendAsync<DiscordSubscription>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.GetSkuSubscription, skuId, subscriptionId)
        }, cancellationToken);

        #endregion
        #region Gateway

        public async ValueTask<DiscordApiResponse<DiscordGatewayInformation>> GetGatewayAsync(CancellationToken cancellationToken = default) => await SendAsync<DiscordGatewayInformation>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.GetGateway)
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<DiscordGatewayBotInformation>> GetGatewayBotAsync(CancellationToken cancellationToken = default) => await SendAsync<DiscordGatewayBotInformation>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.GetGatewayBot)
        }, cancellationToken);

        #endregion
    }
}
