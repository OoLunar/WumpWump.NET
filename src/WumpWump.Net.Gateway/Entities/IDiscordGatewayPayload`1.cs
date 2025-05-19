using System.Text.Json.Serialization;

namespace WumpWump.Net.Gateway.Entities
{
    /// <inheritdoc />
    public interface IDiscordGatewayPayload<T> : IDiscordGatewayPayload
    {
        /// <inheritdoc cref="IDiscordGatewayPayload.Data"/>
        [JsonPropertyName("d")]
        new T Data { get; init; }

        object? IDiscordGatewayPayload.Data { get => Data; init => Data = (T)value!; }
    }
}
