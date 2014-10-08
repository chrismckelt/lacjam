using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Entities.Events
{
    public class EntityChangedGroupEvent : Event
    {
        protected EntityChangedGroupEvent()
        {
        }

        public EntityChangedGroupEvent(Guid aggregateIdentity, Guid definitionGroupId)
            : base(aggregateIdentity)
        {
            DefinitionGroupId = definitionGroupId;
        }

        public Guid DefinitionGroupId { get; private set; }
    }
}