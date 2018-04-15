using System.ComponentModel.Composition;

namespace TcpModemServer.CommandHandling
{
    [Export(typeof(ICommandHandler))]
    public class TcpConnectCommand : AtComand
    {
        private readonly ISessionManager _sessionManager;

        [ImportingConstructor]
        public TcpConnectCommand(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public override string CommandText { get { return "AT+KTCPCNX"; } }
        protected override bool Handle(CommandInfo commandInfo)
        {
            int start = commandInfo.ArgsPos;
            int sessionId;
            string command = commandInfo.CommandLine;
            if (!ParseItem(command, ref start, out sessionId)) return false;

            var tcpSession = GetTcpSession(sessionId);
            if (tcpSession == null) return false;

            var isOk = tcpSession.Connect();
            if(isOk)
                SendResponse("\r\nOK\r\n");

            return isOk;
        }

        public override bool HasData { get { return false; } }
    }
}
