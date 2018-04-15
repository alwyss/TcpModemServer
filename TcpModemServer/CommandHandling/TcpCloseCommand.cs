using System.ComponentModel.Composition;

namespace TcpModemServer.CommandHandling
{
    [Export(typeof(ICommandHandler))]
    class TcpCloseCommand : AtComand
    {
        public override string CommandText { get { return "AT+KTCPCLOSE"; } }
        protected override bool Handle(CommandInfo commandInfo)
        {
            string command = commandInfo.CommandLine;
            int start = commandInfo.ArgsPos;

            var sessionId = 0;
            if (!ParseItem(command, ref start, out sessionId)) return false;

            var closeingType = 1;
            if (!ParseItem(command, ref start, out closeingType)) return false;

            var tcpSession = GetTcpSession(sessionId);
            if (tcpSession == null) return false;


            tcpSession.Close();

            SendResponse("OK");

            return true;
        }

        public override bool HasData { get { return false; } }
    }
}
