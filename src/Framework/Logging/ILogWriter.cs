using System;
using System.Runtime.CompilerServices;

namespace Lacjam.Framework.Logging
{
    /// <summary>
    /// Interface that log writers should implement.
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        void Error(int eventId, string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs the specified error message and exception.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Error(int eventId, string message, Exception exception, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs the specified fatal error message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Fatal(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs the specified fatal error message.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        void Fatal(int eventId, string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs the specified fatal error message and exception.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Fatal(int eventId, string message, Exception exception, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>m>
        /// <param name="message">The message.</param>
        void Warn(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        void Warn(int eventId, string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs the specified warning message and exception.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Warn(int eventId, string message, Exception exception, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        void Info(int eventId, string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        void Debug(int eventId, string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

    }

}
