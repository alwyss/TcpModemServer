using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace TcpModemServer.Notifications
{
    public interface INotificationManager
    {
        void Start();
    }

    [Export(typeof(INotificationManager))]
    public class NotificationManager : INotificationManager
    {
        private readonly Dictionary<DataProtocol, INotificationSender> _notificationSenders;

        [ImportingConstructor]
        public NotificationManager([ImportMany]IEnumerable<INotificationSender> notificationSenders)
        {
            _notificationSenders = notificationSenders.ToDictionary(p => p.Type, p => p);
        }

        public void Start()
        {
            foreach (var sender in _notificationSenders.Values)
            {
                sender.Start();
            }
        }
    }
}
