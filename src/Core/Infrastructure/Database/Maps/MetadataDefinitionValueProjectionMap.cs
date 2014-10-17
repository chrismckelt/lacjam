using System;
using System.Data.Entity.ModelConfiguration;
using System.Globalization;

using Lacjam.Core.Domain.MetadataDefinitions.Events;

namespace Lacjam.Core.Infrastructure.Database.Maps
{
    public class MetadataDefinitionValueProjectionMap : EntityTypeConfiguration<MetadataDefinitionValueProjection>
    {
        public MetadataDefinitionValueProjectionMap()
        {
            //Id(x => x.Identity).Column("[Identity]").GeneratedBy.Assigned();
            //Map(x => x.Value).Not.Nullable();
            //Map(x => x.DefinitionId).Not.Nullable();

            //Component(proj => proj.Tracking, track =>
            //{
            //    track.Map(x => x.CreatedUtcDate).Default(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            //    track.Map(x => x.LastModifiedUtcDate).Default(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            //});
        }
    }
}