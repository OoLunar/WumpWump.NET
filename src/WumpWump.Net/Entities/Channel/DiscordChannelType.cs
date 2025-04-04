namespace WumpWump.Net.Entities
{
    public enum DiscordChannelType
    {
        /// <summary>
        /// a text channel within a server
        /// </summary>
        GuildText = 0,

        /// <summary>
        /// a direct message between users
        /// </summary>
        Dm = 1,

        /// <summary>
        /// a voice channel within a server
        /// </summary>
        GuildVoice = 2,

        /// <summary>
        /// a direct message between multiple users
        /// </summary>
        GroupDm = 3,

        /// <summary>
        /// an <a href="https://support.discord.com/hc/en-us/articles/115001580171-Channel-Categories-101">organizational category</a> that contains up to 50 channels
        /// </summary>
        GuildCategory = 4,

        /// <summary>
        /// a channel that <a href="https://support.discord.com/hc/en-us/articles/360032008192">users can follow and crosspost into their own server</a> (formerly news channels)
        /// </summary>
        GuildAnnouncement = 5,

        /// <summary>
        /// a temporary sub-channel within a <see cref="GuildAnnouncement"/> channel
        /// </summary>
        AnnouncementThread = 10,

        /// <summary>
        /// a temporary sub-channel within a <see cref="GuildText"/> or <see cref="GuildForum"/> channel
        /// </summary>
        PublicThread = 11,

        /// <summary>
        /// a temporary sub-channel within a <see cref="GuildText"/> channel that is only viewable by those invited and those with the <see cref="DiscordPermission.ManageThreads"/> permission
        /// </summary>
        PrivateThread = 12,

        /// <summary>
        /// a voice channel for <a href="https://support.discord.com/hc/en-us/articles/1500005513722">hosting events with an audience</a>
        /// </summary>
        GuildStageVoice = 13,

        /// <summary>
        /// the channel in a <a href="https://support.discord.com/hc/en-us/articles/4406046651927-Discord-Student-Hubs-FAQ">hub</a> containing the listed servers
        /// </summary>
        GuildDirectory = 14,

        /// <summary>
        /// Channel that can only contain threads
        /// </summary>
        GuildForum = 15,

        /// <summary>
        /// Channel that can only contain threads, similar to <see cref="GuildForum"/> channels
        /// </summary>
        GuildMedia = 16
    }
}
