namespace WumpWump.Net.Rest.Entities
{
    public enum DiscordChannelFlags
    {
        /// <summary>
        /// this thread is pinned to the top of its parent <see cref="DiscordChannelType.GuildForum"/> or <see cref="DiscordChannelType.GuildMedia"/> channel
        /// </summary>
        Pinned = 1 << 1,

        /// <summary>
        /// whether a tag is required to be specified when creating a thread in a <see cref="DiscordChannelType.GuildForum"/> or a <see cref="DiscordChannelType.GuildMedia"/> channel. Tags are specified in the <see cref="DiscordChannel.AppliedTags"/> field.
        /// </summary>
        RequireTag = 1 << 4,

        /// <summary>
        /// when set hides the embedded media download options. Available only for media channels.
        /// </summary>
        HideMediaDownloadOptions = 1 << 15
    }
}
