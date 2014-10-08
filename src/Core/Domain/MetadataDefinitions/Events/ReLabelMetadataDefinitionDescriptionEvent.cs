using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class ReLabelMetadataDefinitionDescriptionEvent : Event
    {

        protected ReLabelMetadataDefinitionDescriptionEvent(){}

        public ReLabelMetadataDefinitionDescriptionEvent(Guid aggregateIdentity, string description)
            : base(aggregateIdentity)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}