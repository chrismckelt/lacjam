using System;
using System.Collections.Generic;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
{
    public class MetadataBag
    {
        public MetadataBag()
        {
            Selection = new List<Guid>();
        }

        public MetadataBag(List<Guid> selections)
        {
            Selection = selections;
        }

        public List<Guid> Selection { get; private set; } 
    }
}