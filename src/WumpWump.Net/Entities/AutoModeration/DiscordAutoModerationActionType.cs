namespace WumpWump.Net.Entities
{
    public enum DiscordAutoModerationActionType
    {
        /// <summary>
        /// blocks a member's message and prevents it from being posted. A custom explanation
        /// can be specified and shown to members whenever their message is blocked.
        /// </summary>
        BlockMessage = 1,

        /// <summary>
        /// logs user content to a specified channel
        /// </summary>
        SendAlertMessage = 2,

        /// <summary>
        /// timeout user for a specified duration
        /// </summary>
        /// <remarks>
        /// A <see cref="Timeout"/> action can only be set up for <see cref="DiscordAutoModerationTriggerType.Keyword"/>
        /// and <see cref="DiscordAutoModerationTriggerType.MentionSpam"/> rules.The <see cref="DiscordPermission.ModerateMembers"/>
        /// permission is required to use the <see cref="Timeout"/> action type.
        /// </remarks>
        Timeout = 3,

        /// <summary>
        /// prevents a member from using text, voice, or other interactions
        /// </summary>
        BlockMemberInteraction = 4
    }
}