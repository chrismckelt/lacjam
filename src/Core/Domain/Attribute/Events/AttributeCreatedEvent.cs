using System;
using Structerre.MetaStore.Framework.Events;

namespace Structerre.MetaStore.Core.Domain.Attribute.Events
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
