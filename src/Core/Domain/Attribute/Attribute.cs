using System;
using Lacjam.Core.Domain.Attribute.Events;
using Lacjam.Framework.Exceptions;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.Attribute
{
    public class Attribute : AggregateRoot<AttributeState>
    {
        public Attribute(Guid id, AttributeName name, bool allowMultiple)
        {
            ApplyChange(new AttributeCreatedEvent(id, name, allowMultiple));
        }

        protected void Apply(AttributeCreatedEvent @event)
        {
            ApplyIdentity(@event.AggregateIdentity);
            State = new AttributeState(@event.Name, @event.AllowMultiple);
        }

        public void AddAllowableValue(AttributeValue value)
        {
            if(value == null)
                throw new InvariantGuardFailureException();

            State = State.AssignAllowableValue(value);
        }

        public void Delete()
        {
            if (State.IsDeleted)
                throw new InvariantGuardFailureException();

            ApplyChange(new AttributeDeletedEvent(GetIdentity()));
        }

        protected void Apply(AttributeDeletedEvent @event)
        {
            State = State.Delete();
        }

        public void ReLabel(AttributeName name)
        {
            if (name == null)
                throw new InvariantGuardFailureException();

            ApplyChange(new ReLabelAttributeEvent(GetIdentity(),name.Name));
        }

        protected void Apply(ReLabelAttributeEvent @event)
        {
            State = State.ReLabel(new AttributeName(@event.Name));
        }
    }
}
