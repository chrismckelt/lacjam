using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Lacjam.Core.Infrastructure.Database.Conventions
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.CustomType<Guid>();
            instance.Column("Id");
            instance.GeneratedBy.Custom<global::NHibernate.Id.SequenceHiLoGenerator>();
            instance.UnsavedValue(Guid.Empty.ToString());
        }
    }
}
