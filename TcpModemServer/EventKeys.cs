namespace TcpModemServer
{
    public class EventKeys
    {
        public static readonly object ChannelOpened = new object();
        public static readonly object ChannelClosed = new object();
        public static readonly object ModemDataReceived = new object();
        public static readonly object ModemOpened = new object();
        public static readonly object TcpDataReceived = new object();
        public static readonly object UdpDataReceived = new object();
    }
}
