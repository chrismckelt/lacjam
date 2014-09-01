using System;
using Lacjam.Framework.Events;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Events
{

    public class MetadataDefinitionGroupNameChangedEvent : Event
    {
        public MetadataDefinitionGroupNameChangedEvent(Guid aggregateIdentity, MetadataDefinitionGroupName name)
            : base(aggregateIdentity)
        {

            if (aggregateIdentity == Guid.Empty)
                throw new InvariantGuardFailureException();

            if (name == null)
                throw new InvariantGuardFailureException();

            Name = name;
        }

        public MetadataDefinitionGroupName Name { get; private set; }
    }
}