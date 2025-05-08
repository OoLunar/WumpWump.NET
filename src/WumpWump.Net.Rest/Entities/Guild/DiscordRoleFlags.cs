using System;

namespace WumpWump.Net.Rest.Entities
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
