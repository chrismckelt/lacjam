using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{

    public class MetadataDefinitionCreatedEvent : Event
    {

        public MetadataDefinitionCreatedEvent()
        {
        }

        public MetadataDefinitionCreatedEvent(Guid aggregateIdentity) : base(aggregateIdentity) // all childrens contructors must have same variable name for JSON Serialization to work
        {
            AggregateIdentity = aggregateIdentity;
        }

        public MetadataDefinitionCreatedEvent(Guid aggregateIdentity, MetadataDefinitionName name, IDataType datatype)
            : base(aggregateIdentity)
        {
            Name = name;
            DataType = datatype.Tag;
        }

        public MetadataDefinitionCreatedEvent(Guid aggregateIdentity, MetadataDefinitionName name, IDataType datatype, string regex)
            : base(aggregateIdentity)
        {
            Name = name;
            DataType = datatype.Tag;
            Regex = regex;
        }

        public MetadataDefinitionCreatedEvent(Guid aggregateIdentity, MetadataDefinitionName name, IDataType datatype, string regex, MetadataDefinitionDescription description)
            : base(aggregateIdentity)
        {
            Name = name;
            DataType = datatype.Tag;
            Regex = regex;
            Description = description;
        }

        public MetadataDefinitionName Name{ get; set; }
        public MetadataDefinitionDescription Description { get; set; }
        public string DataType{ get; set; }
        public string Regex { get; set; }
    }
}
