using System;
using Lacjam.Core.Domain.MetadataDefinitionGroups.Events;
using Lacjam.Framework.Exceptions;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{
    public class MetadataDefinitionGroup : AggregateRoot<MetadataDefinitionGroupState>
    {

        protected MetadataDefinitionGroup()
        {

        }

        public MetadataDefinitionGroup(Guid identity, MetadataDefinitionGroupName name, MetadataDefinitionGroupDescription description)
        {
            ApplyChange(new MetadataDefinitionGroupCreatedEvent(identity, name, description));
        }

        protected virtual void Apply(MetadataDefinitionGroupCreatedEvent @event)
        {
            ApplyIdentity(@event.AggregateIdentity);
            State = new MetadataDefinitionGroupState(@event.Name, @event.Description);
        }

        public void AssociateAttribute(Guid attribute)
        {

            if (attribute == Guid.Empty)
                throw new InvariantGuardFailureException();

            if (State.AttributeExists(attribute))
                throw new InvariantGuardFailureException();

            ApplyChange(new MetadataDefinitionGroupAssociatedMetadataEvent(GetIdentity(), attribute));

        }

        protected virtual void Apply(MetadataDefinitionGroupAssociatedMetadataEvent @event)
        {
            State = State.AttachAttribute(@event.DefinitionId);
        }

        public void Delete()
        {
            if (State.IsDeleted)
                throw new InvariantGuardFailureException();

            ApplyChange(new MetadataDefinitionGroupDeletedEvent(GetIdentity()));
        }

        protected virtual void Apply(MetadataDefinitionGroupDeletedEvent @event)
        {
            State = State.MarkDeleted();
        }

        public void ChangeName(MetadataDefinitionGroupName name)
        {
            if (name == null)
                throw new InvariantGuardFailureException();

            ApplyChange(new MetadataDefinitionGroupNameChangedEvent(GetIdentity(), name));

        }

        protected virtual void Apply(MetadataDefinitionGroupNameChangedEvent @event)
        {
            State = State.ChangeName(@event.Name);
        }

        public void ChangeDescription(MetadataDefinitionGroupDescription description)
        {
            if (description == null)
                throw new InvariantGuardFailureException();

            ApplyChange(new MetadataDefinitionGroupDescriptionChangedEvent(GetIdentity(), description));

        }

        protected virtual void Apply(MetadataDefinitionGroupDescriptionChangedEvent @event)
        {
            State = State.ChangeDescription(@event.Description);
        }

        public virtual void ClearAttributes()
        {
            ApplyChange(new MetadataDefinitionGroupAttributesClearedEvent(GetIdentity()));
        }

        protected virtual void Apply(MetadataDefinitionGroupAttributesClearedEvent @event)
        {
            State = State.ClearAttributes();
        }
    }

}
