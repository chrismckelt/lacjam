using System;
using System.Collections.Generic;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitions.Commands
{
    public class CreateMetadataDefinitionCommand : ICommand
    {

        public CreateMetadataDefinitionCommand(Guid identity, string name, string dataType, string regex, IEnumerable<string> values)
        {
            Identity = identity;
            Name = name;
            DataType = dataType;
            Regex = regex;
            Values = values;
        }

        public Guid Identity { get; private set; }
        public string Name { get; private set; }
        public string DataType { get; private set; }
        public string Regex { get; private set; }
        public IEnumerable<string> Values { get; private set; }
    }
}