using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WumpWump.Net.Gateway.Entities;

namespace WumpWump.Net.Gateway.Modules
{
    public interface IDiscordGatewayMessageModule : IAsyncDisposable
    {
        JsonSerializerOptions JsonSerializerOptions { get; }

        ValueTask ConnectAsync(Uri gatewayUrl, CancellationToken cancellationToken = default);
        ValueTask DisconnectAsync(WebSocketCloseStatus? webSocketCloseStatus = null, string? webSocketCloseDescription = null, CancellationToken cancellationToken = default);
        bool IsConnected();
        ValueTask<IDiscordGatewayPayload> ReadAsync(CancellationToken cancellationToken = default);
        ValueTask WriteAsync(IDiscordGatewayPayload payload, CancellationToken cancellationToken = default);
    }
}
