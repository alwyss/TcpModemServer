using System;
using System.Text;

namespace Util.NETMF.Conversions
{
    public static class HexHelper
    {
        public static byte[] HexToBytes(string hexString)
        {
            // Based on http://stackoverflow.com/a/3974535
            if (hexString.Length == 0 || hexString.Length % 2 != 0)
                return new byte[0];

            byte[] buffer = new byte[hexString.Length / 2];
            char c;
            for (int bx = 0, sx = 0; bx < buffer.Length; ++bx, ++sx)
            {
                // Convert first half of byte 
                c = hexString[sx];
                byte b = (byte)((c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0')) << 4);

                // Convert second half of byte 
                c = hexString[++sx];
                b |= (byte)(c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0'));
                buffer[bx] = b;
            }

            return buffer;
        }

        public static string InterpretAsString(this byte[] bytes)
        {
            return InterpretAsString(bytes, 0, bytes.Length);
        }

        public static string InterpretAsString(this byte[] bytes, int offset, int length)
        {
            var str = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var b = bytes[offset + i];
                if (b >= 32 && b <= 126)
                    str.Append((char) b);
                else
                    str.AppendFormat("\\{0}", (int) b);
            }

            return str.ToString();
        }

        public static string ToHexString(this byte[] bytes, int offset, int length, int groupSize = 1, string separator = " ")
        {
            return BytesToHex(bytes, offset, length, groupSize, separator);
        }

        public static string BytesToHex(byte[] bytes, int offset, int length, int groupSize = 1, string separator = " ")
        {
            if (bytes == null) return string.Empty;

            char[] result = new char[length * 2 + ((length + groupSize - 1) / groupSize -1) * separator.Length];
            int resultIndex = 0;
            int j = 0;
            for (int i = 0; i < length; i++)
            {
                if (j >= groupSize)
                {
                    foreach (char cc in separator)
                    {
                        result[resultIndex++] = cc;
                    }

                    j = 0;
                }
                j++;

                //hex.AppendFormat("{0:x2}", b)
                var b = bytes[offset + i] >> 4;
                result[resultIndex++] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = bytes[offset + i] & 0xF;
                result[resultIndex++] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }
            return new string(result);
        }
    }
}