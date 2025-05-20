using System.Net.Http;
using System.Text;

namespace WumpWump.Net.Rest
{
    public static partial class DiscordApiRoutes
    {
        // Lobby
        public static readonly DiscordApiEndpointKey CreateLobby = new(HttpMethod.Post, CompositeFormat.Parse("/lobbies"), CompositeFormat.Parse("/lobbies"));
        public static readonly DiscordApiEndpointKey GetLobby = new(HttpMethod.Get, CompositeFormat.Parse("/lobbies/{0}"), CompositeFormat.Parse("/lobbies/{0}"));
        public static readonly DiscordApiEndpointKey ModifyLobby = new(HttpMethod.Patch, CompositeFormat.Parse("/lobbies/{0}"), CompositeFormat.Parse("/lobbies/{0}"));
        public static readonly DiscordApiEndpointKey DeleteLobby = new(HttpMethod.Delete, CompositeFormat.Parse("/lobbies/{0}"), CompositeFormat.Parse("/lobbies/{0}"));
        public static readonly DiscordApiEndpointKey AddMemberToLobby = new(HttpMethod.Put, CompositeFormat.Parse("/lobbies/{0}/members/{1}"), CompositeFormat.Parse("/lobbies/{0}/members/{1}"));
        public static readonly DiscordApiEndpointKey RemoveMemberFromLobby = new(HttpMethod.Delete, CompositeFormat.Parse("/lobbies/{0}/members/{1}"), CompositeFormat.Parse("/lobbies/{0}/members/{1}"));
        public static readonly DiscordApiEndpointKey LeaveLobby = new(HttpMethod.Delete, CompositeFormat.Parse("/lobbies/{0}/members/@me"), CompositeFormat.Parse("/lobbies/{0}/members/@me"));
        public static readonly DiscordApiEndpointKey ModifyChannelLinkToLobby = new(HttpMethod.Patch, CompositeFormat.Parse("/lobbies/{0}/channel-linking"), CompositeFormat.Parse("/lobbies/{0}/channel-linking"));
    }
}