using TcpModemServer.CommandHandling;

namespace TcpModemServer
{
    public interface IUdpSession : IDataSession
    {
        int Setup(int contextId, UdpMode mode, string remoteAddress, int remotePort, int sourcePort, bool dataMode,
            IpAddrFamily addressFamily);
        void BeginSend(int numData, string remoteAddress, int remotePort);
    }
}