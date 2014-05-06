
using System;
using System.IO;
using Lacjam.Core;
using Quartz.Impl;

namespace Lacjam.ServiceBus
{
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
	    public EndpointConfig()
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
