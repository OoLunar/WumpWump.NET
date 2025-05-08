namespace WumpWump.Net.Rest.Entities
{
    public enum DiscordAutoModerationEventType
    {
        /// <summary>
        /// when a member sends or edits a message in the guild
        /// </summary>
        MessageSend = 1,

        /// <summary>
        /// when a member edits their profile
        /// </summary>
        MemberUpdate = 2
    }
}
