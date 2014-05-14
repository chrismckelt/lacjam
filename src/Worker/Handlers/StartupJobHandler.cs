using Lacjam.Core;
using Lacjam.Worker.Jobs;
using NServiceBus;
using System;

namespace Lacjam.Worker.Handlers
{
    public class StartupJobHandler : HandlerBase<PrintBatchJob>
    {
        public StartupJobHandler(IBus bus, Runtime.ILogWriter logger)
            : base(bus, logger)
        {
        }

        public override void Handle(PrintBatchJob message)
        {
            throw new NotImplementedException();
        }
    }
}