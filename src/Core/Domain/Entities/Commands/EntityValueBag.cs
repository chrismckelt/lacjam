using System;
using System.Collections.Generic;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class EntityValueBag
    {
        public EntityValueBag()
        {
            Selection = new List<string>();
        }

        public EntityValueBag(IEnumerable<string> selections)
        {
            Selection = selections;
        }

        public EntityValueBag(Guid identity, string name, Guid metadataDefinitionIdentity, string dataType, string regex, HashSet<string> values)
        {
            Identity = identity;
            Name = name;
            MetadataDefinitionId = metadataDefinitionIdentity;
            DataType = dataType;
            Regex = regex;
            Selection = values;
        }

        public Guid Identity { get; set; }
        public Guid MetadataDefinitionId { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Regex { get; set; }
        public IEnumerable<string> Selection { get; private set; }
    }
}