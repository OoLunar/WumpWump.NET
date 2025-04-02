using System.Collections.Frozen;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WumpWump.Net.Entities
{
    public partial struct DiscordPermissions
    {
        private static readonly FrozenDictionary<int, string> Names;

        static DiscordPermissions()
        {
            Dictionary<int, string> names = [];
            foreach (FieldInfo field in typeof(DiscordPermissions).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType != typeof(DiscordPermissions))
                {
                    continue;
                }

                StringBuilder builder = new();
                builder.Append(field.Name[0]);
                for (int i = 1; i < field.Name.Length; i++)
                {
                    if (char.IsUpper(field.Name[i]))
                    {
                        builder.Append(' ');
                    }

                    builder.Append(field.Name[i]);
                }

                DiscordPermissions permission = (DiscordPermissions)field.GetValue(null)!;
                names[permission.GetHashCode()] = builder.ToString();
            }

            Names = names.ToFrozenDictionary();
        }

        /// <summary>
        /// No permissions. When used on a <see cref="DiscordChannelOverwrite"/>, this means that nothing
        /// is specifically allowed or denied and will fall back to @everyone permissions.
        /// </summary>
        public static readonly DiscordPermissions None = new();

        /// <summary>
        /// This permission is a combination of all permissions. It differs from <see cref="Administrator"/>
        /// because instead of having one permission set (Administrator), it has all permissions explicitly
        /// and individually set.
        /// </summary>
        public static readonly DiscordPermissions All = CreateFull();

        /// <summary>
        /// Allows creation of instant invites
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions CreateInstantInvite = Create(0);

        /// <summary>
        /// Allows kicking members
        /// </summary>
        public static readonly DiscordPermissions KickMembers = Create(1);

        /// <summary>
        /// Allows banning members
        /// </summary>
        public static readonly DiscordPermissions BanMembers = Create(2);

        /// <summary>
        /// Allows all permissions and bypasses channel permission overwrites
        /// </summary>
        public static readonly DiscordPermissions Administrator = Create(3);

        /// <summary>
        /// Allows management and editing of channels
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions ManageChannels = Create(4);

        /// <summary>
        /// Allows management and editing of the guild
        /// </summary>
        public static readonly DiscordPermissions ManageGuild = Create(5);

        /// <summary>
        /// Allows for adding new reactions to messages. This permission does not apply to reacting with an existing reaction on a message.
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions AddReactions = Create(6);

        /// <summary>
        /// Allows for viewing of audit logs
        /// </summary>
        public static readonly DiscordPermissions ViewAuditLog = Create(7);

        /// <summary>
        /// Allows for using priority speaker in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        public static readonly DiscordPermissions PrioritySpeaker = Create(8);

        /// <summary>
        /// Allows the user to go live
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        public static readonly DiscordPermissions Stream = Create(9);

        /// <summary>
        /// Allows guild members to view a channel, which includes reading messages in text channels and joining voice channels
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions ViewChannel = Create(10);

        /// <summary>
        /// Allows for sending messages in a channel and creating threads in a forum (does not allow sending messages in threads)
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions SendMessages = Create(11);

        /// <summary>
        /// Allows for sending of `/tts` messages
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions SendTtsMessages = Create(12);

        /// <summary>
        /// Allows for deletion of other users messages
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions ManageMessages = Create(13);

        /// <summary>
        /// Links sent by users with this permission will be auto-embedded
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions EmbedLinks = Create(14);

        /// <summary>
        /// Allows for uploading images and files
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions AttachFiles = Create(15);

        /// <summary>
        /// Allows for reading of message history
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions ReadMessageHistory = Create(16);

        /// <summary>
        /// Allows for using the `@everyone` tag to notify all users in a channel, and the `@here` tag to notify all online users in a channel
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions MentionEveryone = Create(17);

        /// <summary>
        /// Allows the usage of custom emojis from other servers
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions UseExternalEmojis = Create(18);

        /// <summary>
        /// Allows for viewing guild insights
        /// </summary>
        public static readonly DiscordPermissions ViewGuildInsights = Create(19);

        /// <summary>
        /// Allows for joining of a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        public static readonly DiscordPermissions Connect = Create(20);

        /// <summary>
        /// Allows for speaking in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        public static readonly DiscordPermissions Speak = Create(21);

        /// <summary>
        /// Allows for muting members in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        public static readonly DiscordPermissions MuteMembers = Create(22);

        /// <summary>
        /// Allows for deafening of members in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        public static readonly DiscordPermissions DeafenMembers = Create(23);

        /// <summary>
        /// Allows for moving of members between voice channels
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        public static readonly DiscordPermissions MoveMembers = Create(24);

        /// <summary>
        /// Allows for using voice-activity-detection in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        public static readonly DiscordPermissions UseVad = Create(25);

        /// <summary>
        /// Allows for modification of own nickname
        /// </summary>
        public static readonly DiscordPermissions ChangeNickname = Create(26);

        /// <summary>
        /// Allows for modification of other users nicknames
        /// </summary>
        public static readonly DiscordPermissions ManageNicknames = Create(27);

        /// <summary>
        /// Allows management and editing of roles
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions ManageRoles = Create(28);

        /// <summary>
        /// Allows management and editing of webhooks
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions ManageWebhooks = Create(29);

        /// <summary>
        /// Allows for editing and deleting emojis, stickers, and soundboard sounds created by all users
        /// </summary>
        public static readonly DiscordPermissions ManageGuildExpressions = Create(30);

        /// <summary>
        /// Allows members to use application commands, including slash commands and context menu commands.
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions UseApplicationCommands = Create(31);

        /// <summary>
        /// Allows for requesting to speak in stage channels. (_This permission is under active development and may be changed or removed._)
        /// </summary>
        /// <remarks>Used on Stage Channels</remarks>
        public static readonly DiscordPermissions RequestToSpeak = Create(32);

        /// <summary>
        /// Allows for editing and deleting scheduled events created by all users
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        public static readonly DiscordPermissions ManageEvents = Create(33);

        /// <summary>
        /// Allows for deleting and archiving threads, and viewing all private threads
        /// </summary>
        /// <remarks>Used on Text Channels</remarks>
        public static readonly DiscordPermissions ManageThreads = Create(34);

        /// <summary>
        /// Allows for creating public and announcement threads
        /// </summary>
        /// <remarks>Used on Text Channels</remarks>
        public static readonly DiscordPermissions CreatePublicThreads = Create(35);

        /// <summary>
        /// Allows for creating private threads
        /// </summary>
        /// <remarks>Used on Text Channels</remarks>
        public static readonly DiscordPermissions CreatePrivateThreads = Create(36);

        /// <summary>
        /// Allows the usage of custom stickers from other servers
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions UseExternalStickers = Create(37);

        /// <summary>
        /// Allows for sending messages in threads
        /// </summary>
        /// <remarks>Used on Text Channels</remarks>
        public static readonly DiscordPermissions SendMessagesInThreads = Create(38);

        /// <summary>
        /// Allows for using Activities (applications with the `EMBEDDED` flag)
        /// </summary>
        /// <remarks>Used on Text Channels and Voice Channels</remarks>
        public static readonly DiscordPermissions UseEmbeddedActivities = Create(39);

        /// <summary>
        /// Allows for timing out users to prevent them from sending or reacting to messages in chat and threads, and from speaking in voice and stage channels
        /// </summary>
        public static readonly DiscordPermissions ModerateMembers = Create(40);

        /// <summary>
        /// Allows for viewing role subscription insights
        /// </summary>
        public static readonly DiscordPermissions ViewCreatorMonetizationAnalytics = Create(41);

        /// <summary>
        /// Allows for using soundboard in a voice channel
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        public static readonly DiscordPermissions UseSoundboard = Create(42);

        /// <summary>
        /// Allows for creating emojis, stickers, and soundboard sounds, and editing and deleting those created by the current user. Not yet available to developers, [see changelog](#DOCS_CHANGE_LOG/clarification-on-permission-splits-for-expressions-and-events).
        /// </summary>
        public static readonly DiscordPermissions CreateGuildExpressions = Create(43);

        /// <summary>
        /// Allows for creating scheduled events, and editing and deleting those created by the current user. Not yet available to developers, [see changelog](#DOCS_CHANGE_LOG/clarification-on-permission-splits-for-expressions-and-events).
        /// </summary>
        /// <remarks>Used on Voice Channels and Stage Channels</remarks>
        public static readonly DiscordPermissions CreateEvents = Create(44);

        /// <summary>
        /// Allows the usage of custom soundboard sounds from other servers
        /// </summary>
        /// <remarks>Used on Voice Channels</remarks>
        public static readonly DiscordPermissions UseExternalSounds = Create(45);

        /// <summary>
        /// Allows sending voice messages
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions SendVoiceMessages = Create(46);

        /// <summary>
        /// Allows sending polls
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions SendPolls = Create(49);

        /// <summary>
        /// Allows user-installed apps to send public responses. When disabled, users will still be allowed to use their apps but the responses will be ephemeral. This only applies to apps not also installed to the server.
        /// </summary>
        /// <remarks>Used on Text Channels, Voice Channels, and Stage Channels</remarks>
        public static readonly DiscordPermissions UseExternalApps = Create(50);
    }
}
