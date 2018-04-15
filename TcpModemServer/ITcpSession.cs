namespace TcpModemServer
{
    public interface ITcpSession : IDataSession 
    {
        bool Urc { get; set; }

        int Setup(int contextId, TcpMode mode, string remoteAddress, int remotePort, int sourcePort,
            bool dataMode, bool enableUrc);
    }
}