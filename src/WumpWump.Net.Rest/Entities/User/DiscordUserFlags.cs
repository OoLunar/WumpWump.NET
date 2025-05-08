namespace WumpWump.Net.Rest.Entities
{
    public enum DiscordUserFlags
    {
        /// <summary>
        /// Discord Employee
        /// </summary>
        Staff = 1 << 0,

        /// <summary>
        /// Partnered Server Owner
        /// </summary>
        Partner = 1 << 1,

        /// <summary>
        /// HypeSquad Events Member
        /// </summary>
        Hypesquad = 1 << 2,

        /// <summary>
        /// Bug Hunter Level 1
        /// </summary>
        BugHunterLevel1 = 1 << 3,

        /// <summary>
        /// House Bravery Member
        /// </summary>
        HypesquadOnlineHouse1 = 1 << 6,

        /// <summary>
        /// House Brilliance Member
        /// </summary>
        HypesquadOnlineHouse2 = 1 << 7,

        /// <summary>
        /// House Balance Member
        /// </summary>
        HypesquadOnlineHouse3 = 1 << 8,

        /// <summary>
        /// Early Nitro Supporter
        /// </summary>
        PremiumEarlySupporter = 1 << 9,

        /// <summary>
        /// User is a <see cref="DiscordTeam"/>
        /// </summary>
        TeamPseudoUser = 1 << 10,

        /// <summary>
        /// Bug Hunter Level 2
        /// </summary>
        BugHunterLevel2 = 1 << 14,

        /// <summary>
        /// Verified Bot
        /// </summary>
        VerifiedBot = 1 << 16,

        /// <summary>
        /// Early Verified Bot Developer
        /// </summary>
        VerifiedDeveloper = 1 << 17,

        /// <summary>
        /// Moderator Programs Alumni
        /// </summary>
        CertifiedModerator = 1 << 18,

        /// <summary>
        /// Bot uses only <a href="https://discord.com/developers/docs/interactions/receiving-and-responding#receiving-an-interaction">HTTP interactions</a> and is shown in the online member list
        /// </summary>
        BotHttpInteractions = 1 << 19,

        /// <summary>
        /// User is an <a href="https://support-dev.discord.com/hc/articles/10113997751447">Active Developer</a>
        /// </summary>
        ActiveDeveloper = 1 << 22
    }
}
