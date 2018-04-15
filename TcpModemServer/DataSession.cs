using System;
using System.Diagnostics;
using System.Net.Sockets;
using Framework.Utilities.Helpers;
using Framework.Utilities.Tracing;
using ProviderBacnet;
using TcpModemServer.CommandHandling;
using TcpModemServer.Sessions;
using Util.NETMF.Conversions;

namespace TcpModemServer
{
    public abstract class DataSession : IDataSession
    {
        private readonly AtomicFlag _opened = new AtomicFlag(false);
        private readonly AtomicFlag _connecting = new AtomicFlag(false);
        private Socket _socket;
        private readonly ISocketDataReceiver _socketDataReceiver;
        private readonly object _lock = new object();

        protected DataSession(int id, IEventBus eventBus, INotifSender notifSender, ISocketDataReceiver socketDataReceiver)
        {
            this.Id = id;
            EventBus = eventBus;
            NotifSender = notifSender;
            _socketDataReceiver = socketDataReceiver;
        }

        public void Dispose()
        {
            Close();
        }

        public int Id { get; private set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public int BytesToWrite { get; set; }
        public int BytesSent { get; set; }

        protected IEventBus EventBus { get; private set; }
        public INotifSender NotifSender { get; set; }

        public IpAddrFamily IpAddrFamily { get; set; }

        public bool DataMode { get; set; }

        public bool Opened
        {
            get { return _opened.Check(); }
        }

        public abstract DataProtocol Protocol { get; }

        public void Close()
        {
            if (!_opened.CheckAndReset()) return;
            if (Id == 1)
            {
                Debug.Print("Close session 1");
            }
            else if (Id == 2)
            {
                Debug.Print("Close session 2");
            }
            else if (Id == 3)
            {
                Debug.Print("Close session 3");
            }
            else if (Id == 4)
            {
                Debug.Print("Close session 4");
            }
            else if (Id == 5)
            {
                Debug.Print("Close session 5");
            }
            var socket = _socket;
            _socket = null;
            if (socket != null && socket.Connected)
            {
                try
                {
                    socket.Close();
                }
                catch (Exception ex)
                {
                    TraceUtil.Error("Close session error", ex);
                }
            }
        }

        public void EndSend()
        {
            if (BytesToWrite > BytesSent)
            {
                TraceUtil.Warn("Data sent ok but more or less data are expected. Session Id: {0}, Protocol: {1}, Bytes to send: {2}, Bytes sent: {3}", Id, Protocol, BytesToWrite, BytesSent);
                NotifSender.SendNotif(Id, WaitMoreOrLessDataCode, Protocol);
            }
            //BytesSent = 0;
            //BytesToWrite = 0;
        }

        public void BeginSend(int length)
        {
            BytesToWrite = length;
            BytesSent = 0;
        }

        public bool Connect()
        {
            if (Opened) return true;

            using (var runonce= CriticalSectionHelper.RunOnce(_connecting))
            {
                if (!runonce.Run) return true;

                _socket = CreateSocket();
                try
                {
                    _socket.Connect(Host, Port);
                }
                catch (Exception ex)
                {
                    TraceUtil.Error("Socket Connect error", ex);
                    NotifSender.SendNotif(Id, NetworkErrorCode, Protocol);
                    return false;
                }

                _socketDataReceiver.Receive(_socket, DataReceivedHandler, Close);
            }

            _opened.Set();

            return true;
        }

        public int Receive(byte[] data)
        {
            return _socket.Receive(data, SocketFlags.None);
        }

        public virtual bool Send(byte[] data)
        {
            if (!Opened)
            {
                NotifSender.SendNotif(Id, NetworkErrorCode, Protocol);
                return false;
            }

            BytesSent += data.Length;
            _socket.Send(data);

            TraceUtil.Info("Data sent to remote({0}: {1}): {2} {3}", Host, Port, data.ToHexString(0, data.Length),
                data.InterpretAsString(0, data.Length));

            return true;
        }

        protected abstract int NetworkErrorCode { get; }
        protected abstract int WaitMoreOrLessDataCode { get; }

        protected abstract void DataReceivedHandler(byte[] data, int length);

        protected abstract Socket CreateSocket();
    }
}
