using System;
using Framework.Utilities.Tracing;

namespace TcpModemServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new AppContext("TcpModemServer", TracingOutput.Console | TracingOutput.File,  TracerType.Log4net))
            {
                TraceUtil.Info("Starting tcp modem server...");
                var modem = container.GetService<IGsmModem>();
                modem.Start();
                Console.ReadKey();
            }
        }
    }
}
