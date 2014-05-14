using Lacjam.Core;
using Lacjam.Worker.Jobs;
using NServiceBus;
using System;

namespace Lacjam.Worker.Handlers
{
    public class WebPageRetrieverHandler : HandlerBase<WebPageRetrieverJob>
    {
        public WebPageRetrieverHandler(IBus bus, Runtime.ILogWriter logger)
            : base(bus, logger)
        {
        }

        public override void Handle(WebPageRetrieverJob message)
        {
            throw new NotImplementedException();
        }
    }
}