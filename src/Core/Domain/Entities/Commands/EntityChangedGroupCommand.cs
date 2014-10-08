using System;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class EntityChangedGroupCommand : ICommand
    {
        public EntityChangedGroupCommand(Guid aggregateIdentity, Guid definitionGroupId)
        {
            AggregateIdentity = aggregateIdentity;
            DefinitionGroupId = definitionGroupId;
        }

        public Guid AggregateIdentity { get; private set; }
        public Guid DefinitionGroupId { get; private set; }
    }
}