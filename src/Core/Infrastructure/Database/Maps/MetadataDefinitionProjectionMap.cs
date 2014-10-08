using System;
using System.Globalization;
using FluentNHibernate.Mapping;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Domain.MetadataDefinitions.Events;

namespace Lacjam.Core.Infrastructure.Database.Maps
{
    public class MetadataDefinitionProjectionMap : ClassMap<MetadataDefinitionProjection>
    {
        public MetadataDefinitionProjectionMap()
        {
            Id(x => x.Identity).Column("[Identity]").GeneratedBy.Assigned();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Description).Nullable();
            Map(x => x.DataType).Not.Nullable();
            Map(x => x.Regex).Nullable();            
   
            Component(proj => proj.Tracking, track =>
            {
                track.Map(x => x.CreatedUtcDate).Default(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
                track.Map(x => x.LastModifiedUtcDate);
            });

        }
    }
}