using System.ComponentModel.Composition;

namespace TcpModemServer.CommandHandling
{
    [Export(typeof(ICommandHandler))]
    class UdpSendCommand : AtComand
    {
        private IUdpSession _session;
        private readonly IDteDataSender _dteDataSender;

        [ImportingConstructor]
        public UdpSendCommand(IDteDataSender dteDataSender)
        {
            _dteDataSender = dteDataSender;
        }

        public override string CommandText { get { return "AT+KUDPSND"; } }

        public override bool HasData { get { return true; } }

        protected override bool Handle(CommandInfo commandInfo)
        {
            string command = commandInfo.CommandLine;
            int start = commandInfo.ArgsPos;
            var sessionId = 0;
            if (!ParseItem(command, ref start, out sessionId)) return false;

            string remoteAddress;
            if (ParseItem(command, ref start, out remoteAddress, string.Empty))
                remoteAddress = Unescape(remoteAddress);

            int remotePort;
            ParseItem(command, ref start, out remotePort, 0);

            var numData = 0;
            if (!ParseItem(command, ref start, out numData)) return false;

            _session = GetUdpSession(sessionId);
            if (_session == null) return false;

            SendResponse("Connect");

            _session.BeginSend(numData, remoteAddress, remotePort);
            return true;
        }

        protected override bool OnHandleData(string request, ref int pos)
        {
            var done = _dteDataSender.SendData(_session, request, ref pos);
            if (done) _session = null;
            return done;
        }
    }
}
