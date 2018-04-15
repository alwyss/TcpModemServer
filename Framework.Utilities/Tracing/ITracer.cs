using System;

namespace Framework.Utilities.Tracing
{
    public interface ITracer : IDisposable
    {
        TracerType TracerType { get; }
        void Error(string message, Exception exception);
        void Fatal(string message, params object[] args);
        void Error(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Info(string message, params object[] args);
        void Debug(string message, params object[] args);
        void Initialize(string appName, TracingOutput tracingOutput, bool debugMode);
        void Flush();
    }
}
