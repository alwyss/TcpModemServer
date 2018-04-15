namespace TcpModemServer
{
    public interface ISessionManager
    {
        IDataSession Add(DataProtocol protocol);
        IDataSession GetSession(int sessionId);
        void Remove(int sessionId);
        TcpSession GetSession();
    }
}