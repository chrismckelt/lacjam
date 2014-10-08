using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Entities.Events
{
    public class EntityMetadataDefinitionRemovedEvent : Event
    {
        public EntityMetadataDefinitionRemovedEvent(Guid aggregateIdentity, Guid definitionId) : base(aggregateIdentity)
        {
            DefinitionId = definitionId;
        }

        public Guid DefinitionId { get; private set; }
    }
}