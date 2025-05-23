namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Permissions are a way to limit and grant certain abilities to users in Discord.
    /// A set of base permissions can be configured at the guild level for different roles.
    /// When these roles are attached to users, they grant or revoke specific privileges within the guild.
    /// Along with the guild-level permissions, Discord also supports permission overwrites that can be
    /// assigned to individual roles or members on a per-channel basis.
    /// </summary>
    public enum DiscordPermission : int
    {
        /// <summary>
        /// Allows creation of instant invites
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        CreateInstantInvite = 0,

        /// <summary>
        /// Allows kicking members
        /// </summary>
        KickMembers = 1,

        /// <summary>
        /// Allows banning members
        /// </summary>
        BanMembers = 2,

        /// <summary>
        /// Allows all permissions and bypasses channel permission overwrites
        /// </summary>
        Administrator = 3,

        /// <summary>
        /// Allows management and editing of channels
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        ManageChannels = 4,

        /// <summary>
        /// Allows management and editing of the guild
        /// </summary>
        ManageGuild = 5,

        /// <summary>
        /// Allows for adding new reactions to messages. This permission does not apply to reacting with an existing reaction on a message.
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        AddReactions = 6,

        /// <summary>
        /// Allows for viewing of audit logs
        /// </summary>
        ViewAuditLog = 7,

        /// <summary>
        /// Allows for using priority speaker in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        PrioritySpeaker = 8,

        /// <summary>
        /// Allows the user to go live
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        Stream = 9,

        /// <summary>
        /// Allows guild members to view a channel, which includes reading messages in text channels and joining voice channels
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        ViewChannel = 10,

        /// <summary>
        /// Allows for sending messages in a channel and creating threads in a forum (does not allow sending messages in threads)
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        SendMessages = 11,

        /// <summary>
        /// Allows for sending of `/tts` messages
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        SendTtsMessages = 12,

        /// <summary>
        /// Allows for deletion of other users messages
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        ManageMessages = 13,

        /// <summary>
        /// Links sent by users with this permission will be auto-embedded
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        EmbedLinks = 14,

        /// <summary>
        /// Allows for uploading images and files
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        AttachFiles = 15,

        /// <summary>
        /// Allows for reading of message history
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        ReadMessageHistory = 16,

        /// <summary>
        /// Allows for using the `@everyone` tag to notify all users in a channel, and the `@here` tag to notify all online users in a channel
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        MentionEveryone = 17,

        /// <summary>
        /// Allows the usage of custom emojis from other servers
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        UseExternalEmojis = 18,

        /// <summary>
        /// Allows for viewing guild insights
        /// </summary>
        ViewGuildInsights = 19,

        /// <summary>
        /// Allows for joining of a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        Connect = 20,

        /// <summary>
        /// Allows for speaking in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        Speak = 21,

        /// <summary>
        /// Allows for muting members in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        MuteMembers = 22,

        /// <summary>
        /// Allows for deafening of members in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        DeafenMembers = 23,

        /// <summary>
        /// Allows for moving of members between voice channels
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        MoveMembers = 24,

        /// <summary>
        /// Allows for using voice-activity-detection in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        UseVad = 25,

        /// <summary>
        /// Allows for modification of own nickname
        /// </summary>
        ChangeNickname = 26,

        /// <summary>
        /// Allows for modification of other users nicknames
        /// </summary>
        ManageNicknames = 27,

        /// <summary>
        /// Allows management and editing of roles
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        ManageRoles = 28,

        /// <summary>
        /// Allows management and editing of webhooks
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        ManageWebhooks = 29,

        /// <summary>
        /// Allows for editing and deleting emojis, stickers, and soundboard sounds created by all users
        /// </summary>
        ManageGuildExpressions = 30,

        /// <summary>
        /// Allows members to use application commands, including slash commands and context menu commands.
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        UseApplicationCommands = 31,

        /// <summary>
        /// Allows for requesting to speak in stage channels. (_This permission is under active development and may be changed or removed._)
        /// </summary>
        /// <remarks>Used on Stage Channels</remarks>
        RequestToSpeak = 32,

        /// <summary>
        /// Allows for editing and deleting scheduled events created by all users
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        ManageEvents = 33,

        /// <summary>
        /// Allows for deleting and archiving threads, and viewing all private threads
        /// </summary>
        /// <remarks>Used on Text Channels</remarks>
        ManageThreads = 34,

        /// <summary>
        /// Allows for creating public and announcement threads
        /// </summary>
        /// <remarks>Used on Text Channels</remarks>
        CreatePublicThreads = 35,

        /// <summary>
        /// Allows for creating private threads
        /// </summary>
        /// <remarks>Used on Text Channels</remarks>
        CreatePrivateThreads = 36,

        /// <summary>
        /// Allows the usage of custom stickers from other servers
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        UseExternalStickers = 37,

        /// <summary>
        /// Allows for sending messages in threads
        /// </summary>
        /// <remarks>Used on Text Channels</remarks>
        SendMessagesInThreads = 38,

        /// <summary>
        /// Allows for using Activities (applications with the `EMBEDDED` flag)
        /// </summary>
        /// <remarks>Used on Text Channels and Voice Channels</remarks>
        UseEmbeddedActivities = 39,

        /// <summary>
        /// Allows for timing out users to prevent them from sending or reacting to messages in chat and threads, and from speaking in voice and stage channels
        /// </summary>
        ModerateMembers = 40,

        /// <summary>
        /// Allows for viewing role subscription insights
        /// </summary>
        ViewCreatorMonetizationAnalytics = 41,

        /// <summary>
        /// Allows for using soundboard in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        UseSoundboard = 42,

        /// <summary>
        /// Allows for creating emojis, stickers, and soundboard sounds, and editing and deleting those created by the current user. Not yet available to developers, <a href="https://discord.com/developers/docs/change-log#clarification-on-permission-splits-for-expressions-and-events">see changelog</a>.
        /// </summary>
        CreateGuildExpressions = 43,

        /// <summary>
        /// Allows for creating scheduled events, and editing and deleting those created by the current user. Not yet available to developers, <a href="https://discord.com/developers/docs/change-log#clarification-on-permission-splits-for-expressions-and-events">see changelog</a>.
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        CreateEvents = 44,

        /// <summary>
        /// Allows the usage of custom soundboard sounds from other servers
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        UseExternalSounds = 45,

        /// <summary>
        /// Allows sending voice messages
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        SendVoiceMessages = 46,

        /// <summary>
        /// Allows sending polls
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        SendPolls = 49,

        /// <summary>
        /// Allows user-installed apps to send public responses. When disabled, users will still be allowed to use their apps but the responses will be ephemeral. This only applies to apps not also installed to the server.
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        UseExternalApps = 50
    }
}
