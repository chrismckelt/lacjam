using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Entities.Events
{
    public class EntityRenamedEvent : Event
    {
        protected EntityRenamedEvent()
        {
        }

        public EntityRenamedEvent(Guid aggregateIdentity, EntityName name)
            : base(aggregateIdentity)
        {
            Name = name;
        }

        public EntityName Name{ get; private set; }
    }
}