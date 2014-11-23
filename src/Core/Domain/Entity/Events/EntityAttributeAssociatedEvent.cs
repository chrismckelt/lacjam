using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Entity.Events
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
