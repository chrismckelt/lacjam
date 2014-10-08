using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Entities.Events
{
    public class EntityDeletedEvent : Event
    {
        public EntityDeletedEvent(Guid aggregateIdentity) : base(aggregateIdentity) 
        {

        }
    }
}