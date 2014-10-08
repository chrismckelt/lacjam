using System;
using Lacjam.Core.Infrastructure;
using Lacjam.Framework.Events;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Events
{
    public class MetadataDefinitionGroupDeletedEvent : Event

    {
        public MetadataDefinitionGroupDeletedEvent(Guid aggregateIdentity)
            : base(aggregateIdentity)
        {

            if (aggregateIdentity == Guid.Empty)
                throw new InvariantGuardFailureException("aggregateIdentity");
        }

    }
}