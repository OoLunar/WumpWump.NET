using System.Collections.Generic;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Gateway.Commands
{
    /// <summary>
    /// Used to request all members for a guild or a list of guilds. When initially connecting, if you don't have the
    /// <see cref="DiscordIntents.GuildPresences"/> <a href="https://discord.com/developers/docs/events/gateway#gateway-intents">Gateway Intent</a>,
    /// or if the guild is over 75k members, it will only send members who are in voice, plus the member for you (the connecting user). Otherwise, if
    /// a guild has over <see cref="DiscordIdentifyCommand.LargeThreshold"/> members it will only send members who are online, have a role, have a
    /// nickname, or are in a voice channel, and if it has under <see cref="DiscordIdentifyCommand.LargeThreshold"/> members, it will send all members.
    /// If a client wishes to receive additional members, they need to explicitly request them via this operation. The server will send
    /// <see cref="DiscordGuildMembersChunk"/> events in response with up to 1000 members per chunk until all members that match the request have been sent.
    /// </summary>
    /// <remarks>
    /// Due to our privacy and infrastructural concerns with this feature, there are some limitations that apply:
    /// - <see cref="DiscordIntents.GuildPresences"/> intent is required to set <see cref="Presences"/> to <see langword="true"/>. Otherwise, it will always be <see langword="false"/>
    /// - <see cref="DiscordIntents.GuildMembers"/> intent is required to request the entire member list - (<see cref="Query"/> = <see cref="string.Empty"/>, <see cref="Limit"/> = 0 <= n)
    /// - You will be limited to requesting 1 <see cref="GuildId"/> per request
    /// - Requesting a prefix (through <see cref="Query"/>) will return a maximum of 100 members
    /// - Requesting <see cref="UserIds"/> will continue to be limited to returning 100 members
    public readonly record struct DiscordRequestGuildMembersCommand
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
        /// The <see cref="None"/> can only be up to 32 bytes. If you send an invalid nonce it will be
        /// ignored and the reply <see cref="DiscordGuildMembersChunk.Nonce"/>(s) will not be set.
        /// </remarks>
        public DiscordOptional<string> Nonce { get; init; }
    }
}