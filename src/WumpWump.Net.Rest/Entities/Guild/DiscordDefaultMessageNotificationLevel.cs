namespace WumpWump.Net.Rest.Entities
{
    public enum DiscordDefaultMessageNotificationLevel
    {
        /// <summary>
        /// members will receive notifications for all messages by default
        /// </summary>
        AllMessages = 0,

        /// <summary>
        /// members will receive notifications only for messages that @mention them by default
        /// </summary>
        OnlyMentions = 1
    }
}
