using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lacjam.Core.Infrastructure.Database.Conventions;

namespace Lacjam.Core.Infrastructure.Database
{
    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {
        private readonly Assembly _assembly;

        public AutoPersistenceModelGenerator()
        {
            _assembly = null;
        }

        public AutoPersistenceModelGenerator(Assembly assembly)
        {
            _assembly = assembly;
        }

        public AutoPersistenceModel Generate()
        {
            var assemblies = _assembly != null
                ? new List<Assembly> { _assembly }
                : (from assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.ToLower().StartsWith("phoenix"))
                   from type in assembly.GetTypes()
                   where type.BaseType == typeof(PhoenixEntity)
                   select assembly).Distinct().ToList();

            var ass = _assembly ?? Assembly.GetAssembly(typeof(PhoenixEntity));


            var model = AutoMap.Assemblies(new PhoenixMappingConfiguration(), assemblies.ToArray())
                .Conventions.Setup(this.GetConventions())
                .IgnoreBase<PhoenixEntity>()
                .IgnoreBase<Individual>()
                .IgnoreBase<IndividualInvestor>()
                .IgnoreBase(typeof (EntityWithTypedId<>))
                .UseOverridesFromAssembly(ass);

            var asses =
                (from assembly in
                     AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.ToLower().StartsWith("phoenix"))
                 from type in assembly.GetTypes()
                 where type.GetInterfaces().Any(a => a == typeof (IAmExcludedFromNHibernateMapping)) && type.IsAbstract
                 select type);

            asses.ToList().Each(a=> model.IgnoreBase(a));
            return model;
        }

        private Action<IConventionFinder> GetConventions()
        {
            return c =>
            {
                c.Add<PrimaryKeyConvention>();
                c.Add<PhoenixForeignKeyConvention>();
                c.Add<DecimalConvention>();
            };
        }
    }
}
