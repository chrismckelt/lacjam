using System;
using Structerre.MetaStore.Framework.Events;

namespace Structerre.MetaStore.Core.Domain.Attribute.Events
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