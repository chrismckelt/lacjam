using System.Collections.Generic;
using System.Configuration;
using Castle.Components.DictionaryAdapter;
using Castle.Facilities.Logging;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NHibernate;
using NHibernate.Engine;
using Lacjam.Core.Infrastructure.Database;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Core.Infrastructure.Ioc.Interceptors;
using Lacjam.Core.Infrastructure.Projection;
using Lacjam.Core.Infrastructure.Settings;
using Lacjam.Core.Infrastructure.Storage;
using Lacjam.Core.Services;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;
using Lacjam.Framework.Storage;
using uNhAddIns.CastleAdapters;
using uNhAddIns.SessionEasier;

namespace Lacjam.Core.Infrastructure.Ioc
{
    public class CoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindsorContainer>().Instance(container).LifestyleSingleton());
            AddWindsorLogging(container);
            RegisterSettings(container);
            RegisterBaseSessionMaanagement(container);
            RegisterDomainComponents(container);
            RegisterServiceComponents(container);
            RegisterStorage(container);
            RegisterIndexers(container);
        }

        private void RegisterIndexers(IWindsorContainer container)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .InSameNamespaceAs<EntityIndexer>()
                    .If(x => x.Name.EndsWith("Indexer"))
                    .WithServiceAllInterfaces()
                    .LifestyleSingleton());
        }

        private static void RegisterDomainComponents(IWindsorContainer container)
        {
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn(typeof(ICommandHandler<>), typeof(IEventHandler<>))
                       .WithServiceAllInterfaces()
                       .LifestyleTransient()
            );
        }

        private static void RegisterServiceComponents(IWindsorContainer container)
        {

            container.Register(
                Classes.FromThisAssembly()
                       .Where(x => x.Name.Contains("Service"))
                       .WithServiceDefaultInterfaces()
                       .LifestyleTransient()
            );
        }

        private static void AddWindsorLogging(IWindsorContainer container)
        {
            container.AddFacility<LoggingFacility>(facility => facility.LogUsing(LoggerImplementation.Diagnostics));
            container.Register(Component.For<LoggingInterceptor>().LifestyleTransient());
            container.Register(Component.For<ExceptionHandlerInterceptor>().LifestyleTransient());            
        }

        private static void RegisterSettings(IWindsorContainer container)
        {
            var factory = new DictionaryAdapterFactory();
            var adapter = factory.GetAdapter<IConfigSetting>(ConfigurationManager.AppSettings);
            container.Register(Component.For<IConfigSetting>().Instance(adapter));
        }

        private static void RegisterBaseSessionMaanagement(IWindsorContainer container)
        {
            container.Kernel.AddFacility<TypedFactoryFacility>();

            container.Register(Component.For<ISessionFactoryProvider>().AsFactory());

            container.Register(Component.For<INHibernateFluentConfiguration>().ImplementedBy<NHibernateFluentConfiguration>());

            container.Register(Component.For<ISessionWrapper>().ImplementedBy<SessionWrapper>());


        }

        private static void RegisterStorage(IWindsorContainer container)
        {

            container.Register(Component.For<IEventStore>().ImplementedBy<EventStore>().LifestyleSingleton());
            container.Register(Component.For(typeof(IReadStoreRepository<>))
                .ImplementedBy(typeof(ReadStoreRepository<>))
                .LifestyleSingleton());
        }
    }
}