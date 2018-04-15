using System;

namespace Framework.Utilities.Tracing
{
    public class TraceUtil
    {
        private static ITracer _tracer;

        public static void SetTracer(ITracer tracer)
        {
            _tracer = tracer;
        }

        public static void Error(string message, Exception exception)
        {
            _tracer.Error(message, exception);
        }

        public static void Error(string message, params object[] args)
        {
            _tracer.Error(message, args);
        }

        public static void Warn(string message, params object[] args)
        {
            _tracer.Warn(message, args);
        }

        public static void Info(string message, params object[] args)
        {
            _tracer.Info(message, args);
        }

        public static void Debug(string message, params object[] args)
        {
            _tracer.Debug(message, args);
        }

        public static void Fatal(string message, params object[] args)
        {
            _tracer.Fatal(message, args);
        }

        public static void Flush()
        {
            _tracer.Flush();
        }
    }
}
