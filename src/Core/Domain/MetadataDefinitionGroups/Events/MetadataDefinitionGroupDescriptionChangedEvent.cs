using System;
using Lacjam.Framework.Events;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Events
{
    public class MetadataDefinitionGroupDescriptionChangedEvent : Event
    {
        public MetadataDefinitionGroupDescriptionChangedEvent(Guid aggregateIdentity, MetadataDefinitionGroupDescription description)
            : base(aggregateIdentity)
        {

            if (aggregateIdentity == Guid.Empty)
                throw new InvariantGuardFailureException();

            if (description == null)
                throw new InvariantGuardFailureException();

            Description = description;
        }

        public MetadataDefinitionGroupDescription Description { get; private set; }
    }
}