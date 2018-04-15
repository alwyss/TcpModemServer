using System;
using System.ComponentModel.Composition;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

//[assembly: XmlConfigurator(Watch = true)]
namespace Framework.Utilities.Tracing
{
    [Export(typeof(log4net.Core.ILogger))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Log4netLogger : ILogger
    {
        private string _loggerName;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4netLogger"/> class.
        /// </summary>
        public Log4netLogger()
        {
            _loggerName = Assembly.GetEntryAssembly().FullName;
        }

        #endregion

        #region Methods

        public void Initialize(string loggerName, TracingOutput tracingOutput, bool debugMode)
        {
            _loggerName = loggerName;
            var hierarchy = (Hierarchy)LogManager.GetRepository();

            if (tracingOutput.HasFlag(TracingOutput.File))
            {
                PatternLayout patternLayout = new PatternLayout();
                patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
                patternLayout.ActivateOptions();

                RollingFileAppender roller = new RollingFileAppender();

                roller.AppendToFile = true;
                roller.File = "Logs\\" + loggerName + "\\";
                roller.Layout = patternLayout;
                roller.ImmediateFlush = true;
                roller.DatePattern = "yyyyMM/yyyy-MM-dd'.log'";
                roller.RollingStyle = RollingFileAppender.RollingMode.Date;
                roller.StaticLogFileName = false;
                roller.ActivateOptions();
                hierarchy.Root.AddAppender(roller);
            }

            if (tracingOutput.HasFlag(TracingOutput.Console))
            {
                PatternLayout patternLayout = new PatternLayout();
                patternLayout.ConversionPattern = "%date %-5level:  %message%newline";
                patternLayout.ActivateOptions();

                var appender = new ColoredConsoleAppender();
                appender.AddMapping(CreateColorMapping(Level.Fatal, ColoredConsoleAppender.Colors.Purple | ColoredConsoleAppender.Colors.HighIntensity));
                appender.AddMapping(CreateColorMapping(Level.Error, ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity));
                appender.AddMapping(CreateColorMapping(Level.Warn, ColoredConsoleAppender.Colors.Yellow | ColoredConsoleAppender.Colors.HighIntensity));
                appender.AddMapping(CreateColorMapping(Level.Info, ColoredConsoleAppender.Colors.Green | ColoredConsoleAppender.Colors.HighIntensity));
                appender.AddMapping(CreateColorMapping(Level.Debug, ColoredConsoleAppender.Colors.Blue | ColoredConsoleAppender.Colors.HighIntensity));

                appender.Layout = patternLayout;
                appender.ActivateOptions();
                hierarchy.Root.AddAppender(appender);
            }

            if (debugMode)
            {
                hierarchy.Root.Level = Level.Debug;
            }
            else
            {
                hierarchy.Root.Level = Level.Info;
            }

            hierarchy.Configured = true;
        }

        private static ColoredConsoleAppender.LevelColors CreateColorMapping(Level level, ColoredConsoleAppender.Colors foreColor)
        {
            var errorColorMapping = new ColoredConsoleAppender.LevelColors()
            {
                ForeColor = foreColor,
                Level = level
            };
            return errorColorMapping;
        }

        /// <summary>
        /// Log the debug message
        /// </summary>
        /// <param name="message">
        /// The message
        /// </param>
        public void Debug(object message)
        {
            GetLogger().Debug(message);
        }

        private ILog GetLogger()
        {
            var logger = LogManager.GetLogger(_loggerName);
            return logger;
        }

        /// <summary>
        /// Log the debug message
        /// </summary>
        /// <param name="message">
        /// The message
        /// </param>
        /// <param name="e">
        /// The exception
        /// </param>
        public void Debug(object message, Exception e)
        {
            LogManager.GetLogger(_loggerName).Debug(message, e);
        }

        /// <summary>
        /// Log the info message
        /// </summary>
        /// <param name="message">The message</param>
        public void Info(object message)
        {
            LogManager.GetLogger(_loggerName).Info(message);
        }

        /// <summary>
        /// Log the info message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">The exception</param>
        public void Info(object message, Exception e)
        {
            LogManager.GetLogger(_loggerName).Info(message, e);
        }

        /// <summary>
        /// Log the warning message
        /// </summary>
        /// <param name="message">The message</param>
        public void Warn(object message)
        {
            LogManager.GetLogger(_loggerName).Warn(message);
        }

        /// <summary>
        /// Log the warning message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">The exception</param>
        public void Warn(object message, Exception e)
        {
            LogManager.GetLogger(_loggerName).Warn(message, e);
        }

        /// <summary>
        /// Log the error message
        /// </summary>
        /// <param name="message">The message</param>
        public void Error(object message)
        {
            ILog log = LogManager.GetLogger(_loggerName);
            log.Error(message);
        }

        /// <summary>
        /// Log the error message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">The exception</param>
        public void Error(object message, Exception e)
        {
            LogManager.GetLogger(_loggerName).Error(message, e);
        }

        /// <summary>
        /// Log the fatal message
        /// </summary>
        /// <param name="message">The message</param>
        public void Fatal(object message)
        {
            LogManager.GetLogger(_loggerName).Fatal(message);
        }

        /// <summary>
        /// Log the fatal message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">The exception</param>
        public void Fatal(object message, Exception e)
        {
            LogManager.GetLogger(_loggerName).Fatal(message, e);
        }

        public void Cleanup()
        {
            LogManager.Shutdown();
        }

        #endregion Methods

        public void Flush()
        {
            ILog log = LogManager.GetLogger(_loggerName);
            var logger = log.Logger as Logger;
            if (logger != null)
            {
                foreach (IAppender appender in logger.Appenders)
                {
                    var buffered = appender as BufferingAppenderSkeleton;
                    if (buffered != null)
                    {
                        buffered.Flush();
                    }
                }
            }
        }
    }
}
