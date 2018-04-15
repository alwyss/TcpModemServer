using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpModemServer
{
    public enum CommandType
    {
        None,
        Test,
        Read,
        Write,
        Execute
    }
}
