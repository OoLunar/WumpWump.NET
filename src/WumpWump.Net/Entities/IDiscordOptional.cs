namespace WumpWump.Net.Entities
{
    /// <summary>
    /// A non-generic interface for <see cref="DiscordOptional{T}"/>.
    /// </summary>
    public interface IDiscordOptional
    {
        /// <inheritdoc cref="DiscordOptional{T}.Value"/>
        public bool HasValue { get; }

        /// <inheritdoc cref="DiscordOptional{T}.Value"/>
        public object? Value { get; }
    }
}
