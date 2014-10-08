using System;
using System.Globalization;
using FluentNHibernate.Mapping;
using Lacjam.Core.Domain.MetadataDefinitionGroups;

namespace Lacjam.Core.Infrastructure.Database.Maps
{
    public class MetadataDefinitionGroupBagProjectionMap : ClassMap<MetadataDefinitionGroupBagProjection>
    {
        public MetadataDefinitionGroupBagProjectionMap()
        {
            Id(x => x.Identity).Column("[Identity]").GeneratedBy.Assigned();
            Map(x => x.AggregateIdentity);
            Map(x => x.DefinitionId);

            Component(proj => proj.Tracking, track =>
            {
                track.Map(x => x.CreatedUtcDate).Default(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
                track.Map(x => x.LastModifiedUtcDate);
            });
            
        }
    }
}