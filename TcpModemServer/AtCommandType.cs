using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpModemServer
{
    public enum AtCommandType
    {
        None,
        SetupContext,
        SetupTcp,
        ConnectTcp,
        SendTcp,
    }
}
