using System;
using System.ComponentModel.Composition;

namespace TcpModemServer
{
    public interface INotifSender
    {
        void SendTcpNotif(int sessionId, TcpNotifCode tcpNotif);
        void SendUdpNotif(int sessionId, UdpNotifCode udpNotif);
        void SendNotif(int sessionId, int notifCode, DataProtocol protocol);
    }

    [Export(typeof(INotifSender))]
    public class NotifSender : INotifSender
    {
        private readonly IResponseSender _responseSender;

        [ImportingConstructor]
        public NotifSender(IResponseSender responseSender)
        {
            _responseSender = responseSender;
        }

        public void SendTcpNotif(int sessionId, TcpNotifCode tcpNotif)
        {
            _responseSender.SendResponse("+KTCP_NOTIF:" + sessionId + "," + (int)tcpNotif);
        }

        public void SendUdpNotif(int sessionId, UdpNotifCode udpNotif)
        {
            _responseSender.SendResponse("+KUDP_NOTIF:" + sessionId + "," + (int)udpNotif);
        }

        public void SendNotif(int sessionId, int notifCode, DataProtocol protocol)
        {
            if (notifCode < 0) return;

            if(protocol == DataProtocol.TCP) SendTcpNotif(sessionId, (TcpNotifCode)notifCode);
            else if(protocol == DataProtocol.UDP) SendUdpNotif(sessionId, (UdpNotifCode)notifCode);
            else throw new NotSupportedException("Protocol not supported: " + protocol);
        }
    }
}
