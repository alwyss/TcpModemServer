using System;
using Framework.Utilities.Helpers;

namespace TcpModemServer.Notifications
{
    public abstract class NotificationSender : INotificationSender
    {
        private readonly IEventBus _eventBus;
        private readonly IResponseSender _responseSender;
        private Action<ReceivedData> _dataReceived;

        protected NotificationSender(IEventBus eventBus, IResponseSender responseSender)
        {
            _eventBus = eventBus;
            _responseSender = responseSender;
        }

        protected abstract string NotificationCommand { get; }

        public void Start()
        {
            _dataReceived = OnDataReceived;
            _eventBus.Add(DataReceivedEventKey, _dataReceived);
        }

        public abstract DataProtocol Type { get; }

        protected abstract object DataReceivedEventKey { get; }

        private void OnDataReceived(ReceivedData receivedData)
        {
            var responseFormat = HeaderFormat;
            var arguments = GetHeaderArguments(receivedData);
            var header = string.Format(responseFormat, arguments);
            var trailer = "\r\n";

            var dataToSend = new byte[header.Length + receivedData.Length + trailer.Length];
            var pos = 0;
            for (int i = 0; i < header.Length; i++)
            {
                dataToSend[pos++] = (byte)header[i];
            }

            for (int i = 0; i < receivedData.Length; i++)
            {
                dataToSend[pos++] = receivedData.Data[i];
            }

            for (int i = 0; i < trailer.Length; i++)
            {
                dataToSend[pos++] = (byte)trailer[i];
            }

            _responseSender.SendData(dataToSend);

        }

        protected abstract object[] GetHeaderArguments(ReceivedData receivedData);
        //private void OnDataReceived(ReceivedData receivedData)
        //{
        //    int sessionId = receivedData.SessionId;
        //    byte[] data = receivedData.Data;
        //    int len = receivedData.Length;
        //    string host = receivedData.RemoteAddr;
        //    int port = receivedData.RemotePort;
        //    var responseFormat = ResponseFormat;
        //    var response = NotificationCommand + sessionId + "," + len + ",";
        //    var arguments = GetArguments(sessionId, data, len);

        //    var dataToSend = new byte[response.Length + len+2];
        //    var pos = 0;
        //    for (int i = 0; i < response.Length; i++)
        //    {
        //        dataToSend[pos++] = (byte) response[i];
        //    }

        //    for (int i = 0; i < len; i++)
        //    {
        //        dataToSend[pos++] = (byte) data[i];
        //    }

        //    dataToSend[pos++] = 0xd;
        //    dataToSend[pos] = 0xa;

        //    _responseSender.SendData(dataToSend);
        //}

        protected abstract string HeaderFormat { get; }

        public void Dispose()
        {
            if(_dataReceived != null)
                _eventBus.Remove(DataReceivedEventKey, _dataReceived);
        }
    }
}
