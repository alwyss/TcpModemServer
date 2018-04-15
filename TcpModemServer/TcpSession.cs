using System;
using System.Net.Sockets;
using Framework.Utilities.Helpers;
using Framework.Utilities.Tracing;
using TcpModemServer.Notifications;
using TcpModemServer.Sessions;

namespace TcpModemServer
{
    public class TcpSession : DataSession, ITcpSession
    {
        public TcpSession(int sessionId, IEventBus eventBus, INotifSender notifSender, ISocketDataReceiver socketDataReceiver)
            : base(sessionId, eventBus, notifSender, socketDataReceiver)
        {
        }

        public override DataProtocol Protocol { get { return DataProtocol.TCP;} }

        public bool Urc { get; set; }

        public int Setup(int contextId, TcpMode mode, string remoteAddress, int remotePort, int sourcePort,
            bool dataMode, bool enableUrc)
        {
            Host = remoteAddress;
            Port = remotePort;
            Urc = enableUrc;
            DataMode = dataMode;

            return Id;
        }

        protected override Socket CreateSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        protected override void DataReceivedHandler(byte[] data, int length)
        {
            EventBus.Raise(EventKeys.TcpDataReceived, new ReceivedData(Id, length, data, Host, Port));
        }

        protected override int NetworkErrorCode
        {
            get { return (int) TcpNotifCode.NetworkError; }
        }

        protected override int WaitMoreOrLessDataCode
        {
            get { return (int)TcpNotifCode.WaitMoreOrLessData; }
        }
    }
}
