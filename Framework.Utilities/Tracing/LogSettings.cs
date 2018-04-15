using System.IO;
using System.Reflection;

namespace Framework.Utilities.Tracing
{
    public class LogSettings
    {
        public LogSettings()
        {
            LoggingBaseFolder = Path.Combine(Assembly.GetExecutingAssembly().Location, "Logs");
        }

        public static string LoggingBaseFolder { get; set; }
    }
}
