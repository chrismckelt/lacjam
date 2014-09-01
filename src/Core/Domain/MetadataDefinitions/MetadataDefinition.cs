using System;
using Lacjam.Core.Domain.MetadataDefinitions.Events;
using Lacjam.Framework.Exceptions;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.MetadataDefinitions
{

    public class MetadataDefinition : AggregateRoot<MetadataDefinitionState>
    {

        public MetadataDefinition(Guid id, MetadataDefinitionName name, IDataType datatype)
        {
            ApplyChange(new MetadataDefinitionCreatedEvent(id, name, datatype));
        }

        public MetadataDefinition(Guid id, MetadataDefinitionName name, IDataType datatype, string regex)
        {
            ApplyChange(new MetadataDefinitionCreatedEvent(id, name, datatype, regex));
        }

        protected void Apply(MetadataDefinitionCreatedEvent @event)
        {
            ApplyIdentity(@event.AggregateIdentity);
            State = new MetadataDefinitionState(@event.Name, @event.DataType, @event.Regex);
        }

        public void ChangeRegularExpression(string regex)
        {
            State.GuardRegex();
            ApplyChange(new MetadataDefinitionRegexChangedEvent(GetIdentity(), regex));
        }

        protected void Apply(MetadataDefinitionRegexChangedEvent @event)
        {
            State = State.ChangeRegularExpression(@event.Regex);
        }

        public void AddAllowableValue(AllowableValue value)
        {
            if (value == null)
                throw new InvariantGuardFailureException();

            ApplyChange(new MetadataDefinitionAddAllowableValueEvent(GetIdentity(), value));
        }

        protected void Apply(MetadataDefinitionAddAllowableValueEvent @event)
        {
            State = State.AssignAllowableValue(@event.Value);
        }

        public void Delete()
        {
            if (State.IsDeleted)
                throw new InvariantGuardFailureException();

            ApplyChange(new MetadataDefinitionDeletedEvent(GetIdentity()));
        }

        protected void Apply(MetadataDefinitionDeletedEvent @event)
        {
            State = State.Delete();
        }

        public void ReLabel(MetadataDefinitionName name)
        {
            if (name == null)
                throw new InvariantGuardFailureException();

            ApplyChange(new ReLabelMetadataDefinitionEvent(GetIdentity(), name.Name));
        }

        protected void Apply(ReLabelMetadataDefinitionEvent @event)
        {
            State = State.ReLabel(new MetadataDefinitionName(@event.Name));
        }
    }


}