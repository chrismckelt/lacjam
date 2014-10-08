using System;
using Lacjam.Framework.Commands;

namespace Lacjam.Core.Domain.MetadataDefinitions.Commands
{
    public class ChangeMetadataDefinitionDataTypeCommand : ICommand
    {
        public ChangeMetadataDefinitionDataTypeCommand(Guid identity, string datatype)
        {
            Identity = identity;
            DataType = datatype;
        }

        public Guid Identity { get; private set; }
        public string DataType { get; private set; }
    }
}