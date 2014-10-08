using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Entities.Events
{

    public class EntityCreatedEvent : Event
    {

        protected EntityCreatedEvent()
        {
        }

        public EntityCreatedEvent(Guid aggregateIdentity, Guid metadataDefinitionGroupIdentity, EntityName name) : base(aggregateIdentity)
        {
            Name = name;
            MetadataDefinitionGroupIdentity = metadataDefinitionGroupIdentity;
        }

        public Guid MetadataDefinitionGroupIdentity { get; private set; }
        public EntityName Name{ get; private set; }
    }
}
