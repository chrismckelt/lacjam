using System.Collections.Generic;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
using uNhAddIns.SessionEasier;

namespace Lacjam.Core.Infrastructure.Database
{
    public interface INHibernateFluentConfiguration : IConfigurationProvider
    {
        FluentConfiguration GetFluentConfiguration();
       
    }
}