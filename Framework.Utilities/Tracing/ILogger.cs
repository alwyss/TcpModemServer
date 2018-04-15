using System;

namespace Framework.Utilities.Tracing
{
    /// <summary>
    /// Defines operations of a logger used to logs all kinds of information.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log the debug message
        /// </summary>
        /// <param name="message">
        /// The message
        /// </param>
        void Debug(object message);

        /// <summary>
        /// Log the debug message
        /// </summary>
        /// <param name="message">
        /// The message
        /// </param>
        /// <param name="e">
        /// The exception
        /// </param>
        void Debug(object message, Exception e);

        /// <summary>
        /// Log the info message
        /// </summary>
        /// <param name="message">The message</param>
        void Info(object message);

        /// <summary>
        /// Log the info message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">The exception</param>
        void Info(object message, Exception e);

        /// <summary>
        /// Log the warning message
        /// </summary>
        /// <param name="message">The message</param>
        void Warn(object message);

        /// <summary>
        /// Log the warning message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">The exception</param>
        void Warn(object message, Exception e);

        /// <summary>
        /// Log the error message
        /// </summary>
        /// <param name="message">The message</param>
        void Error(object message);

        /// <summary>
        /// Log the error message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">The exception</param>
        void Error(object message, Exception e);

        /// <summary>
        /// Log the fatal message
        /// </summary>
        /// <param name="message">The message</param>
        void Fatal(object message);

        /// <summary>
        /// Log the fatal message
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="e">The exception</param>
        void Fatal(object message, Exception e);

        void Flush();
    }
}
