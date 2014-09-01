using System;
using Lacjam.Core.Infrastructure;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{

    public class MetadataDefinitionGroupMetadataProjection : TrackingBase
    {
        public Guid ConceptIdentity{ get; set; }
        public string Name{ get; set; }
    }

}
