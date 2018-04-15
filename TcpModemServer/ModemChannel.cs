using System;
using System.ComponentModel.Composition;
using System.Net.Sockets;
using System.Threading.Tasks;
using Framework.Utilities.Helpers;
using Framework.Utilities.Tracing;
using TcpModemServer.Sessions;
using Util.NETMF.Conversions;

namespace TcpModemServer
{
    public interface IModemChannel : IDisposable
    {
        void Send(byte[] data);
    }

    [Export(typeof(IModemChannel))]
    public class ModemChannel : IModemChannel
    {
        private readonly IEventBus _eventBus;
        private readonly ISocketDataReceiver _socketDataReceiver;
        private Socket _connectSocket;
        private readonly Action<Socket> _dceOpenedDelegate;

        [ImportingConstructor]
        public ModemChannel(IEventBus eventBus, ISocketDataReceiver socketDataReceiver)
        {
            _eventBus = eventBus;
            _socketDataReceiver = socketDataReceiver;
            _dceOpenedDelegate = OnDceConnect;
            _eventBus.Add(EventKeys.ChannelOpened, _dceOpenedDelegate);
        }

        private void OnDceConnect(Socket socket)
        {
            if (_connectSocket != null)
            {
                TraceUtil.Warn("Modem is already opened.");

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return;
            }

            TraceUtil.Info("Modem is opened successfully.");
            _connectSocket = socket;
            _socketDataReceiver.Receive(_connectSocket, OnDataReceived, CloseSocket);
            _eventBus.Raise(EventKeys.ModemOpened);
        }

        public void Send(byte[] data)
        {
            if(_connectSocket != null) _connectSocket.Send(data);
        }

        private void CloseSocket()
        {
            var socket = _connectSocket;
            if (socket != null && socket.Connected)
            {
                socket.Close();
            }

            if (socket != null)
            {
                _connectSocket = null;
                _eventBus.Raise(EventKeys.ChannelClosed);
            }
        }

        private void OnDataReceived(byte[] data, int len)
        {
            TraceUtil.Info("{0} data received: {1} {2}", len, data.ToHexString(0, len), data.InterpretAsString(0, len));
            _eventBus.Raise(EventKeys.ModemDataReceived, data, len);
        }

        public void Dispose()
        {
            _eventBus.Remove(EventKeys.ChannelOpened, _dceOpenedDelegate);
            CloseSocket();
        }
    }
}
