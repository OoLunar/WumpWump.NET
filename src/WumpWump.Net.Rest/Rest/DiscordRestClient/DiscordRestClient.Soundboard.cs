using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WumpWump.Net.Rest.Entities;
using WumpWump.Net.Rest.PostModels;

namespace WumpWump.Net.Rest
{
    public partial class DiscordRestClient : IDiscordRestClient
    {
        /// <summary>
        /// Send a soundboard sound to a voice channel the user is connected to. Fires a
        /// <a href="https://discord.com/developers/docs/events/gateway-events#voice-channel-effect-send">Voice Channel Effect Send</a>
        /// Gateway event.
        /// </summary>
        /// <remarks>
        /// Requires the <see cref="DiscordPermission.Speak"/> and <see cref="DiscordPermission.UseSoundboard"/> permissions, and also
        /// the <see cref="DiscordPermission.UseExternalSounds"/> permission if the sound is from a different server. Additionally,
        /// requires the user to be connected to the voice channel, having a voice state without <see cref="DiscordMember.Deaf"/>,
        /// self_deaf, <see cref="DiscordMember.Mute"/>, or suppress enabled.
        /// </remarks>
        /// <param name="channelId">The channel ID to send the sound to.</param>
        /// <param name="soundId">the id of the soundboard sound to play</param>
        /// <param name="sourceGuildId">the id of the guild the soundboard sound is from, required to play sounds from different servers</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        // TODO: Use DiscordMemberVoiceState for remarks
        public async ValueTask<DiscordApiResponse<object?>> SendSoundboardSoundAsync(DiscordSnowflake channelId, DiscordSnowflake soundId, DiscordOptional<DiscordSnowflake> sourceGuildId = default, CancellationToken cancellationToken = default)
            => await SendAsync<object?>(new()
            {
                Method = HttpMethod.Post,
                Route = new Uri("https://discord.com/api/v10/channels/:channel_id/send-soundboard-sound"),
                Url = new Uri($"https://discord.com/api/v10/channels/{channelId}/send-soundboard-sound"),
                Body = new DiscordSoundboardSoundSendModel()
                {
                    SoundId = soundId,
                    SourceGuildId = sourceGuildId
                }
            }, cancellationToken);

        public async ValueTask<DiscordApiResponse<IReadOnlyList<DiscordSoundboardSound>>> ListDefaultSoundboardSoundsAsync(CancellationToken cancellationToken = default)
            => await SendAsync<IReadOnlyList<DiscordSoundboardSound>>(new()
            {
                Method = HttpMethod.Get,
                Route = new Uri("https://discord.com/api/v10/soundboard-sounds"),
                Url = new Uri("https://discord.com/api/v10/soundboard-sounds")
            }, cancellationToken);

        /// <summary>
        /// Returns a list of the guild's soundboard sounds. Includes <see cref="DiscordSoundboardSound.User"/> fields if the bot has the <see cref="DiscordPermission.CreateGuildExpressions"/> or <see cref="DiscordPermission.ManageGuildExpressions"/> permission.
        /// </summary>
        /// <param name="guildId">The guild ID to get the soundboard sounds from.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<IReadOnlyList<DiscordSoundboardSound>>> ListGuildSoundboardSoundsAsync(DiscordSnowflake guildId, CancellationToken cancellationToken = default)
            => await SendAsync<IReadOnlyList<DiscordSoundboardSound>>(new()
            {
                Method = HttpMethod.Get,
                Route = new Uri("https://discord.com/api/v10/guilds/:guild_id/soundboard-sounds"),
                Url = new Uri($"https://discord.com/api/v10/guilds/{guildId}/soundboard-sounds")
            }, cancellationToken);

        /// <summary>
        /// Returns a soundboard sound object for the given sound id. Includes the <see cref="DiscordSoundboardSound.User"/> field if the bot has the <see cref="DiscordPermission.CreateGuildExpressions"/> or <see cref="DiscordPermission.ManageGuildExpressions"/> permission.
        /// </summary>
        /// <param name="guildId">The guild ID to get the soundboard sound from.</param>
        /// <param name="soundId">The sound ID to get the soundboard sound from.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordSoundboardSound>> GetGuildSoundboardSoundAsync(DiscordSnowflake guildId, DiscordSnowflake soundId, CancellationToken cancellationToken = default)
            => await SendAsync<DiscordSoundboardSound>(new()
            {
                Method = HttpMethod.Get,
                Route = new Uri("https://discord.com/api/v10/guilds/:guild_id/soundboard-sounds/:sound_id"),
                Url = new Uri($"https://discord.com/api/v10/guilds/{guildId}/soundboard-sounds/{soundId}")
            }, cancellationToken);

        /// <summary>
        /// Create a new soundboard sound for the guild. Requires the <see cref="DiscordPermission.CreateGuildExpressions"/> permission. Returns the new <see cref="DiscordSoundboardSound"/> object on success. Fires a <a href="https://discord.com/developers/docs/events/gateway-events#guild-soundboard-sound-create">Guild Soundboard Sound Create</a> Gateway event.
        /// </summary>
        /// <remarks>
        /// Soundboard sounds have a max file size of 512kb and a max duration of 5.2 seconds.
        /// </remarks>
        /// <param name="guildId">The guild ID to create the soundboard sound in.</param>
        /// <param name="createModel">The model to create the soundboard sound with.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordSoundboardSound>> CreateGuildSoundboardSoundAsync(DiscordSnowflake guildId, DiscordSoundboardSoundCreateModel createModel, CancellationToken cancellationToken = default)
            => await SendAsync<DiscordSoundboardSound>(new()
            {
                Method = HttpMethod.Post,
                Route = new Uri("https://discord.com/api/v10/guilds/:guild_id/soundboard-sounds"),
                Url = new Uri($"https://discord.com/api/v10/guilds/{guildId}/soundboard-sounds"),
                Body = createModel
            }, cancellationToken);

        /// <summary>
        /// Modify the given soundboard sound. For sounds created by the current user, requires either the <see cref="DiscordPermission.CreateGuildExpressions"/>
        /// or <see cref="DiscordPermission.ManageGuildExpressions"/> permission. For other sounds, requires the <see cref="DiscordPermission.ManageGuildExpressions"/>
        /// permission. Returns the updated <see cref="DiscordSoundboardSound"/> object on success. Fires a <a href="https://discord.com/developers/docs/resources/soundboard#soundboard-sound-object">Guild Soundboard Sound Update</a> Gateway event.
        /// </summary>
        /// <param name="guildId">The guild ID to modify the soundboard sound in.</param>
        /// <param name="soundId">The sound ID to modify.</param>
        /// <param name="editModel">The model to edit the soundboard sound with.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<DiscordSoundboardSound>> ModifyGuildSoundboardSoundAsync(DiscordSnowflake guildId, DiscordSnowflake soundId, DiscordSoundboardSoundEditModel editModel, CancellationToken cancellationToken = default)
            => await SendAsync<DiscordSoundboardSound>(new()
            {
                Method = HttpMethod.Patch,
                Route = new Uri("https://discord.com/api/v10/guilds/:guild_id/soundboard-sounds/:sound_id"),
                Url = new Uri($"https://discord.com/api/v10/guilds/{guildId}/soundboard-sounds/{soundId}"),
                Body = editModel
            }, cancellationToken);

        /// <summary>
        /// Delete the given soundboard sound. For sounds created by the current user, requires either the <see cref="DiscordPermission.CreateGuildExpressions"/> or <see cref="DiscordPermission.ManageGuildExpressions"/> permission.
        /// For other sounds, requires the <see cref="DiscordPermission.ManageGuildExpressions"/> permission. Returns <see cref="System.Net.HttpStatusCode.NoContent"/> on success. Fires a <a href="https://discord.com/developers/docs/resources/soundboard#soundboard-sound-object">Guild Soundboard Sound Update</a> Gateway event.
        /// </summary>
        /// <param name="guildId">The guild ID to delete the soundboard sound from.</param>
        /// <param name="soundId">The sound ID to delete.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the request.</param>
        public async ValueTask<DiscordApiResponse<object?>> DeleteGuildSoundboardSoundAsync(DiscordSnowflake guildId, DiscordSnowflake soundId, CancellationToken cancellationToken = default)
            => await SendAsync<object?>(new()
            {
                Method = HttpMethod.Delete,
                Route = new Uri("https://discord.com/api/v10/guilds/:guild_id/soundboard-sounds/:sound_id"),
                Url = new Uri($"https://discord.com/api/v10/guilds/{guildId}/soundboard-sounds/{soundId}")
            }, cancellationToken);
    }
}
