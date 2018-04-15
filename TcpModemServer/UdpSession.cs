using System;
using System.Net.Sockets;
using Framework.Utilities.Helpers;
using TcpModemServer.CommandHandling;
using TcpModemServer.Notifications;
using TcpModemServer.Sessions;

namespace TcpModemServer
{
    class UdpSession : DataSession, IUdpSession
    {
        public UdpSession(int id, IEventBus eventBus, INotifSender notifSender, ISocketDataReceiver socketDataReceiver)
            : base(id, eventBus, notifSender, socketDataReceiver)
        {
        }

        public int Setup(int contextId, UdpMode mode, string remoteAddress, int remotePort, int sourcePort, bool dataMode,
            IpAddrFamily addressFamily)
        {
            Mode = mode;
            Host = remoteAddress;
            Port = remotePort;
            LocalPort = sourcePort;
            DataMode = dataMode;
            IpAddrFamily = addressFamily;

            return Id;
        }

        public void BeginSend(int numData, string remoteAddress, int remotePort)
        {
            BeginSend(numData);
            if (!string.IsNullOrEmpty(remoteAddress)) Host = remoteAddress;
            if (remotePort > 0) Port = remotePort;

            if (!Opened) Connect();
        }

        public int LocalPort { get; set; }

        public UdpMode Mode { get; set; }

        public override DataProtocol Protocol { get {return DataProtocol.UDP;} }

        protected override int NetworkErrorCode { get { return (int)UdpNotifCode.NetworkError; } }
        protected override int WaitMoreOrLessDataCode { get { return (int) UdpNotifCode.WaitForMoreOrLessData; } }

        protected override void DataReceivedHandler(byte[] data, int length)
        {
            EventBus.Raise(EventKeys.UdpDataReceived, new ReceivedData(Id, length, data, Host, Port));
        }

        protected override Socket CreateSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
    }
}