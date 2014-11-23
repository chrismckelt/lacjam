using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using NHibernate;
using Lacjam.Core.Infrastructure.Database;
using Lacjam.Core.Infrastructure.Database.Conventions;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.Handlers;
using uNhAddIns.SessionEasier;

namespace Lacjam.Dispatcher
{
    public static class NHibernateBootStrap
    {
        public static void Configure()
        {
            var container = WindsorAccessor.Instance.Container;
              var service = container.Resolve<INHibernateFluentConfiguration>();
            var fluent = service.GetFluentConfiguration();
            fluent = fluent.Mappings(x => x.FluentMappings.AddFromAssemblyOf<HandlerSequence>().Conventions.Add<EnumConvention>());
            var cfg = fluent.BuildConfiguration();
            var sessionFactory = cfg.BuildSessionFactory();

            container.Register(Component.For<ISessionFactory>().Instance(sessionFactory).LifestyleSingleton());

            WindsorAccessor.Instance.Container.Register(Component.For<IUnitOfWork>().ImplementedBy<UnitOfWork>().LifestylePerThread());
            WindsorAccessor.Instance.Container.BeginScope();
          
        }

    }
}