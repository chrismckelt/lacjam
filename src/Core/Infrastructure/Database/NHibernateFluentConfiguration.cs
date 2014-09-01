using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using FluentNHibernate;
using FluentNHibernate.Cfg;
﻿using System.Collections.Generic;
﻿using System.Linq;
﻿using FluentNHibernate.Cfg.Db;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Infrastructure.Database.Conventions;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Framework.Converters;
using Lacjam.Framework.Extensions;
using uNhAddIns.SessionEasier;
using Configuration = NHibernate.Cfg.Configuration;

namespace Lacjam.Core.Infrastructure.Database
{
    public class NHibernateFluentConfiguration : AbstractConfigurationProvider, INHibernateFluentConfiguration
    {
        public override IEnumerable<Configuration> Configure()
        {
            var configPath = WindsorAccessor.FindNHibernateConfigFile();
            var configuration = new Configuration().Configure(configPath);

        
            // Debugger.Break();
            var cfg = Fluently.Configure(configuration)
                              .Database(MsSqlConfiguration.MsSql2012)
                              .Mappings(a =>
                                 {
                                     a.FluentMappings
                                         .AddFromAssemblyOf<MetadataDefinitionDescription>()
                                         .AddFromAssemblyOf<InlineJson>()
                                         .Conventions.Add<EnumConvention>();

                                 });

            var list = new List<Configuration> {cfg.BuildConfiguration()};
            return list.AsEnumerable();
        }

        public FluentConfiguration GetFluentConfiguration()
        {
            
            var configPath = WindsorAccessor.FindNHibernateConfigFile();
            var configuration = new Configuration().Configure(configPath);

            var cfg = Fluently.Configure(configuration)
                              .Database(MsSqlConfiguration.MsSql2012.IsolationLevel(IsolationLevel.ReadCommitted))
                              .CurrentSessionContext<HybridWebSessionContext>()
                              .Mappings(a =>
                                  a.FluentMappings.AddFromAssemblyOf<MetadataDefinitionDescription>()
                                                  .AddFromAssemblyOf<InlineJson>()
                                                  .Conventions.Add<EnumConvention>());

             cfg.BuildConfiguration();
            return cfg;
        }
    }
}