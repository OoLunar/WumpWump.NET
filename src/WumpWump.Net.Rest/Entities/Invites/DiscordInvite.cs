using System;

namespace WumpWump.Net.Rest.Entities
{
    public record DiscordInvite
    {
        public required DiscordInviteType Type { get; init; }
        public required string Code { get; init; }
        public DiscordOptional<DiscordGuild> Guild { get; init; }
        public required DiscordChannel? Channel { get; init; }
        public DiscordOptional<DiscordUser> Inviter { get; set; }
        public DiscordOptional<DiscordInviteTargetType> TargetType { get; init; }
        public DiscordOptional<DiscordUser> TargetUser { get; init; }
        public DiscordOptional<DiscordApplication> TargetApplication { get; init; }
        public DiscordOptional<int> ApproximatePresenceCount { get; init; }
        public DiscordOptional<int> ApproximateMemberCount { get; init; }
        public DiscordOptional<DateTimeOffset?> ExpiresAt { get; init; }

#pragma warning disable CS0618 // Type or member is obsolete
        public DiscordOptional<DiscordInviteStageInstance> StageInstance { get; init; }
#pragma warning restore CS0618 // Type or member is obsolete

        //public DiscordOptional<DiscordGuildScheduledEvent> GuildScheduledEvent { get; init; }
    }
}
