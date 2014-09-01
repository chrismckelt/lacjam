using System.Collections.Generic;
using Lacjam.Core.Domain.MetadataDefinitionGroups;

namespace Lacjam.Core.Services
{
    public class SchemaTransferObject
    {
        public SchemaTransferObject(MetadataDefinitionGroupProjection entity, IEnumerable<MetadataDefinitionGroupMetadataProjection> attributes)
        {
            Entity = entity;
            Attributes = attributes;
        }

        public MetadataDefinitionGroupProjection Entity { get; set; }
        public IEnumerable<MetadataDefinitionGroupMetadataProjection> Attributes { get; set; }
    }
}