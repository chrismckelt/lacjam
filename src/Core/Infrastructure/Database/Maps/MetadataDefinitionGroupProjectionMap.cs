using System;
using System.Data.Entity.ModelConfiguration;
using System.Globalization;

using Lacjam.Core.Domain.MetadataDefinitionGroups;

namespace Lacjam.Core.Infrastructure.Database.Maps
{
    public class MetadataDefinitionGroupProjectionMap : EntityTypeConfiguration<MetadataDefinitionGroupProjection>
    {
        public MetadataDefinitionGroupProjectionMap()
        {
            //Id(x => x.Identity).Column("[Identity]").GeneratedBy.Assigned();
            //Map(x => x.Name);
            //Map(x => x.Description);

            //Component(proj => proj.Tracking, track =>
            //{
            //    track.Map(x => x.CreatedUtcDate).Default(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            //    track.Map(x => x.LastModifiedUtcDate);
            //});
        }
    }
}