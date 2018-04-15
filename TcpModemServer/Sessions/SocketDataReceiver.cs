using System;
using System.ComponentModel.Composition;
using System.Net.Sockets;
using Framework.Utilities.Helpers;
using Framework.Utilities.Tracing;

namespace TcpModemServer.Sessions
{
    public interface ISocketDataReceiver
    {
        void Receive(Socket socket, Action<byte[], int> dataReceivedHandler, Action closeHandler);
    }

    [Export(typeof(ISocketDataReceiver))]
    public class SocketDataReceiver : ISocketDataReceiver
    {
        private IEventBus _eventBus;

        [ImportingConstructor]
        public SocketDataReceiver(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private class ReceiveData
        {
            public ReceiveData(Socket socket, byte[] data, Action<byte[], int> dataReceivedHandler, Action closeHandler)
            {
                Socket = socket;
                Data = data;
                DataReceivedHandler = dataReceivedHandler;
                CloseHandler = closeHandler;
            }

            public Socket Socket { get; private set; }
            public byte[] Data { get; private set; }
            public Action<byte[], int> DataReceivedHandler { get; private set; }
            public Action CloseHandler { get; set; }
        }

        public void Receive(Socket socket, Action<byte[], int> dataReceivedHandler, Action closeHandler)
        {
            var count = socket.Available == 0 ? 256 : socket.Available;
            var data = new byte[count];
            var receiveData = new ReceiveData(socket, data, dataReceivedHandler, closeHandler);
            socket.BeginReceive(data, 0, count, SocketFlags.None, ReceiveCallback, receiveData);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var receiveData = ar.AsyncState as ReceiveData;
            var data = receiveData.Data;
            var socket = receiveData.Socket;
            var closeHandler = receiveData.CloseHandler;
            int readLen=0;
            try
            {
                SocketError socketError = SocketError.Success;
                if(socket.Connected)
                    readLen = socket.EndReceive(ar, out socketError);
                if (socketError != SocketError.Success)
                {
                    TraceUtil.Error("End receive data error: {0}", socketError);
                    Close(socket, closeHandler);
                    return;
                }
            }
            catch (Exception ex)
            {
                TraceUtil.Error("End receive data error", ex);
                Close(socket, closeHandler);
                return;
            }

            if (readLen == 0)
            {
                Close(socket, closeHandler);
                return;
            }

            receiveData.DataReceivedHandler(data, readLen);

            Receive(socket, receiveData.DataReceivedHandler, closeHandler);
        }

        private void Close(Socket socket, Action closeHandler)
        {
            //if (socket != null && socket.Connected)
            //{
            //    socket.Close();
            //}

            closeHandler();
        }
    }
}
