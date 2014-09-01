using System;

namespace Lacjam.Core.Domain.MetadataDefinitions
{

    public class MetadataDefinitionProjection
    {

        public MetadataDefinitionProjection() { }

        public MetadataDefinitionProjection(Guid identity, string name, string datatype, string regex)
        {
            Identity = identity;
            Name = name;
            DataType = datatype;
            Regex = regex;
        }

        public MetadataDefinitionProjection(Guid identity, string name, string datatype)
            : this(identity, name, datatype, String.Empty)
        {

        }

        public MetadataDefinitionProjection WithNewRegularExpression(string regex)
        {
           return new MetadataDefinitionProjection(Identity,Name,DataType,regex);
        }

        public MetadataDefinitionProjection WithNewDetails(string name)
        {
            return new MetadataDefinitionProjection(Identity, name, DataType, Regex);
        }

        public Guid Identity { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Regex { get; set; }

        
    }
}
