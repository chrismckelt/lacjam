using System;
using System.Threading;
using Castle.Core.Logging;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using Lacjam.Framework.Dispatchers;

namespace Lacjam.Dispatcher
{
    public class Dispatcher : IDispatcher
    {

        public Dispatcher(IDispatcherEngine dispatcherEngine)
        {
            _dispatcherEngine = dispatcherEngine;
        }

        public void Start()
        {
            NHibernateProfiler.Initialize();
            Logger.Info("Dispatcher started");
            ParseCommandLine();
            ThreadPool.QueueUserWorkItem(x => Run());
        }

        public void Stop()
        {
            Logger.Info("Shutting down dispatcher");
            IsRunning = false;
        }

        private void ParseCommandLine()
        {
            var arg = Environment.GetEnvironmentVariable(UITestEnvironmentVariable);
            ExitAtEndOfStream = string.Equals(arg, "true", StringComparison.InvariantCultureIgnoreCase);
        }

        private void Run()
        {
            Logger.Info("Starting dispatcher");
            IsRunning = true;
            _dispatcherEngine.Process(() => !IsRunning, ExitAtEndOfStream);
        }

        public ILogger Logger { get; set; }

        private readonly IDispatcherEngine _dispatcherEngine;
        private const string UITestEnvironmentVariable = "UITest";
        private bool IsRunning { get; set; }
        private bool ExitAtEndOfStream { get; set; }
    }
}
