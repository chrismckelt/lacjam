using System;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitions.Commands
{
    public class ReLabelMetadataDefinitionDescriptionCommand : ICommand
    {

        public ReLabelMetadataDefinitionDescriptionCommand(Guid identity, MetadataDefinitionDescription description)
        {
            Identity = identity;
            this.Description = description;
        }

        public Guid Identity { get; private set; }
        public MetadataDefinitionDescription Description { get; private set; }

    }
}