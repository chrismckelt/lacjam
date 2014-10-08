using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Entities.Events
{
    public class EntityMetadataRemovedEvent : Event
    {
        public EntityMetadataRemovedEvent(Guid aggregateIdentity) : base(aggregateIdentity)
        {
        }
    }
}