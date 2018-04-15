using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utilities.Helpers
{
    public class StringHelper
    {
        public static string GetString(byte[] data, int len)
        {
            var sb = new char[len];
            for (int i = 0; i < len; i++)
            {
                sb[i] = (char)data[i];
            }

            return new string(sb);
        }
    }
}
