using System;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitions.Commands
{
    public class DeleteMetadataDefinitionCommand : ICommand
    {
        public DeleteMetadataDefinitionCommand(Guid identity)
        {
            Identity = identity;
        }

        public Guid Identity { get; private set; }
    }
}