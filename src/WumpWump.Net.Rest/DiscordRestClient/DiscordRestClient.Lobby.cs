using System.Threading;
using System.Threading.Tasks;
using WumpWump.Net.Rest.Entities;
using WumpWump.Net.Rest.JsonParameterModels;

namespace WumpWump.Net.Rest
{
    public partial class DiscordRestClient : IDiscordRestClient
    {
        #region Lobby

        public async ValueTask<DiscordApiResponse<DiscordLobby>> CreateLobbyAsync(DiscordAddMemberToLobbyJsonParameterModel createModel, CancellationToken cancellationToken = default) => await SendAsync<DiscordLobby>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.CreateLobby),
            Body = createModel
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<DiscordLobby?>> GetLobbyAsync(DiscordSnowflake lobbyId, CancellationToken cancellationToken = default) => await SendAsync<DiscordLobby?>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.GetLobby, lobbyId)
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<DiscordLobby>> ModifyLobbyAsync(DiscordSnowflake lobbyId, DiscordAddMemberToLobbyJsonParameterModel modifyModel, CancellationToken cancellationToken = default) => await SendAsync<DiscordLobby>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.ModifyLobby, lobbyId),
            Body = modifyModel
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<object?>> DeleteLobbyAsync(DiscordSnowflake lobbyId, CancellationToken cancellationToken = default) => await SendAsync<object?>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.DeleteLobby, lobbyId)
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<DiscordLobbyMember>> AddMemberToLobbyAsync(DiscordSnowflake lobbyId, DiscordSnowflake userId, DiscordAddMemberToLobbyJsonParameterModel addModel, CancellationToken cancellationToken = default) => await SendAsync<DiscordLobbyMember>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.AddMemberToLobby, lobbyId, userId),
            Body = addModel
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<object?>> RemoveMemberFromLobbyAsync(DiscordSnowflake lobbyId, DiscordSnowflake userId, CancellationToken cancellationToken = default) => await SendAsync<object?>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.RemoveMemberFromLobby, lobbyId, userId)
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<object?>> LeaveLobbyAsync(DiscordSnowflake lobbyId, CancellationToken cancellationToken = default) => await SendAsync<object?>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.LeaveLobby, lobbyId)
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<DiscordLobby>> LinkChannelToLobbyAsync(DiscordSnowflake lobbyId, DiscordModifyChannelLinkToLobbyJsonParameterModel linkModel, CancellationToken cancellationToken = default) => await SendAsync<DiscordLobby>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.ModifyChannelLinkToLobby, lobbyId),
            Body = linkModel
        }, cancellationToken);

        public async ValueTask<DiscordApiResponse<object?>> UnlinkChannelFromLobbyAsync(DiscordSnowflake lobbyId, CancellationToken cancellationToken = default) => await SendAsync<object?>(new()
        {
            Endpoint = _urlResolver.GetEndpoint(DiscordApiRoutes.ModifyChannelLinkToLobby, lobbyId)
        }, cancellationToken);

        #endregion
    }
}
