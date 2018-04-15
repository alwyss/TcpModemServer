using System;

namespace TcpModemServer
{
    public interface IDataSession : IDisposable
    {
        int Id { get; }
        int Port { get; set; }
        string Host { get; set; }
        int BytesToWrite { get; set; }
        int BytesSent { get; set; }
        bool Opened { get; }
        DataProtocol Protocol { get; }
        void Close();
        bool Send(byte[] data);
        int Receive(byte[] data);
        void EndSend();
        void BeginSend(int numData);
        bool Connect();
    }
}