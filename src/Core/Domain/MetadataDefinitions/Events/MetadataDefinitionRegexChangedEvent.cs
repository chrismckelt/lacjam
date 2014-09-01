using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.MetadataDefinitions.Events
{
    public class MetadataDefinitionRegexChangedEvent : Event
    {
        public MetadataDefinitionRegexChangedEvent(Guid aggregateIdentity, string regex)
            : base(aggregateIdentity)
        {

            Regex = regex;
        }

        public string Regex { get; private set; }
    }
}