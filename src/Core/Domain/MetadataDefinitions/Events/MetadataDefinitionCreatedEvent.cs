using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{

    public class MetadataDefinitionCreatedEvent : Event
    {
        public MetadataDefinitionCreatedEvent(Guid aggregateIdentity, MetadataDefinitionName name, IDataType datatype)
            : base(aggregateIdentity)
        {
            Name = name;
            DataType = datatype;
        }

        public MetadataDefinitionCreatedEvent(Guid aggregateIdentity, MetadataDefinitionName name, IDataType datatype, string regex)
            : base(aggregateIdentity)
        {
            Name = name;
            DataType = datatype;
            Regex = regex;
        }

        public MetadataDefinitionName Name{ get; private set; }
        public IDataType DataType{ get; private set; }
        public string Regex { get; private set; }
    }
}
