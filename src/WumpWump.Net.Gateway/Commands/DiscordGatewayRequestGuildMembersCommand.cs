using System.Collections.Generic;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Rest.Entities;

namespace WumpWump.Net.Gateway.Commands
{
    /// <summary>
    /// Used to request all members for a guild or a list of guilds. When initially connecting, if you don't have the
    /// <see cref="Entities.DiscordGatewayIntents.GuildPresences"/> or <a href="https://discord.com/developers/docs/events/gateway#gateway-intents">Gateway Intent</a>,
    /// or if the guild is over 75k members, it will only send members who are in voice, plus the member for you (the connecting user). Otherwise, if
    /// a guild has over <see cref="DiscordGatewayIdentifyCommand.LargeThreshold"/> members it will only send members who are online, have a role, have a
    /// nickname, or are in a voice channel, and if it has under <see cref="DiscordGatewayIdentifyCommand.LargeThreshold"/> members, it will send all members.
    /// If a client wishes to receive additional members, they need to explicitly request them via this operation. The server will send
    /// <see cref="Payloads.DiscordGuildMembersChunk"/> events in response with up to 1000 members per chunk until all members that match the request have been sent.
    /// </summary>
    /// <remarks>
    /// Due to our privacy and infrastructural concerns with this feature, there are some limitations that apply:
    /// <list type="bullet">
    /// <item><see cref="Entities.DiscordGatewayIntents.GuildPresences"/> intent is required to set <see cref="Presences"/> to <see langword="true"/>. Otherwise, it will always be <see langword="false"/></item>
    /// <item><see cref="Entities.DiscordGatewayIntents.GuildMembers"/> intent is required to request the entire member list - (<see cref="Query"/> = <see cref="string.Empty"/>, <see cref="Limit"/> = 0 &lt;= n)</item>
    /// <item>You will be limited to requesting 1 <see cref="GuildId"/> per request</item>
    /// <item>Requesting a prefix (through <see cref="Query"/>) will return a maximum of 100 members</item>
    /// <item>Requesting <see cref="UserIds"/> will continue to be limited to returning 100 members</item>
    /// </list>
    /// </remarks>
    [DiscordGatewayEvent(DiscordGatewayOpCode.RequestGuildMembers, null)]
    public record DiscordGatewayRequestGuildMembersCommand
    {
        /// <summary>
        /// ID of the guild to get members for
        /// </summary>
        public required DiscordSnowflake GuildId { get; init; }

        /// <summary>
        /// string that username starts with, or an empty string to return all members
        /// </summary>
        /// <remarks>
        /// <see cref="Query"/> and <see cref="UserIds"/> are mutually exclusive; you can only use one of them per request.
        /// </remarks>
        public DiscordOptional<string> Query { get; init; }

        /// <summary>
        /// maximum number of members to send matching the query; a limit of 0 can be used with an empty string query to return all members
        /// </summary>
        /// <remarks>
        /// Required when using <see cref="Query"/>.
        /// </remarks>
        public required int Limit { get; init; }

        /// <summary>
        /// used to specify if we want the presences of the matched members
        /// </summary>
        public DiscordOptional<bool> Presences { get; init; }

        /// <summary>
        /// used to specify which users you wish to fetch
        /// </summary>
        /// <remarks>
        /// <see cref="Query"/> and <see cref="UserIds"/> are mutually exclusive; you can only use one of them per request.
        /// </remarks>
        public DiscordOptional<IReadOnlyList<DiscordSnowflake>> UserIds { get; init; }

        /// <summary>
        /// nonce to identify the <see cref="DiscordGuildMembersChunk"/> response
        /// </summary>
        /// <remarks>
        /// The <see cref="Nonce"/> can only be up to 32 bytes. If you send an invalid nonce it will be
        /// ignored and the reply <see cref="DiscordGuildMembersChunk.Nonce"/>(s) will not be set.
        /// </remarks>
        public DiscordOptional<string> Nonce { get; init; }
    }
}
