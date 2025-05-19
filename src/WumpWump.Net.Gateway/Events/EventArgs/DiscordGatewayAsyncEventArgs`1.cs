namespace WumpWump.Net.Gateway.Events.EventArgs
{
    public class DiscordGatewayAsyncEventArgs<T> : DiscordGatewayAsyncEventArgs
    {
        public T Data => (T)Payload.Data!;
    }
}
