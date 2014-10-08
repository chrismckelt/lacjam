using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Lacjam.Core.Infrastructure.Database.Conventions
{
    public class CascadeConvention : IHasManyToManyConvention, IHasManyConvention, IHasManyConventionAcceptance, IReferenceConvention, IHasManyToManyConventionAcceptance
    {
        public void Apply(IManyToManyCollectionInstance instance)
        {
            DefaultCascade.All();
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            DefaultCascade.All();
        }

        public void Accept(IAcceptanceCriteria<IOneToManyCollectionInspector> criteria)
        {
            DefaultCascade.All();
        }

        public void Apply(IOneToOneInstance instance)
        {
            DefaultCascade.All();
        }

        public void Apply(IManyToOneInstance instance)
        {
            DefaultCascade.All();
        }

        public void Accept(IAcceptanceCriteria<IManyToManyCollectionInspector> criteria)
        {
            DefaultCascade.All();
        }
    }
}
