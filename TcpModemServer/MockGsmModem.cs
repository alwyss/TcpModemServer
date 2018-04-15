using System;
using System.ComponentModel.Composition;
using Framework.Utilities.Helpers;
using TcpModemServer.CommandHandling;
using TcpModemServer.Notifications;

namespace TcpModemServer
{
    public interface IGsmModem : IDisposable
    {
        void Start();
    }

    [Export(typeof(IGsmModem))]
    public class MockGsmModem : IGsmModem
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly IResponseSender _responseSender;
        private readonly IEventBus _eventBus;
        private readonly IModemListener _listener;
        private readonly INotificationManager _notificationManager;
        private readonly Action _modemOpenedHandler;
        private readonly Action<byte[], int> _dataReceivedHandler;

        [ImportingConstructor]
        public MockGsmModem(ICommandDispatcher dispatcher, IResponseSender responseSender, IEventBus eventBus,
            IModemListener listener, INotificationManager notificationManager)
        {
            _dispatcher = dispatcher;
            _responseSender = responseSender;
            _eventBus = eventBus;
            _listener = listener;
            _notificationManager = notificationManager;
            _modemOpenedHandler = OnModemOpen;
            _dataReceivedHandler = OnDataReceived;
            _eventBus.Add(EventKeys.ModemOpened, _modemOpenedHandler);
            _eventBus.Add(EventKeys.ModemDataReceived, _dataReceivedHandler);
        }

        private void OnModemOpen()
        {
            SendGsmState();
            SendGprsState();
        }

        public void Start()
        {
            _notificationManager.Start();
            _listener.Open();
        }

        private void OnDataReceived(byte[] data, int len)
        {
            _dispatcher.Dispatch(data, len);
        }

        private void SendGprsState()
        {
            _responseSender.SendResponse("+CREG: 5");
        }

        private void SendGsmState()
        {
            _responseSender.SendResponse("+CGREG: 5");
        }

        public void Dispose()
        {
            _eventBus.Remove(EventKeys.ModemOpened, _modemOpenedHandler);
            _eventBus.Remove(EventKeys.ModemDataReceived, _dataReceivedHandler);
        }
    }
}
