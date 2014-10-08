using System.Data;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Infrastructure.Database;
using Lacjam.Core.Infrastructure.Database.Conventions;
using Lacjam.Framework.Converters;

namespace Lacjam.Core.Infrastructure.Ioc
{
    internal class SessionManagementInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            string configPath = WindsorAccessor.FindNHibernateConfigFile();
            Configuration configuration = new Configuration().Configure(configPath);

            FluentConfiguration cfg = Fluently.Configure(configuration)
                .Database(MsSqlConfiguration.MsSql2012.IsolationLevel(IsolationLevel.ReadUncommitted))
                .Mappings(a =>
                    a.FluentMappings.AddFromAssemblyOf<MetadataDefinitionDescription>()
                        .AddFromAssemblyOf<InlineJson>()
                        .Conventions.Add<EnumConvention>()
                        .Conventions.Add<CascadeConvention>()
                        )
                .CurrentSessionContext<HybridWebSessionContext>()
                
                ;


#if DEBUG
            cfg.Diagnostics(x => x.Enable(true));
#endif

            container.Register(Component.For<ISessionFactory>()
                .UsingFactoryMethod(k => cfg.BuildSessionFactory()));

            container.Register(
                Component.For<ISessionFactoryImplementor>()
                    .UsingFactoryMethod(() => (ISessionFactoryImplementor) cfg.BuildSessionFactory()));

        }

    }
}


