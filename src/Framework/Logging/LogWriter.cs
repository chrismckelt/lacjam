using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Core;

namespace Lacjam.Framework.Logging
{
    /// <summary>
    ///     Logging helper class. Wraps log4net, but also implements an interface so that
    ///     logging can be mocked out when testing.
    /// </summary>
    public class LogWriter : ILogWriter
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof (LogWriter));

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
       // [DebuggerStepThrough]
        public void Error(string message)
        {
            Error(EventIds.Error, message, null);
        }

        /// <summary>
        ///     Logs the specified error message.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        [DebuggerStepThrough]
        public void Error(int eventId, string message)
        {
            Error(eventId, message, null);
        }


        /// <summary>
        ///     Logs the specified error message and exception.
        ///     We need to use a new transaction here as we do not want logging to fail because a transaction fails.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        [DebuggerStepThrough]
        public void Error(int eventId, string message, Exception exception)
        {
            if (_logger.IsErrorEnabled)
            {
                WriteLog(eventId, message, Level.Error, exception);
            }
        }

        /// <summary>
        ///     Logs the specified fatal error message.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        [DebuggerStepThrough]
        public void Fatal(int eventId, string message)
        {
            Fatal(eventId, message, null);
        }

        /// <summary>
        ///     Logs the specified fatal error message and exception.
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        [DebuggerStepThrough]
        public void Fatal(int eventId, string message, Exception exception)
        {
            WriteLog(eventId, message, Level.Fatal, exception);
        }

        /// <summary>
        ///     Logs a debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="eventId">The id of the event</param>
        [DebuggerStepThrough]
        public void Debug(int eventId, string message)
        {
            if (_logger.IsDebugEnabled)
            {
                WriteLog(eventId, message, Level.Debug, null);
            }
        }

        [DebuggerStepThrough]
        public void Debug(string message)
        {
            if (_logger.IsDebugEnabled)
            {
                WriteLog(EventIds.Debug, message, Level.Debug, null);
            }
        }

        [DebuggerStepThrough]
        public void Info(int eventId, string message)
        {
            if (_logger.IsInfoEnabled)
            {
                WriteLog(eventId, message, Level.Info, null);
            }
        }

        [DebuggerStepThrough]
        private static void WriteLog(int eventId, string message, Level level, Exception exception)
        {
            string name = string.Empty;

            if (_logger.IsDebugEnabled)
            {
                var frame = new StackFrame(2, true);
                if (frame != null)
                {
                    MethodBase method = frame.GetMethod();
                    if (method != null) if (method.DeclaringType != null) name = method.DeclaringType.Name;
                }
            }

            var loggingEvent = new LoggingEvent(_logger.GetType(), _logger.Logger.Repository, name, level, message,
                exception);
            loggingEvent.Properties["EventID"] = eventId;
            _logger.Logger.Log(loggingEvent);
        }


        /// <summary>
        ///     Logs the warning message if warning level is enabled.
        /// </summary>
        /// <param name="eventId">Event Id to log</param>
        /// <param name="message">Message to log</param>
        [DebuggerStepThrough]
        public void Warn(int eventId, string message)
        {
            if (_logger.IsWarnEnabled)
            {
                WriteLog(eventId, message, Level.Warn, null);
            }
        }

        /// <summary>
        /// Logs the warning message and exception if warning level is enabled.
        /// </summary>
        /// <param name="eventId">Event Id to log</param>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        [DebuggerStepThrough]
        public void Warn(int eventId, string message, Exception exception)
        {
            if (_logger.IsWarnEnabled)
            {
                WriteLog(eventId, message, Level.Warn, exception);
            }
        }

        [DebuggerStepThrough]
        public void Fatal(string message)
        {
            Fatal(EventIds.Fatal, message);
        }

        [DebuggerStepThrough]
        public void Warn(string message)
        {
            Warn(EventIds.Warn, message);
        }

        [DebuggerStepThrough]
        public void Info(string message)
        {
            Info(EventIds.Info, message);
        }
    }
}
