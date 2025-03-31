using System.Threading.Tasks;

namespace WumpWump.Net.Gateway
{
    public interface IDiscordGatewayClient
    {
        ValueTask ConnectAsync();
        ValueTask DisconnectAsync();
    }
}
