using NHibernate;
using Lacjam.Core.Infrastructure.Database;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.WebApi.Infrastructure.Ioc;

namespace Lacjam.WebApi.Infrastructure.Database
{
    public static class NHibernateBootStrap
    {
        public static ISessionFactory Configure()
        {
            var fluentShared = WindsorAccessor.Instance.Container.Resolve<INHibernateFluentConfiguration>();
            var fluent = fluentShared.Configure();
            var cfg = fluent.Mappings(a => a.FluentMappings.AddFromAssemblyOf<ControllerFactory>())
                            .CurrentSessionContext<HybridWebSessionContext>()
                            .BuildConfiguration();
            return cfg.BuildSessionFactory();
        }

    }
}