using System.ComponentModel.Composition;

namespace TcpModemServer.CommandHandling
{
    [Export(typeof(ICommandHandler))]
    class TcpDeleteCommand : AtComand
    {
        public override string CommandText { get { return "AT+KTCPDEL"; } }
        protected override bool Handle(CommandInfo commandInfo)
        {
            string command = commandInfo.CommandLine;
            int start = commandInfo.ArgsPos;

            var sessionId = 0;
            if (!ParseItem(command, ref start, out sessionId)) return false;
            

            var session = GetTcpSession(sessionId);
            if (session == null) return false;

            if (session.Opened) return false;

            SessionManager.Remove(sessionId);

            SendResponse("OK");
            return true;
        }

        public override bool HasData { get { return false; } }
    }
}
