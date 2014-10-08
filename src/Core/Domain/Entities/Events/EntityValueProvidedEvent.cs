using System;
using Newtonsoft.Json;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Entities.Events
{
    public class EntityValueProvidedEvent : Event
    {

        protected EntityValueProvidedEvent()
        {
        }

        public EntityValueProvidedEvent(Guid aggregateIdentity, Guid metadataDefinitionIdentity, MetadataDefinitionName name, IDataType dataType, string regex, IValue value)
            : base(aggregateIdentity)
        {
            MetadataDefinitionIdentity = metadataDefinitionIdentity;
            Name = name;
            DataType = dataType;
            Regex = regex;
            Value = value;
        }

        public Guid MetadataDefinitionIdentity { get; private set; }
        public MetadataDefinitionName Name { get; private set; }
        public IDataType DataType { get; private set; }
        public string Regex { get; private set; }
        public IValue Value { get; private set; }
    }
}