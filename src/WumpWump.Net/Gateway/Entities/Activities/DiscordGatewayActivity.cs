using System;
using System.Collections.Generic;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Gateway.Entities
{
    /// <remarks>
    /// Bot users are only able to set <see cref="Name"/>, <see cref="State"/>, <see cref="Type"/>, and <see cref="Url"/>.
    /// </remarks>
    public readonly record struct DiscordGatewayActivity
    {
        /// <summary>
        /// Activity's name
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// <see cref="DiscordGatewayActivityType"/>
        /// </summary>
        public required DiscordGatewayActivityType Type { get; init; }

        /// <summary>
        /// Stream URL, is validated when <see cref="Type"/> is <see cref="DiscordGatewayActivityType.Streaming"/>
        /// </summary>
        public DiscordOptional<Uri?> Url { get; init; }

        /// <summary>
        /// Unix timestamp (in milliseconds) of when the activity was added to the user's session
        /// </summary>
        public required long CreatedAt { get; init; }

        /// <summary>
        /// Unix timestamps for start and/or end of the game
        /// </summary>
        public DiscordOptional<DiscordGatewayActivityTimestamps> Timestamps { get; init; }

        /// <summary>
        /// Application ID for the game
        /// </summary>
        public DiscordOptional<DiscordSnowflake> ApplicationId { get; init; }

        /// <summary>
        /// What the player is currently doing
        /// </summary>
        public DiscordOptional<string?> Details { get; init; }

        /// <summary>
        /// User's current party status, or text used for a custom status
        /// </summary>
        public DiscordOptional<string?> State { get; init; }

        /// <summary>
        /// Emoji used for a custom status
        /// </summary>
        public DiscordOptional<DiscordGatewayActivityEmoji?> Emoji { get; init; }

        /// <summary>
        /// Information for the current party of the player
        /// </summary>
        public DiscordOptional<DiscordGatewayActivityParty> Party { get; init; }

        /// <summary>
        /// Images for the presence and their hover texts
        /// </summary>
        public DiscordOptional<DiscordGatewayActivityAssets> Assets { get; init; }

        /// <summary>
        /// Secrets for Rich Presence joining and spectating
        /// </summary>
        public DiscordOptional<DiscordGatewayActivitySecrets> Secrets { get; init; }

        /// <summary>
        /// Whether or not the activity is an instanced game session
        /// </summary>
        public DiscordOptional<bool> Instance { get; init; }

        /// <summary>
        /// <see cref="DiscordGatewayActivityFlags"/> <c>OR</c>d together, describes what the payload includes
        /// </summary>
        public DiscordOptional<DiscordGatewayActivityFlags> Flags { get; init; }

        /// <summary>
        /// Custom buttons shown in the Rich Presence (max 2)
        /// </summary>
        public DiscordOptional<IReadOnlyList<DiscordGatewayActivityButton>> Buttons { get; init; }
    }
}