using System;
using System.Collections.Generic;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
{
    public class AssociateDefinitionsToMetadataDefinitionGroupCommand : ICommand
    {
        public AssociateDefinitionsToMetadataDefinitionGroupCommand(Guid identity, IEnumerable<Guid> definitionIds)
        {
            Identity = identity;
            DefinitionIds = definitionIds;
        }

        public Guid Identity { get; private set; }
        public IEnumerable<Guid> DefinitionIds { get; private set; }
    }
}