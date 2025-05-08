using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using WumpWump.Net.Rest.Json;
using WumpWump.Net.Rest.Json.Attributes;

namespace WumpWump.Net.Rest.Entities
{
    /// <summary>
    /// Often used in Discord API responses and requests, this class is used to represent an optional field.
    /// </summary>
    /// <remarks>
    /// While a Json field can be null, Discord enforces that a field can be missing entirely. This means that a field can be missing and/or null at any time. This struct is used to represent that.
    /// </remarks>
    [SuppressMessage("Design", "WWL0002:Type 'DiscordOptional' in 'WumpWump.Net.Entities' namespace must be declared as a record", Justification = "We override the Equals implementation.")]
    [JsonConverter(typeof(DiscordOptionalJsonConverterFactory))]
    [JsonTypeIgnore(JsonIgnoreCondition.WhenWritingDefault)]
    [DebuggerDisplay("HasValue = {HasValue}, Value = {_value}")]
    public readonly struct DiscordOptional<T> : IDiscordOptional, IEquatable<DiscordOptional<T>>
    {
        /// <summary>
        /// An <see cref="DiscordOptional{T}"/> without a value.
        /// </summary>
        public static DiscordOptional<T> Empty { get; }

        /// <summary>
        /// If the <see cref="DiscordOptional{T}"/> has a value. The value may be null.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// The value to be returned if the <see cref="DiscordOptional{T}"/> has a value.
        /// </summary>
        /// <exception cref="InvalidOperationException">If this <see cref="DiscordOptional{T}"/> has no value.</exception>
        public T Value => HasValue ? _value! : throw new InvalidOperationException("DiscordOptional<T> has no value.");

        /// <inheritdoc cref="IDiscordOptional.Value"/>
        object? IDiscordOptional.Value => Value;

        /// <summary>
        /// The internal value. If no value is provided, this will be initialized to the default value of <typeparamref name="T"/>.
        /// </summary>
        private readonly T? _value = default;

        /// <summary>
        /// Creates an empty instance of <see cref="DiscordOptional{T}"/>.
        /// </summary>
        public DiscordOptional() => HasValue = false;

        /// <summary>
        /// Creates an instance of <see cref="DiscordOptional{T}"/> with the specified value.
        /// </summary>
        public DiscordOptional(T value)
        {
            _value = value;
            HasValue = true;
        }

        /// <summary>
        /// Checks if the property has a value that isn't null.
        /// </summary>
        /// <returns>If the property has a value that isn't null.</returns>
        public bool IsDefined() => HasValue && _value != null;

        /// <summary>
        /// If a value is present, applies the provided function to it, and returns the result.
        /// </summary>
        /// <param name="ifPresentFunction">The lambda function to evaluate if there's a value present.</param>
        /// <typeparam name="TOutput">The type that the lambda function is expected to return.</typeparam>
        /// <returns>An <see cref="DiscordOptional{TOutput}"/>. Optional.HasValue will be false if the field wasn't present.</returns>
        /// <remarks>This checks if the <see cref="DiscordOptional{T}"/> has a value, not if the value is null.</remarks>
        public DiscordOptional<TOutput> IfPresent<TOutput>(Func<T, TOutput> ifPresentFunction) => HasValue ? new DiscordOptional<TOutput>(ifPresentFunction(_value!)) : DiscordOptional<TOutput>.Empty;

        /// <summary>
        /// Gets the hash code for this <see cref="DiscordOptional{T}"/>.
        /// </summary>
        /// <returns>The hash code for this <see cref="DiscordOptional{T}"/>.</returns>
        public override int GetHashCode() => HasValue ? (_value?.GetHashCode() ?? 0) : 0;

        /// <summary>
        /// Checks whether this <see cref="DiscordOptional{T}"/> is equal to another <see cref="DiscordOptional{T}"/>.
        /// </summary>
        /// <param name="other"><see cref="DiscordOptional{T}"/> to compare to.</param>
        /// <returns>Whether the <see cref="DiscordOptional{T}"/> is equal to this <see cref="DiscordOptional{T}"/>.</returns>
        public bool Equals(DiscordOptional<T> other) => HasValue
            ? (other.HasValue && EqualityComparer<T>.Default.Equals(_value, other._value))
            : !other.HasValue;

        /// <summary>
        /// Checks whether the value of this <see cref="DiscordOptional{T}"/> is equal to specified object.
        /// </summary>
        /// <param name="other">Object to compare to.</param>
        /// <returns>Whether the object is equal to the value of this <see cref="DiscordOptional{T}"/>.</returns>
        public bool Equals(T other) => HasValue && EqualityComparer<T>.Default.Equals(_value, other);

        /// <summary>
        /// Checks whether the other object is equal to this <see cref="DiscordOptional{T}"/> or it's inner value.
        /// </summary>
        public override bool Equals(object? obj) => obj switch
        {
            T otherType => Equals(otherType),
            DiscordOptional<T> otherObject => Equals(otherObject),
            _ => false
        };

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString() => HasValue ? (_value?.ToString() ?? string.Empty) : "<Empty>";

        public static implicit operator DiscordOptional<T>(T value) => new(value);
        public static implicit operator T(DiscordOptional<T> optional) => optional.Value;
        public static bool operator ==(DiscordOptional<T> opt, T t) => opt.Equals(t);
        public static bool operator !=(DiscordOptional<T> opt, T t) => !opt.Equals(t);
        public static bool operator ==(DiscordOptional<T> lhs, DiscordOptional<T> rhs) => lhs.Equals(rhs);
        public static bool operator !=(DiscordOptional<T> lhs, DiscordOptional<T> rhs) => !lhs.Equals(rhs);
    }
}
