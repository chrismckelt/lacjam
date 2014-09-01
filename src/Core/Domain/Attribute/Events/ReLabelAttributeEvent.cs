using System;
using Lacjam.Framework.Events;

namespace Lacjam.Core.Domain.Attribute.Events
{
    public class ReLabelAttributeEvent : Event
    {
        public ReLabelAttributeEvent(Guid identity, string name) : base(identity)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}