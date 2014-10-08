using System;
using System.Collections.Generic;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class CreateEntityCommand : ICommand
    {
        public CreateEntityCommand(Guid identity, Guid metadataDefinitionGroupIdentity, string name, IEnumerable<ValueSet> values)
        {
            Identity = identity;
            MetadataDefinitionGroupIdentity = metadataDefinitionGroupIdentity;
            Name = name;
            Values = values;
        }

        public Guid Identity { get; private set; }
        public Guid MetadataDefinitionGroupIdentity { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<ValueSet> Values { get; private set; } 
    }

    public class ValueSet
    {
        public Guid MetadataDefinitionIdentity { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Regex { get; set; }
        public List<string> Values { get; set; }
    }
}