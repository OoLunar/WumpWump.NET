namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// A non-generic interface for <see cref="DiscordOptional{T}"/>.
    /// </summary>
    public interface IDiscordOptional
    {
        /// <inheritdoc cref="DiscordOptional{T}.Value"/>
        bool HasValue { get; }

        /// <inheritdoc cref="DiscordOptional{T}.Value"/>
        object? Value { get; }
    }
}
