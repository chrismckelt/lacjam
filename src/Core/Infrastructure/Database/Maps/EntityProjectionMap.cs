using System;
using System.Data.Entity.ModelConfiguration;
using System.Globalization;

using Lacjam.Core.Domain.Entities;

namespace Lacjam.Core.Infrastructure.Database.Maps
{
    public class EntityProjectionMap : EntityTypeConfiguration<EntityProjection>
    {
        public EntityProjectionMap()
        {
            //Id(x => x.Identity).Column("[Identity]").GeneratedBy.Assigned();
            //Map(x => x.MetadataDefinitionGroupIdentity);
            //Map(x => x.Name);

            //Component(proj => proj.Tracking, track =>
            //{
            //    track.Map(x => x.CreatedUtcDate).Default(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            //    track.Map(x => x.LastModifiedUtcDate);
            //});

        }
    }
}