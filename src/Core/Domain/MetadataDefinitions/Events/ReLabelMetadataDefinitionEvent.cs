using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class ReLabelMetadataDefinitionEvent : Event
    {
        public ReLabelMetadataDefinitionEvent(Guid aggregateIdentity, string name)
            : base(aggregateIdentity)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}