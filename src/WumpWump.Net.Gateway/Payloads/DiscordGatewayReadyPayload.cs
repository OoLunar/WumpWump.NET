using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Entities
{
    /// <summary>
    /// The ready event is dispatched when a client has completed the initial handshake with the gateway (for new sessions).
    /// The ready event can be the largest and most complex event the gateway will send, as it contains all the state required
    /// for a client to begin interacting with the rest of the platform.
    /// </summary>
    /// <remarks>
    /// <see cref="Guilds"/> are the guilds of which your bot is a member. They start out as unavailable when
    /// you connect to the gateway. As they become available, your bot will be notified via Guild Create events.
    /// </remarks>
    [DiscordGatewayEvent(DiscordGatewayOpCode.Dispatch, "ready")]
    public record DiscordGatewayReadyPayload
    {
        /// <summary>
        /// <a href="https://discord.com/developers/docs/reference#api-versioning-api-versions">API version</a>
        /// </summary>
        [JsonPropertyName("v")]
        public required int Version { get; init; }

        /// <summary>
        /// Information about the user including email
        /// </summary>
        public required DiscordUser User { get; init; }

        /// <summary>
        /// Guilds the user is in
        /// </summary>
        public required IReadOnlyList<DiscordUnavailableGuild> Guilds { get; init; }

        /// <summary>
        /// Used for resuming connections
        /// </summary>
        public required string SessionId { get; init; }

        /// <summary>
        /// Gateway URL for resuming connections
        /// </summary>
        public required Uri ResumeGatewayUrl { get; init; }

        /// <summary>
        /// <a href="https://discord.com/developers/docs/events/gateway#sharding">Shard information</a> associated with this session, if sent when identifying
        /// </summary>
        public DiscordOptional<DiscordGatewayShardInformation> Shard { get; init; }

        /// <summary>
        /// Contains <see cref="DiscordApplication.Id"/> and <see cref="DiscordApplication.Flags"/>
        /// </summary>
        public required DiscordUnavailableApplication Application { get; init; }
    }
}
