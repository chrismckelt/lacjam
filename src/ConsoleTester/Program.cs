using System;
using System.IO;
using System.Linq;
using Lacjam.Core;
using Lacjam.Worker.Handlers;
using Lacjam.Worker.Jobs;
using NServiceBus;

namespace Lacjam.ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Setup();
            var logger = Runtime.Ioc.Resolve<Runtime.ILogWriter>();
            var bus = Runtime.Ioc.Resolve<IBus>();
            var quote = Lacjam.Modules.StockPrice.getStockQuote("msft").LastOrDefault().High;


            var handler = new SendTweetJobHandler(bus, logger);
            var job = new SendTweetJob();
            job.To = "chris_mckelt";
            job.Settings = Settings.GetTwitterSettings;
            job.Payload = "MSFT = "+ quote;
            handler.Handle(job);
        }

        private static void Setup()
        {
            Configure.Transactions.Enable();
            Configure.Serialization.Json();
            Configure.ScaleOut(a => a.UseSingleBrokerQueue());

            try
            {
                Configure.With()
                    .Log4Net()
                    .DefineEndpointName("lacjam.servicebus")
                    .LicensePath((Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToLowerInvariant(), "license.xml")))
                    .CastleWindsorBuilder(Lacjam.Core.Runtime.Ioc)
                    .InMemorySagaPersister()
                    .InMemoryFaultManagement()
                    .InMemorySubscriptionStorage()
                    .UseInMemoryTimeoutPersister()
                    .UseTransport<Msmq>()
                    .PurgeOnStartup(true)
                    .UnicastBus();
            }
            catch (Exception ex)
            {
                Runtime.Ioc.Resolve<Runtime.ILogWriter>().Error("StartupInitialisation", ex);
                throw;
            }
        }
    }
}
