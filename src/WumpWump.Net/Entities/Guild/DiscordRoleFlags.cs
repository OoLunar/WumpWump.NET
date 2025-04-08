using System;

namespace WumpWump.Net.Entities
{
    [Flags]
    public enum DiscordRoleFlags
    {
        /// <summary>
        /// role can be selected by members in an onboarding prompt
        /// </summary>
        InPrompt = 1 << 0,
    }
}