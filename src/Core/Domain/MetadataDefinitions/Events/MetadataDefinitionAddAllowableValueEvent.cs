using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionAddAllowableValueEvent : Event
    {
        public MetadataDefinitionAddAllowableValueEvent(Guid aggregateIdentity, AllowableValue value)
            : base(aggregateIdentity)
        {
            Value = value;
        }

        public AllowableValue Value { get; protected set; }
    }
}