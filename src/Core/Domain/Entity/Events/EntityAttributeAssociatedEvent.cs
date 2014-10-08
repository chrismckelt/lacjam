using System;
using Structerre.MetaStore.Framework.Events;

namespace Structerre.MetaStore.Core.Domain.Entity.Events
{
    public class EntityAttributeAssociatedEvent : Event
    {
        public EntityAttributeAssociatedEvent(Guid identity, Guid attribute)
        {
            AggregateIdentity = identity;
            Attribute = attribute;
        }

        public Guid Attribute{ get; private set; }
    }

}
