using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Attribute.Events
{
    public class AttributeDeletedEvent : Event
    {
        public AttributeDeletedEvent(Guid identity) : base(identity)
        {

        }

    }
}