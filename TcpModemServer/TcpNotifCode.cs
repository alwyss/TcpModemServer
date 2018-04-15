using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpModemServer
{
    public enum TcpNotifCode
    {
        OK=-1,
        NetworkError=0,
        NoAvailableSocket,
        WaitMoreOrLessData=8,
        BadSessionId=9,
        NoSessionId
    }
}
