using NHibernate;
using Structerre.MetaStore.Core.Infrastructure.Database;
using Structerre.MetaStore.Core.Infrastructure.Ioc;
using Structerre.MetaStore.WebApi.Infrastructure.Ioc;

namespace Structerre.MetaStore.WebApi.Infrastructure.Database
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