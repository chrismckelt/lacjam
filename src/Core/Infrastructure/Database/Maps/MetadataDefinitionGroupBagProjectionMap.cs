using FluentNHibernate.Mapping;
using Lacjam.Core.Domain.MetadataDefinitionGroups;

namespace Lacjam.Core.Infrastructure.Database.Maps
{
    public class MetadataDefinitionGroupBagProjectionMap : ClassMap<MetadataDefinitionGroupBagProjection>
    {
        public MetadataDefinitionGroupBagProjectionMap()
        {
            Id(x => x.Identity).Column("[Identity]").GeneratedBy.GuidComb();
            Map(x => x.AggregateIdentity);
            Map(x => x.DefinitionId);
          
        }
    }
}