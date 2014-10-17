using System;
using System.Threading;
using Castle.Core.Logging;

using Lacjam.Framework.Dispatchers;

namespace Lacjam.Dispatcher
{
    public class Dispatcher : IDispatcher
    {
        public Dispatcher(IDispatcherEngine dispatcherEngine)
        {
            _dispatcherEngine = dispatcherEngine;
        }

        public void Start(string mode)
        {
           
            Logger.Info("Dispatcher started");
            Mode = mode;
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
            if (string.Equals(Mode, "uitest", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("======== UI Test Mode ========="); 
                _includeImmediate = false;
                ExitAtEndOfStream = true;    
            }
            else if (string.Equals(Mode, "recovery", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("======== RECOVERY MODE =========");
                _includeImmediate = true;
                ExitAtEndOfStream = true;
            }
            else
            {
                Console.WriteLine("======== Wather Mode ========="); 
                _includeImmediate = false;
                ExitAtEndOfStream = false;
            }
            
        }

        private void Run()
        {
            Logger.Info("Starting dispatcher");
            IsRunning = true;
            _dispatcherEngine.Process(() => !IsRunning, ExitAtEndOfStream, _includeImmediate);
        }

        public ILogger Logger { get; set; }

        private readonly IDispatcherEngine _dispatcherEngine;
        private bool _includeImmediate;
        private bool IsRunning { get; set; }
        private bool ExitAtEndOfStream { get; set; }
        public string Mode { get; set; }
    }
}
