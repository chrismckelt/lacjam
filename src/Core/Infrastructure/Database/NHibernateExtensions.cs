using System.Collections.Generic;

using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Engine;
using NHibernate.Engine.Query.Sql;

using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Core.Infrastructure.Ioc.Convo;

namespace Lacjam.Core.Infrastructure.Database
{
    public static class NHibernateExtensions
    {
        public static Configuration AddSqlNamedQuery(this Configuration configuration,
            string queryName, string queryDefinition)
        {
            configuration.NamedSQLQueries
                .AddOrOverride(new Dictionary<string, NamedSQLQueryDefinition>
                                    {
                                        {
                                            queryName,
                                            new NamedSQLQueryDefinition(
                                            queryDefinition,
                                            new INativeSQLQueryReturn[] {},
                                            null,
                                            false, null, 3000, 5000, FlushMode.Auto, CacheMode.Normal, true, string.Empty,
                                            null, false)
                                            }
                                    });

            return configuration;
        }

        public static Configuration AddHqlNamedQuery(this Configuration configuration,
            string queryName, string queryDefinition)
        {
            configuration.NamedQueries
                .AddOrOverride(new Dictionary<string, NamedQueryDefinition>
                                    {
                                        {
                                            queryName,
                                            new NamedQueryDefinition(
                                            queryDefinition,
                                            false, null, 3000, 5000, FlushMode.Auto, true, string.Empty,
                                            null)
                                            }
                                    });

            return configuration;
        }


        public static ISession GetCurrentSessionOrOpen(this DbContext sessionFactory)
        {
            if (HybridWebSessionContext.HasBind(sessionFactory))
            {
                return sessionFactory.GetCurrentSession();
            }

            return WindsorAccessor.Instance.Container.Resolve<IUnitOfWork>().Session;

        }
    }
}