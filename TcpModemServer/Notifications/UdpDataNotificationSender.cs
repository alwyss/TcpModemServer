using System.ComponentModel.Composition;
using Framework.Utilities.Helpers;

namespace TcpModemServer.Notifications
{
    [Export(typeof(INotificationSender))]
    public class UdpDataNotificationSender : NotificationSender, IUdpDataNotificationSender
    {
        [ImportingConstructor]
        public UdpDataNotificationSender(IEventBus eventBus, IResponseSender responseSender) : base(eventBus, responseSender)
        {
        }

        protected override string NotificationCommand { get { return "+KUDP_DATA:"; } }

        public override DataProtocol Type
        {
            get {return DataProtocol.UDP;}
        }

        protected override object DataReceivedEventKey { get { return EventKeys.UdpDataReceived; } }

        protected override object[] GetHeaderArguments(ReceivedData receivedData)
        {
            return new object[]
            {
                NotificationCommand, receivedData.SessionId, receivedData.Length, receivedData.RemoteAddr, receivedData.RemotePort
            };
        }

        protected override string HeaderFormat
        {
            get { return "{0}{1},{2},{3},{4},"; }
        }
    }
}
