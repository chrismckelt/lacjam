using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionDataTypeChangedEvent : Event
    {
        public MetadataDefinitionDataTypeChangedEvent(Guid aggregateIdentity, string dataType, string regex) : base(aggregateIdentity)
        {
            DataType = dataType;
            Regex = regex;
        }

        public string DataType { get; private set; }
        public string Regex { get; private set; }
    }
}