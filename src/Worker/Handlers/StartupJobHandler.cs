using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacjam.Core;
using Lacjam.Worker.Jobs;
using NServiceBus;

namespace Lacjam.Worker.Handlers
{
    public class StartupJobHandler : HandlerBase<StartupJob>
    {
        public StartupJobHandler(IBus bus, Runtime.ILogWriter logger) : base(bus, logger)
        {
        }


        public override void Handle(StartupJob message)
        {
            throw new NotImplementedException();
        }
    }
}
