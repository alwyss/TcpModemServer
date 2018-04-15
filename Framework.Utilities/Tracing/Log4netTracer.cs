using System;
using System.ComponentModel.Composition;

namespace Framework.Utilities.Tracing
{
    [Export(typeof(ITracer))]
    public class Log4netTracer : ITracer
    {
        private readonly Log4netLogger _logger;

        public Log4netTracer()
        {
            _logger= new Log4netLogger();
        }

        public void Dispose()
        {
            _logger.Cleanup();
        }

        public TracerType TracerType
        {
            get { return TracerType.Log4net; }
        }

        public void Initialize(string traceName, TracingOutput output, bool debugMode)
        {
            _logger.Initialize(traceName, output, debugMode);
        }

        public void Flush()
        {
            _logger.Flush();
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void Fatal(string message, params object[] args)
        {
            _logger.Fatal(string.Format(message, args));
        }

        public void Error(string message, params object[] args)
        {
            _logger.Error(string.Format(message, args));
        }

        public void Warn(string message, params object[] args)
        {
            _logger.Warn(string.Format(message, args));
        }

        public void Info(string message, params object[] args)
        {
            _logger.Info(string.Format(message, args));
        }

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(string.Format(message, args));
        }
    }
}
