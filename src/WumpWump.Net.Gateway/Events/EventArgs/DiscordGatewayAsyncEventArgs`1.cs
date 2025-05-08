namespace WumpWump.Net.Gateway.Events.EventArgs
{
    public class DiscordGatewayAsyncEventArgs<T> : DiscordGatewayAsyncEventArgs
    {
        public T? Data => Payload.Data == default ? default : (T)Payload.Data;
    }
}
