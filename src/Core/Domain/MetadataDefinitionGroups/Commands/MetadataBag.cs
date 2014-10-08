using System;
using System.Collections.Generic;
using System.Linq;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
{
    public class MetadataBag
    {
        public MetadataBag()
        {
            Selection = new Guid[0];
        }

        public MetadataBag(IEnumerable<Guid> selections)
        {
            Selection = selections;
        }

        public IEnumerable<Guid> Selection { get; private set; } 
    }
}