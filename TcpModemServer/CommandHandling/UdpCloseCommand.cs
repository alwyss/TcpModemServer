using System.ComponentModel.Composition;

namespace TcpModemServer.CommandHandling
{
    [Export(typeof(ICommandHandler))]
    class UdpCloseCommand : AtComand
    {
        public override string CommandText { get { return "AT+KUDPCLOSE"; } }
        protected override bool Handle(CommandInfo commandInfo)
        {
            string command = commandInfo.CommandLine;
            int start = commandInfo.ArgsPos;

            var sessionId = 0;
            if (!ParseItem(command, ref start, out sessionId)) return false;

            var keepSession = 0;
            if (!ParseItem(command, ref start, out keepSession)) return false;

            var session = GetUdpSession(sessionId);
            if (session == null) return false;


            session.Close();
            if(keepSession == 0)
                SessionManager.Remove(sessionId);

            SendResponse("OK");

            return true;
        }

        public override bool HasData { get { return false; } }
    }
}
