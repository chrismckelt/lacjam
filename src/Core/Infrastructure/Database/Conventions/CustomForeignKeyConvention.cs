using System;
using System.Collections.Generic;
using FluentNHibernate;
using FluentNHibernate.Conventions;

namespace Lacjam.Core.Infrastructure.Database.Conventions
{
    public class CustomForeignKeyConvention : ForeignKeyConvention
    {
        public static IList<IMappingProvider> Mappings = new List<IMappingProvider>();

        protected override string GetKeyName(Member property, Type type)
        {
            return type.Name + "Id";

        }
    }
}

