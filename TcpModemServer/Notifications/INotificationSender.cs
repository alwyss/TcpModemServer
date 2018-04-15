using System;

namespace TcpModemServer.Notifications
{
    public interface INotificationSender : IDisposable
    {
        void Start();
        DataProtocol Type { get; }
    }
}