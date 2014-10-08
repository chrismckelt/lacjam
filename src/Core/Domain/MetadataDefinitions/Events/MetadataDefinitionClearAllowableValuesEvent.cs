using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionClearAllowableValuesEvent : Event
    {
        public MetadataDefinitionClearAllowableValuesEvent(Guid aggregateIdentity)
            : base(aggregateIdentity)
        {

        }
    }
}