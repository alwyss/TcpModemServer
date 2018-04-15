using System.ComponentModel.Composition;

namespace TcpModemServer.CommandHandling
{
    [Export(typeof(ICommandHandler))]
    class TcpSetupCommand : AtComand
    {
        private readonly ISessionManager _sessionManager;
        private readonly ISessionIdPool _sessionIdPool;

        private TcpNotifCode _tcpNotif = TcpNotifCode.OK;

        [ImportingConstructor]
        public TcpSetupCommand(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public override string CommandText { get { return "AT+KTCPCFG"; } }

        protected override bool Handle(CommandInfo commandInfo)
        {
            int start = commandInfo.ArgsPos;
            int contextId;
            string command = commandInfo.CommandLine;
            if (!ParseItem(command, ref start, out contextId)) return false;

            int mode;
            if (!ParseItem(command, ref start, out mode)) return false;

            string remoteAddress;
            if (!ParseItem(command, ref start, out remoteAddress)) return false;
            remoteAddress = Unescape(remoteAddress);
           
            int remotePort;
            if (!ParseItem(command, ref start, out remotePort)) return false;

            int sourcePort;
            if (!ParseItem(command, ref start, out sourcePort))
            {
                sourcePort = 0;
            }

            int dataMode;
            if (!ParseItem(command, ref start, out dataMode))
            {
                dataMode = 0;
            }

            int enableUrc;
            if (!ParseItem(command, ref start, out enableUrc))
            {
                enableUrc = 1;
            }

            var session = CreateSession(DataProtocol.TCP) as ITcpSession;
            if (session == null) return false;

            var id = session.Setup(contextId, (TcpMode) mode, remoteAddress, remotePort, sourcePort, dataMode == 1,
                enableUrc == 1);
            SendResponse("+KTCPCFG: " + id);

            return true;
        }

        public override bool HasData { get { return false; } }
    }
}
