using System;
using System.Data.Entity.ModelConfiguration;
using System.Globalization;

using Lacjam.Core.Domain.Entities;

namespace Lacjam.Core.Infrastructure.Database.Maps
{
    public class EntityValueProjectionMap : EntityTypeConfiguration<EntityValueProjection>
    {
        public EntityValueProjectionMap()
        {
            //Id(x => x.Identity).Column("[Identity]").GeneratedBy.Assigned();
            //Map(x => x.Name);
            //Map(x => x.Regex);
            //Map(x => x.MetadataDefinitionIdentity);
            //Map(x => x.DataType);
            //Map(x => x.EntityIdentity);
            //Map(x => x.Value);

            //Component(proj => proj.Tracking, track =>
            //{
            //    track.Map(x => x.CreatedUtcDate).Default(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            //    track.Map(x => x.LastModifiedUtcDate);
            //});

        }
    }
}