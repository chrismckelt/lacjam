using System;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Entities.Events
{
    public class UpdatedEntityMetadataDefinitionValueEvent : Event
    {
        public UpdatedEntityMetadataDefinitionValueEvent(Guid aggregateIdentity,Guid definitionId, EntityName name, IDataType dataType, string regex, IValue supplied) : base(aggregateIdentity)
        {
            DefinitionId = definitionId;
            Name = name;
            DataType = dataType;
            Regex = regex;
            Supplied = supplied;
        }

        public Guid DefinitionId { get; private set; }
        public EntityName Name { get; private set; }
        public IDataType DataType { get; private set; }
        public string Regex { get; private set; }
        public IValue Supplied { get; private set; }
    }
}