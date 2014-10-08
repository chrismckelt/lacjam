using System;
using Lacjam.Core.Infrastructure;

namespace Lacjam.Core.Domain.Entities
{
    public class EntityProjection
    {

        protected EntityProjection(){}

        public EntityProjection(Guid identity, Guid metadataDefinitionGroupIdentity, string name) : this(identity, metadataDefinitionGroupIdentity, name, new TrackingBase())
        {
        }

        public EntityProjection(Guid identity, Guid metadataDefinitionGroupIdentity, string name, TrackingBase tracking)
        {
            Identity = identity;
            MetadataDefinitionGroupIdentity = metadataDefinitionGroupIdentity;
            Name = name;
            Tracking = tracking;
        }

        public virtual EntityProjection WithName(string name)
        {
            return new EntityProjection(Identity,MetadataDefinitionGroupIdentity, name, Tracking.WithUpdatedDateTime());
        }

        public virtual EntityProjection WithGroup(Guid definitionGroupId)
        {
            return new EntityProjection(Identity, definitionGroupId, Name, Tracking.WithUpdatedDateTime());
        }

        public virtual Guid Identity { get; set; }
        public virtual Guid MetadataDefinitionGroupIdentity { get; set; }
        public virtual string Name { get; set; }
        public virtual TrackingBase Tracking { get; set; }
    }
}