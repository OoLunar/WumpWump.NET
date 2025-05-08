namespace WumpWump.Net.Rest.Entities
{
    public enum DiscordExplicitContentFilterLevel
    {
        /// <summary>
        /// media content will not be scanned
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// media content sent by members without roles will be scanned
        /// </summary>
        MembersWithoutRoles = 1,

        /// <summary>
        /// media content sent by all members will be scanned
        /// </summary>
        AllMembers = 2
    }
}
