using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Attribute.Events
{

    public class AttributeCreatedEvent : Event
    {
        public AttributeCreatedEvent(Guid identity, AttributeName name, bool allowMultiple)
            : base(identity)
        {
            Name = name;
            AllowMultiple = allowMultiple;
        }

        public AttributeName Name{ get; private set; }

        public bool AllowMultiple{ get; private set; }
    }

}
