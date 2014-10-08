using System;
using Structerre.MetaStore.Framework.Events;

namespace Structerre.MetaStore.Core.Domain.Attribute.Events
{
    public class AttributeDeletedEvent : Event
    {
        public AttributeDeletedEvent(Guid identity) : base(identity)
        {

        }

    }
}