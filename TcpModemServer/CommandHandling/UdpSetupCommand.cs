using System.ComponentModel.Composition;

namespace TcpModemServer.CommandHandling
{
    [Export(typeof(ICommandHandler))]
    public class UdpSetupCommand : AtComand
    {
        public override string CommandText { get { return "AT+KUDPCFG"; } }

        protected override bool Handle(CommandInfo commandInfo)
        {
            int start = commandInfo.ArgsPos;
            int contextId;
            string command = commandInfo.CommandLine;
            if (!ParseItem(command, ref start, out contextId)) return false;

            int mode;
            if (!ParseItem(command, ref start, out mode)) return false;

            int sourcePort;
            ParseItem(command, ref start, out sourcePort, 0);

            int dataMode;
            ParseItem(command, ref start, out dataMode, 0);

            string remoteAddress;
            if(ParseItem(command, ref start, out remoteAddress, string.Empty))
                remoteAddress = Unescape(remoteAddress);

            int remotePort;
            ParseItem(command, ref start, out remotePort, 0);

            int addressFamily; //0: ipv4; 1: ipv6
            ParseItem(command, ref start, out addressFamily, 0);

            IUdpSession session = CreateSession(DataProtocol.UDP) as IUdpSession;
            if (session == null) return false;

            var id = session.Setup(contextId, (UdpMode) mode, remoteAddress, remotePort, sourcePort, dataMode == 1,
                (IpAddrFamily) addressFamily);

            SendResponse("+KUDPCFG: " + id);

            return true;
        }
    }
}
