namespace TcpModemServer.Notifications
{
    public class ReceivedData
    {
        public ReceivedData(int sessionId, int length, byte[] data, string remoteAddr, int remotePort)
        {
            SessionId = sessionId;
            Length = length;
            Data = data;
            RemoteAddr = remoteAddr;
            RemotePort = remotePort;
        }

        public int SessionId { get; private set; }

        public int Length { get; private set; }

        public byte[] Data { get; private set; }

        public string RemoteAddr { get; private set; }

        public int RemotePort { get; private set; }
    }
}
