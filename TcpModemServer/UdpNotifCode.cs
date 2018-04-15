namespace TcpModemServer
{
    public enum UdpNotifCode
    {
        OK=-1,
        NetworkError=0,
        NoAvailableSocket,
        MemoryProblem=2,
        DNSError=3,
        HostUnreachable=5,
        GenericError=6,
        WaitForMoreOrLessData = 8,
        BadSessionId = 9,
        SessionAlreadyRunning=10,
        AllSessionsAreUsed=11
    }
}
