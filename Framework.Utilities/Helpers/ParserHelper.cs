using System;
using System.Reflection;

namespace Framework.Utilities.Helpers
{
    public class ParserHelper
    {
        public static bool TryParse<T>(string input, out T result)
        {
            if (typeof(T) == typeof(string))
            {
                result = (T)(object)input;
                return true;
            }

            result = default(T);
            if (string.IsNullOrEmpty(input))
                return false;

            var tryParseMethod = typeof(T).GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, null,
                new Type[] { typeof(string), typeof(T).MakeByRefType() },
                null);
            if (tryParseMethod == null)
            {
                return false;
            }

            var parameters = new object[] { input, null };
            var success = (bool)tryParseMethod.Invoke(null, parameters);
            result = (T)parameters[1];

            return success;
        }
    }
}
