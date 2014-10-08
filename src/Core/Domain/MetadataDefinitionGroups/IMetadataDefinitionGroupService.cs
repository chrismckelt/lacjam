using System;
using System.Collections.Generic;
using Lacjam.Core.Domain.MetadataDefinitionGroups.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{
    public interface IMetadataDefinitionGroupService
    {
        void Create(Guid conceptIdentity, string name, string description, MetadataBag bag);
    }
}
