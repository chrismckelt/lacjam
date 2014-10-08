using System;
using Lacjam.Framework.Events;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Events
{
    public class MetadataDefinitionGroupAttributeRemovedEvent : Event
    {
        public MetadataDefinitionGroupAttributeRemovedEvent(Guid aggregateIdentity, Guid definitionIdentity)
            : base(aggregateIdentity)
        {
            if (aggregateIdentity == Guid.Empty)
                throw new InvariantGuardFailureException("aggregateIdentity");

            if (definitionIdentity == Guid.Empty)
                throw new InvariantGuardFailureException("definitionIdentity");

            DefinitionIdentity = definitionIdentity;
        }

        public Guid DefinitionIdentity { get; private set; }
    }
}