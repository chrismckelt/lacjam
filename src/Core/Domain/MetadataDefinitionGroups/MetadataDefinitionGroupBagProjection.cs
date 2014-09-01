using System;
using Lacjam.Core.Infrastructure;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{
    public class MetadataDefinitionGroupBagProjection : TrackingBase
    {
        public virtual Guid Identity { get; set; }
        public virtual Guid AggregateIdentity { get; set; }
        public virtual Guid DefinitionId { get; set; }
        
    }
}