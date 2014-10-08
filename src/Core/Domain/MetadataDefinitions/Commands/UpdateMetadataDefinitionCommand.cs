using System;
using System.Collections.Generic;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitions.Commands
{
    public class UpdateMetadataDefinitionValuesCommand : ICommand
    {
        public UpdateMetadataDefinitionValuesCommand(Guid identity, IEnumerable<string> values)
        {
            Identity = identity;
            Values = values;
        }

        public Guid Identity { get; private set; }
        public IEnumerable<string> Values { get; private set; }
    }

}