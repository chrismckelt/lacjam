using System;
using Structerre.MetaStore.Core.Domain.Attribute.Events;
using Structerre.MetaStore.Core.Domain.Entity.Events;
using Structerre.MetaStore.Framework.Exceptions;
using Structerre.MetaStore.Framework.Model;

namespace Structerre.MetaStore.Core.Domain.Entity
{
    public class Entity : AggregateRoot<EntityState>
    {
        public Entity(Guid identity, EntityName name, EntityDescription description)
        {
            ApplyChange(new EntityCreatedEvent(identity, name, description));
        }

        protected virtual void Apply(EntityCreatedEvent @event)
        {
            ApplyIdentity(@event.AggregateIdentity);
            State = new EntityState(@event.Name, @event.Description);
        }

        public void AssociateAttribute(Guid attribute)
        {

            if (attribute == Guid.Empty)
                throw new InvariantGuardFailureException();

            if (State.AttributeExists(attribute))
                throw new InvariantGuardFailureException();

            ApplyChange(new EntityAttributeAssociatedEvent(GetIdentity(), attribute));

        }

        protected virtual void Apply(EntityAttributeAssociatedEvent @event)
        {
            State = State.AttachAttribute(@event.Attribute);
        }

        public void Delete()
        {
            if(State.IsDeleted)
                throw new InvariantGuardFailureException();

            ApplyChange(new AttributeDeletedEvent(GetIdentity()));
        }

        protected virtual void Apply(EntityDeletedEvent @event)
        {
            State = State.MarkDeleted();
        }

        public void ChangeName(EntityName name)
        {
            if (name == null)
                throw new InvariantGuardFailureException();

            ApplyChange(new EntityNameChangedEvent(GetIdentity(), name));

        }

        protected virtual void Apply(EntityNameChangedEvent @event)
        {
            State = State.ChangeName(@event.Name);
        }

        public void ChangeDescription(EntityDescription description)
        {
            if (description == null)
                throw new InvariantGuardFailureException();

            ApplyChange(new EntityDescriptionChangedEvent(GetIdentity(), description));

        }

        protected virtual void Apply(EntityDescriptionChangedEvent @event)
        {
            State = State.ChangeDescription(@event.Description);
        }
    }

}
