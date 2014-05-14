using Lacjam.Core;
using Lacjam.Worker.Batches;
using Lacjam.Worker.Scheduler;
using NServiceBus;
using Quartz;
using Quartz.Impl;
using System;
using System.Linq;

namespace Lacjam.ServiceBus
{
    public class ServiceBusStartUp : IWantToRunWhenBusStartsAndStops
    {
        private Runtime.ILogWriter _logWriter;
        private IBus _bus;
        private IJobScheduler _js;

        public void Start()
        {
            _logWriter = Runtime.Ioc.Resolve<Runtime.ILogWriter>();
            _bus = Runtime.Ioc.Resolve<IBus>();

            _logWriter.Info("-- Service Bus Started --");

            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, errors) => true; //four underscores (and seven years ago?)

            var fac = new StdSchedulerFactory();
            fac.Initialize();
            var scheduler = fac.GetScheduler();
            scheduler.ListenerManager.AddSchedulerListener(new JobSchedulerListener(Runtime.Ioc.Resolve<Runtime.ILogWriter>(), Runtime.Ioc.Resolve<IBus>()));
            scheduler.Start();

            Runtime.Ioc.Register(
                Castle.MicroKernel.Registration.Component.For<Quartz.IScheduler>()
                    .Instance(scheduler)
                    .LifestyleSingleton());

            Runtime.Ioc.Register(
                Castle.MicroKernel.Registration.Component.For<IJobScheduler>()
                    .ImplementedBy<JobScheduler>()
                    .LifestyleSingleton());

            _js = Runtime.Ioc.Resolve<IJobScheduler>();

            Configure.Instance.Configurer.ConfigureComponent<Quartz.IScheduler>(DependencyLifecycle.SingleInstance);

            var type = typeof(IContainBatches);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                // .Cast<IContainBatches>()
                ;
        }

        public void Stop()
        {
            Runtime.Ioc.Resolve<IScheduler>().Shutdown(true);
            _logWriter.Info("-- Service Bus Stopped --");
            Runtime.Ioc.Dispose();
        }
    }
}