namespace WumpWump.Net.Gateway.Entities
{
    /// <summary>
    /// All gateway events in Discord are tagged with an opcode that denotes the payload type.
    /// </summary>
    public enum DiscordGatewayOpCode
    {
        /// <summary>
        /// An event was dispatched.
        /// </summary>
        /// <remarks>
        /// Can only be received by the client.
        /// </remarks>
        Dispatch = 0,

        /// <summary>
        /// Fired periodically by the client to keep the connection alive.
        /// </summary>
        /// <remarks>
        /// Can be sent or received by the client.
        /// </remarks>
        Heartbeat = 1,

        /// <summary>
        /// Starts a new session during the initial handshake.
        /// </summary>
        /// <remarks>
        /// Can only be sent by the client.
        /// </remarks>
        Identify = 2,

        /// <summary>
        /// Update the client's presence.
        /// </summary>
        /// <remarks>
        /// Can only be sent by the client.
        /// </remarks>
        PresenceUpdate = 3,

        /// <summary>
        /// Used to join/leave or move between voice channels.
        /// </summary>
        /// <remarks>
        /// Can only be sent by the client.
        /// </remarks>
        VoiceStateUpdate = 4,

        /// <summary>
        /// Resume a previous session that was disconnected.
        /// </summary>
        /// <remarks>
        /// Can only be sent by the client.
        /// </remarks>
        Resume = 6,

        /// <summary>
        /// You should attempt to reconnect and resume immediately.
        /// </summary>
        /// <remarks>
        /// Can only be received by the client.
        /// </remarks>
        Reconnect = 7,

        /// <summary>
        /// Request information about offline guild members in a large guild.
        /// </summary>
        /// <remarks>
        /// Can only be sent by the client.
        /// </remarks>
        RequestGuildMembers = 8,

        /// <summary>
        /// The session has been invalidated. You should reconnect and identify/resume accordingly.
        /// </summary>
        /// <remarks>
        /// Can only be received by the client.
        /// </remarks>
        InvalidSession = 9,

        /// <summary>
        /// Sent immediately after connecting, contains the `heartbeat_interval` to use.
        /// </summary>
        /// <remarks>
        /// Can only be received by the client.
        /// </remarks>
        Hello = 10,

        /// <summary>
        /// Sent in response to receiving a heartbeat to acknowledge that it has been received.
        /// </summary>
        /// <remarks>
        /// Can only be received by the client.
        /// </remarks>
        HeartbeatACK = 11,

        /// <summary>
        /// Request information about soundboard sounds in a set of guilds.
        /// </summary>
        /// <remarks>
        /// Can only be sent by the client.
        /// </remarks>
        RequestSoundboardSounds = 31
    }
}