namespace WumpWump.Net.Gateway.Entities
{
    public enum DiscordGatewayActivityType
    {
        /// <summary>
        /// Playing <c>{name}</c>
        /// </summary>
        Playing = 0,

        /// <summary>
        /// Streaming <c>{details}</c>
        /// </summary>
        /// <remarks>
        /// The streaming type currently only supports Twitch and YouTube. Only <a href="https://twitch.tv/"/> and <a href="https://youtube.com/"/> urls will work.
        /// </remarks>
        Streaming = 1,

        /// <summary>
        /// Listening to <c>{name}</c>
        /// </summary>
        Listening = 2,

        /// <summary>
        /// Watching <c>{name}</c>
        /// </summary>
        Watching = 3,

        /// <summary>
        /// <c>{emoji}</c> <c>{state}</c>
        /// </summary>
        Custom = 4,

        /// <summary>
        /// Competing in <c>{name}</c>
        /// </summary>
        Competing = 5
    }
}