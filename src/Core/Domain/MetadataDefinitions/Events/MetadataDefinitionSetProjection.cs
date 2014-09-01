using System;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionSetProjection
    {

        public MetadataDefinitionSetProjection(){}

        public MetadataDefinitionSetProjection(Guid definitionId, string value)
        {
            DefinitionId = definitionId;
            Value = value;
        }

        public Guid Identity { get; set; }
        public Guid DefinitionId { get; set; }
        public string Value { get; set; }
    }
}