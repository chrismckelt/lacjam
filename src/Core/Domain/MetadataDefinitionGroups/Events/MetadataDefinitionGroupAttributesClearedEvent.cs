using System;
using Lacjam.Framework.Events;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Events
{
    public class MetadataDefinitionGroupAttributesClearedEvent : Event
    {
        public MetadataDefinitionGroupAttributesClearedEvent(Guid aggregateIdentity)
            : base(aggregateIdentity)
        {

            if (aggregateIdentity == Guid.Empty)
                throw new InvariantGuardFailureException();
        }

    }
}