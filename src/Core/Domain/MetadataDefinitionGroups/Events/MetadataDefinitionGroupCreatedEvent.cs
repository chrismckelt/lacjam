using System;
using System.Collections.Generic;
using Lacjam.Framework.Events;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Events
{
    public class MetadataDefinitionGroupCreatedEvent : Event
    {

        public MetadataDefinitionGroupCreatedEvent(Guid aggregateIdentity, MetadataDefinitionGroupName name, MetadataDefinitionGroupDescription description)
            : base(aggregateIdentity)
        {

            if (aggregateIdentity == Guid.Empty)
                throw new InvariantGuardFailureException("aggregateIdentity");

            if (name == null)
                throw new InvariantGuardFailureException("name");

            if (description == null)
                throw new InvariantGuardFailureException("description");

            Name = name;
            Description = description;
        }

        public MetadataDefinitionGroupName Name { get; private set; }
        public MetadataDefinitionGroupDescription Description { get; private set; }
    }
}
