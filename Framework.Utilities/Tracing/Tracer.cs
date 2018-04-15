using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Framework.Utilities.Tracing
{
    [Export(typeof(ITracer))]
    public class Tracer : ITracer
    {
        private readonly List<TraceListener> _customTraceListeners = new List<TraceListener>();

        private void InitTrace(string traceName, TracingOutput output)
        {
            Trace.AutoFlush = true;
            var folder = new DirectoryInfo("Logs\\");
            if (!folder.Exists)
            {
                folder.Create();
            }

            if (output.HasFlag(TracingOutput.File))
            {
                var streamWriter = new StreamWriter(Path.Combine(folder.FullName, traceName + ".log"), append: true);
                var fileListener = new TextWriterTraceListener(streamWriter);
                fileListener.TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime;
                _customTraceListeners.Add(fileListener);
                System.Diagnostics.Debug.Listeners.Add(fileListener);
            }

            if (output.HasFlag(TracingOutput.Console))
            {
                var consoleListener = new ColoredConsoleTraceListener();
                _customTraceListeners.Add(consoleListener);
                System.Diagnostics.Debug.Listeners.Add(consoleListener);
            }
        }

        public TracerType TracerType
        {
            get { return TracerType.Default; }
        }

        public void Initialize(string traceName, TracingOutput output, bool debugMode)
        {
            InitTrace(traceName, output);
        }

        public void Flush()
        {
            Trace.Flush();
        }

        public void Error(string message, Exception exception)
        {
            Trace.TraceError("{0}\n{1}", message, exception);
        }

        public void Fatal(string message, params object[] args)
        {
            Trace.TraceError(message, args);
        }

        public void Error(string message, params object[] args)
        {
            Trace.TraceError(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            Trace.TraceWarning(message, args);
        }

        public void Info(string message, params object[] args)
        {
            Trace.TraceInformation(message, args);
        }

        public void Debug(string message, params object[] args)
        {
            Info(message, args);
        }

        public void Dispose()
        {
            DisposeListeners();
        }

        private void DisposeListeners()
        {
            _customTraceListeners.ToList().ForEach(p =>
            {
                System.Diagnostics.Debug.Listeners.Remove(p);
                p.Dispose();
            });
        }
    }
}
