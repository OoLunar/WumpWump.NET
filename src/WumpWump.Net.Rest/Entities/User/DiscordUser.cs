using System.Drawing;

namespace WumpWump.Net.Rest.Entities
{
    public record DiscordUser
    {
        /// <summary>
        /// the user's id
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// the user's username, not unique across the platform
        /// </summary>
        public required string Username { get; init; }

        /// <summary>
        /// the user's Discord-tag
        /// </summary>
        public required string Discriminator { get; init; }

        /// <summary>
        /// the user's display name, if it is set. For bots, this is the application name
        /// </summary>
        public required string? GlobalName { get; init; }

        /// <summary>
        /// the user's <a href="https://discord.com/developers/docs/reference#image-formatting">avatar hash</a>
        /// </summary>
        public required string? Avatar { get; init; }

        /// <summary>
        /// whether the user belongs to an OAuth2 application
        /// </summary>
        public DiscordOptional<bool> Bot { get; init; }

        /// <summary>
        /// whether the user is an Official Discord System user (part of the urgent message system)
        /// </summary>
        public DiscordOptional<bool> System { get; init; }

        /// <summary>
        /// whether the user has two factor enabled on their account
        /// </summary>
        public DiscordOptional<bool> MfaEnabled { get; init; }

        /// <summary>
        /// the user's <a href="https://discord.com/developers/docs/reference#image-formatting">banner hash</a>
        /// </summary>
        public DiscordOptional<string?> Banner { get; init; }

        /// <summary>
        /// the user's banner color encoded as an integer representation of hexadecimal color code
        /// </summary>
        public DiscordOptional<Color?> AccentColor { get; init; }

        /// <summary>
        /// the user's chosen <a href="https://discord.com/developers/docs/reference#locales">language option</a>
        /// </summary>
        public DiscordOptional<string> Locale { get; init; }

        /// <summary>
        /// whether the email on this account has been verified
        /// </summary>
        /// <remarks>
        /// Requires the <c>email</c> <a href="https://discord.com/developers/docs/topics/oauth2">OAuth2 scope</a>
        /// </remarks>
        public DiscordOptional<bool> Verified { get; init; }

        /// <summary>
        /// the user's email
        /// </summary>
        /// <remarks>
        /// Requires the <c>email</c> <a href="https://discord.com/developers/docs/topics/oauth2">OAuth2 scope</a>
        /// </remarks>
        public DiscordOptional<string?> Email { get; init; }

        /// <summary>
        /// the <see cref="DiscordUserFlags"/> on a user's account
        /// </summary>
        public DiscordOptional<DiscordUserFlags> Flags { get; init; }

        /// <summary>
        /// the type of <see cref="DiscordUserPremiumType"/> on a user's account
        /// </summary>
        public DiscordOptional<DiscordUserPremiumType> PremiumType { get; init; }

        /// <summary>
        /// the public <see cref="DiscordUserFlags"/> on a user's account
        /// </summary>
        public DiscordOptional<DiscordUserFlags> PublicFlags { get; init; }

        /// <summary>
        /// data for the user's avatar decoration
        /// </summary>
        public DiscordOptional<DiscordUserAvatarDecorationData?> AvatarDecorationData { get; init; }
    }
}
