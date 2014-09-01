using System;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
{
    public class DeleteMetadataDefinitionGroupCommand : ICommand
    {
        public DeleteMetadataDefinitionGroupCommand(Guid identity)
        {
            Identity = identity;
        }

        public Guid Identity { get; protected set; }
    }
}