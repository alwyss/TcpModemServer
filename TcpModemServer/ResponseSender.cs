using System.ComponentModel.Composition;
using System.Text;
using Framework.Utilities.Tracing;
using Util.NETMF.Conversions;

namespace TcpModemServer
{
    public interface IResponseSender
    {
        void SendResponse(string response);
        void SendData(byte[] data);
        void SendTcpNotif(int sessionId, TcpNotifCode tcpNotif);
    }

    [Export(typeof(IResponseSender))]
    class ResponseSender : IResponseSender
    {
        private readonly IModemChannel _modemChannel;

        [ImportingConstructor]
        public ResponseSender(IModemChannel modemChannel)
        {
            _modemChannel = modemChannel;
        }

        public void SendResponse(string response)
        {
            TraceUtil.Info(response);
            response = "\r\n" + response + "\r\n";
            SendData(Encoding.UTF8.GetBytes(response));
        }

        public void SendData(byte[] data)
        {
            TraceUtil.Info("{0} {1}", data.ToHexString(0, data.Length), data.InterpretAsString(0, data.Length));
            _modemChannel.Send(data);
        }

        public void SendTcpNotif(int sessionId, TcpNotifCode tcpNotif)
        {
            SendResponse("+KTCP_NOTIF:"+sessionId+","+tcpNotif);
        }
    }
}
