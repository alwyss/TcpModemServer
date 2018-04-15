using System.ComponentModel.Composition;

namespace TcpModemServer.CommandHandling
{
    [Export(typeof(ICommandHandler))]
    public class TcpSendCommand : AtComand
    {
        private const string Eofpattern = "--EOF--Pattern--";
        private readonly ISessionManager _sessionManager;
        private readonly IDteDataSender _dteDataSender;
        private int _tcpNotif;
        private ITcpSession _session;

        [ImportingConstructor]
        public TcpSendCommand(ISessionManager sessionManager, IDteDataSender dteDataSender)
        {
            _sessionManager = sessionManager;
            _dteDataSender = dteDataSender;
        }

        public override string CommandText { get { return "AT+KTCPSND"; } }

        protected override bool Handle(CommandInfo commandInfo)
        {
            string command = commandInfo.CommandLine;
            int start = commandInfo.ArgsPos;
            var sessionId = 0;
            if (!ParseItem(command, ref start, out sessionId)) return false;
            var numData = 0;
            if (!ParseItem(command, ref start, out numData)) return false;
            _session = GetTcpSession(sessionId);
            if (_session == null) return false;

            SendResponse("Connect");
            _session.BeginSend(numData);
            return true;
        }

        public override bool HasData { get { return true; } }

        protected override bool OnHandleData(string request, ref int pos)
        {
            var done = _dteDataSender.SendData(_session, request, ref pos);
            if (done) _session = null;
            return done;
        }
    }
}
