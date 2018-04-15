using System.ComponentModel.Composition;

namespace TcpModemServer.CommandHandling
{
    public interface IDteDataSender
    {
        bool SendData(IDataSession session, string request, ref int pos);
    }

    [Export(typeof(IDteDataSender))]
    public class DteDataSender : IDteDataSender
    {
        private readonly IResponseSender _responseSender;
        private const string Eofpattern = "--EOF--Pattern--";

        [ImportingConstructor]
        public DteDataSender(IResponseSender responseSender)
        {
            _responseSender = responseSender;
        }

        public bool SendData(IDataSession session, string request, ref int pos)
        {
            var end = request.IndexOf(Eofpattern, pos);
            var begin = pos;
            var eof = true;

            if (end == -1)
            {
                eof = false;
                pos = request.Length;
                end = request.Length;
            }
            else
            {
                pos = end + Eofpattern.Length;
            }

            var data = GetData(request, begin, end);

            if (data.Length != 0)
            {
                session.Send(data);
            }

            if (eof)
            {
                session.EndSend();
                SendResponse("OK");

                return true;
            }

            return false;
        }

        private void SendResponse(string response)
        {
            _responseSender.SendResponse(response);
        }

        private static byte[] GetData(string request, int start, int end)
        {
            var data = new byte[end - start];
            if (data.Length > 0)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    data[j] = (byte)request[start + j];
                }
            }

            return data;
        }
    }
}
