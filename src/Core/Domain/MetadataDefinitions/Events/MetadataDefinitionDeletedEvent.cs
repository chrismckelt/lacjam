using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionDeletedEvent : Event
    {
        public MetadataDefinitionDeletedEvent(Guid aggregateIdentity)
            : base(aggregateIdentity)
        {

        }

    }
}