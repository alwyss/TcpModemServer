using System.ComponentModel.Composition;
using Framework.Utilities.Helpers;

namespace TcpModemServer.Notifications
{
    [Export(typeof(INotificationSender))]
    public class TcpDataNotificationSender : NotificationSender, ITcpDataNotificationSender
    {
        [ImportingConstructor]
        public TcpDataNotificationSender(IEventBus eventBus, IResponseSender responseSender) : base(eventBus, responseSender)
        {
        }

        protected override string NotificationCommand { get { return "+KTCP_DATA:"; } }

        public override DataProtocol Type
        {
            get {return DataProtocol.TCP;}
        }

        protected override object DataReceivedEventKey
        {
            get { return EventKeys.TcpDataReceived; }
        }

        protected override object[] GetHeaderArguments(ReceivedData receivedData)
        {
            return new object[]
            {
                NotificationCommand, receivedData.SessionId,  receivedData.Length
            };
        }

        protected override string HeaderFormat
        {
            get { return "{0}{1},{2},"; }
        }
    }
}
