using System;
using Lacjam.Core.Infrastructure;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
{
    public class ReLabelMetadataDefinitionGroupCommand : ICommand
    {
        private readonly TrackingBase _tracking;

        public ReLabelMetadataDefinitionGroupCommand(Guid identity, MetadataDefinitionGroupName name, MetadataDefinitionGroupDescription description,  TrackingBase tracking)
        {
            _tracking = tracking;
            Identity = identity;
            Name = name;
            Description = description;
        }

        public Guid Identity { get; private set; }
        public MetadataDefinitionGroupName Name { get; private set; }
        public MetadataDefinitionGroupDescription Description { get; private set; }
        public TrackingBase Tracking { get; private set; }
    }
}