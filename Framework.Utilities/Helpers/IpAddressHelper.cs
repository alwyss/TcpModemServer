using System;
using System.Net;
using System.Net.Sockets;

namespace Framework.Utilities.Helpers
{
    /// <summary>
    /// IPAddress Utility class
    /// </summary>
    public static class IPAddressHelper
    {
        public static IPAddress Parse(string ipString)
        {
            if (ipString == null)
            {
                return null;
            }

            ulong num = 0L;
            int num2 = 0;
            int num3 = 0;
            ulong num4 = 0xffL;
            ulong num5 = 0L;
            int num6 = ipString.Length;
            for (int i = 0; i < num6; i++)
            {
                if ((ipString[i] == '.') || (i == (num6 - 1)))
                {
                    if (((i == 0) || ((i - num2) > 3)) || (num3 > 0x18))
                    {
                        return null;
                    }
                    i = (i == (num6 - 1)) ? ++i : i;
                    int temp;
                    if (!int.TryParse(ipString.Substring(num2, i - num2), out temp))
                    {
                        return null;
                    }

                    num5 = (ulong)(temp & 0xff);
                    num += (num5 << num3) & num4;
                    num2 = i + 1;
                    num3 += 8;
                    num4 = num4 << 8;
                }
            }

            return new IPAddress((long)num);
        }

        public static IPAddress ParseIPAddress(string host)
        {
            // check if remoteHostName is a valid IP address and get it
            var ipAddress = Parse(host);

            // in this case the parameter remoteHostName isn't a valid IP address
            if (ipAddress == null)
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(host);
                if ((hostEntry != null) && (hostEntry.AddressList.Length > 0))
                {
                    // check for the first address not null
                    // it seems that with .Net Micro Framework, the IPV6 addresses aren't supported and return "null"
                    int i = 0;
                    while (hostEntry.AddressList[i] == null ||
                           hostEntry.AddressList[i].AddressFamily != AddressFamily.InterNetwork) i++;
                    ipAddress = hostEntry.AddressList[i];
                }
                else
                {
                    throw new Exception("No address found for the remote host name");
                }
            }

            return ipAddress;
        }

        /// <summary>
        /// Return AddressFamily for the IP address
        /// </summary>
        /// <param name="ipAddress">IP address to check</param>
        /// <returns>Address family</returns>
        public static AddressFamily GetAddressFamily(this IPAddress ipAddress)
        {
#if (!MF_FRAMEWORK_VERSION_V4_2 && !MF_FRAMEWORK_VERSION_V4_3 && !MF_FRAMEWORK_VERSION_V4_4)
            return ipAddress.AddressFamily;
#else
            return (ipAddress.ToString().IndexOf(':') != -1) ?
                AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
#endif
        }
    }
}
