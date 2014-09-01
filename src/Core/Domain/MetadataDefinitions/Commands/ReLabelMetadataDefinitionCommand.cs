using System;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitions.Commands
{
    public class ReLabelMetadataDefinitionCommand : ICommand
    {

        public ReLabelMetadataDefinitionCommand(Guid identity, MetadataDefinitionName name)
        {
            Identity = identity;
            Name = name;
        }

        public Guid Identity { get; private set; }
        public MetadataDefinitionName Name { get; private set; }

    }
}