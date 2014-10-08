using System;
using Lacjam.Core.Infrastructure;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{
    public class MetadataDefinitionGroupBagProjection
    {

        public MetadataDefinitionGroupBagProjection()
        {
            Identity = Guid.NewGuid();
            Tracking = new TrackingBase();
        }

        public virtual Guid Identity { get; set; }
        public virtual Guid AggregateIdentity { get; set; }
        public virtual Guid DefinitionId { get; set; }
        public virtual TrackingBase Tracking { get; set; }
        
    }
}