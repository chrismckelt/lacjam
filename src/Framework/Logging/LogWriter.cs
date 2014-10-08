using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Common.Logging;
using Common.Logging.Factory;
using log4net.Config;
using log4net.Core;
using log4net.Util;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;

namespace Lacjam.Framework.Logging
{
    /// <summary>
    ///     Logging helper class. Wraps log4net, but also implements an interface so that
    ///     logging can be mocked out when testing.
    /// </summary>
    public class LogWriter : ILogWriter
    {
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Type _callerStackBoundaryType;
        /// <summary>
        ///     Force log4net to load the configuration details.
        /// </summary>
        static LogWriter()
        {


            var dir = AppDomain.CurrentDomain.BaseDirectory.ToLowerInvariant();
            string file = Directory.EnumerateFiles(dir, "log4net.config", SearchOption.AllDirectories).First();

            if (!File.Exists(file))
                throw new FileNotFoundException(file);

            XmlConfigurator.ConfigureAndWatch(new FileInfo(file));
        }


        /// <summary>
        ///     Logs the specified error message.
        /// </summary>
        /// <param name="message">The message.</param>
        [DebuggerStepThrough]
        public void Error(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Error(EventIds.Error, message, null, callerName,sourceFilePath,sourceLineNumber);
        }

        /// <summary>
        ///     Logs the specified error message.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        [DebuggerStepThrough]
        public void Error(int eventId, string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Error(eventId, message, null, callerName,sourceFilePath,sourceLineNumber);
        }


        /// <summary>
        ///     Logs the specified error message and exception.
        ///     We need to use a new transaction here as we do not want logging to fail because a transaction fails.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        [DebuggerStepThrough]
        public void Error(int eventId, string message, Exception exception,[CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_logger.IsErrorEnabled)
            {
                WriteLog(eventId, message, Level.Error, exception, callerName,sourceFilePath,sourceLineNumber);
            }
        }

        /// <summary>
        ///     Logs the specified fatal error message.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        [DebuggerStepThrough]
        public void Fatal(int eventId, string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Fatal(eventId, message, null, callerName,sourceFilePath,sourceLineNumber);
        }

        /// <summary>
        ///     Logs the specified fatal error message and exception.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        [DebuggerStepThrough]
        public void Fatal(int eventId, string message, Exception exception, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            WriteLog(eventId, message, Level.Fatal, exception, callerName,sourceFilePath,sourceLineNumber);
        }

        /// <summary>
        ///     Logs a debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="eventId">The id of the event</param>
        [DebuggerStepThrough]
        public void Debug(int eventId, string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_logger.IsDebugEnabled)
            {
                WriteLog(eventId, message, Level.Debug, null, callerName,sourceFilePath,sourceLineNumber);
            }
        }

        [DebuggerStepThrough]
        public void Debug(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_logger.IsDebugEnabled)
            {
                WriteLog(EventIds.Debug, message, Level.Debug, null, callerName,sourceFilePath,sourceLineNumber);
            }
        }

        [DebuggerStepThrough]
        public void Info(int eventId, string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_logger.IsInfoEnabled)
            {
                WriteLog(eventId, message, Level.Info, null, callerName,sourceFilePath,sourceLineNumber);
            }
        }

        [DebuggerStepThrough]
        private void WriteLog(int eventId, string message, Level level, Exception exception, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var data = new LoggingEventData();
            data.Level = level;
            data.LocationInfo = new LocationInfo("", callerName, sourceFilePath,sourceLineNumber.ToString(CultureInfo.InvariantCulture));
            data.Message = message;
            data.Properties = new PropertiesDictionary();
            data.Properties["EventID"] = eventId;
            if (exception != null) data.ExceptionString = exception.ToString();

            _logger.Logger.Log(new LoggingEvent(data));
        }

        /// <summary>
        ///     Logs the warning message if warning level is enabled.
        /// </summary>
        /// <param name="eventId">Event Id to log</param>
        /// <param name="message">Message to log</param>
        [DebuggerStepThrough]
        public void Warn(int eventId, string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_logger.IsWarnEnabled)
            {
                WriteLog(eventId, message, Level.Warn, null, callerName,sourceFilePath,sourceLineNumber);
            }
        }

        /// <summary>
        /// Logs the warning message and exception if warning level is enabled.
        /// </summary>
        /// <param name="eventId">Event Id to log</param>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        [DebuggerStepThrough]
        public void Warn(int eventId, string message, Exception exception, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (_logger.IsWarnEnabled)
            {
                WriteLog(eventId, message, Level.Warn, exception, callerName,sourceFilePath,sourceLineNumber);
            }
        }

        [DebuggerStepThrough]
        public void Fatal(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Fatal(EventIds.Fatal, message, callerName,sourceFilePath,sourceLineNumber);
        }

        [DebuggerStepThrough]
        public void Warn(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Warn(EventIds.Warn, message, callerName,sourceFilePath,sourceLineNumber);
        }

        [DebuggerStepThrough]
        public void Info(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Info(EventIds.Info, message, callerName,sourceFilePath,sourceLineNumber);
        }
      
       
    }
}
