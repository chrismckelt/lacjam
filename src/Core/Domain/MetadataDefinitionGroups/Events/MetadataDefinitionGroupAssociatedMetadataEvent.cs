using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Events
{
    public class MetadataDefinitionGroupAssociatedMetadataEvent : Event
    {
        public MetadataDefinitionGroupAssociatedMetadataEvent(Guid aggregateIdentity, Guid definitionId)
        {
            AggregateIdentity = aggregateIdentity;
            DefinitionId = definitionId;
        }

        public Guid DefinitionId{ get; private set; }
    }

}
