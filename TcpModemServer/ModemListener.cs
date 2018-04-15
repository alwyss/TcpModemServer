using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Framework.Utilities.Helpers;
using Framework.Utilities.Tracing;
using ProviderBacnet;

namespace TcpModemServer
{
    public interface IModemListener : IDisposable
    {
        void Open();
    }

    [Export(typeof(IModemListener))]
    public class ModemListener : IModemListener
    {
        private readonly IEventBus _eventBus;
        private readonly AtomicFlag _opened = new AtomicFlag(false);
        private Socket _listenSocket;
        private Thread _listenThread;

        [ImportingConstructor]
        public ModemListener(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Open()
        {
            if (_opened.CheckAndSet()) return;

            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(new IPEndPoint(IPAddressHelper.ParseIPAddress("localhost"), 12345));
            _listenSocket.Listen(1);

            _listenThread = new Thread(Listen);
            _listenThread.Start();
        }

        private void Listen()
        {
            while (_opened.Check())
            {
                Socket connectSocket;
                try
                {
                    connectSocket = _listenSocket.Accept();
                }
                catch (SocketException e)
                {
                    TraceUtil.Warn("The socket is closed by another thread while listening", e);
                    return;
                }

                _eventBus.Raise(EventKeys.ChannelOpened, connectSocket);
            }
        }

        private void Run()
        {
            //this should only be called the first time.
            var listener = new TcpListener(IPAddress.Any, 1234);
            var socket = listener.AcceptSocket();
        }

        public void Dispose()
        {
            if (!_opened.CheckAndReset()) return;

            try
            {
                if(_listenSocket != null)
                    _listenSocket.Close();
                _listenThread.Join();
            }
            catch (Exception ex)
            {
                TraceUtil.Error("Close listener error", ex);
            }
        }
    }
}
