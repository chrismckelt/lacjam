using System;
using Lacjam.Core.Domain.MetadataDefinitions;

namespace Lacjam.Core.Domain.Entities
{
    public class EntityValueSetState
    {

        protected EntityValueSetState()
        {
        }

        public EntityValueSetState(Guid metadataDefinitionIdentity, MetadataDefinitionName name, IDataType dataType, string regex, IValue supplied)
        {
            MetadataDefinitionIdentity = metadataDefinitionIdentity;
            Name = name;
            DataType = dataType;
            Regex = regex;
            Values = supplied;
        }

        public Guid MetadataDefinitionIdentity { get; set; }
        public MetadataDefinitionName Name { get; set; }
        public string Regex { get; set; }
        public IDataType DataType { get; set; }
        public IValue Values { get; set; } 
    }
}