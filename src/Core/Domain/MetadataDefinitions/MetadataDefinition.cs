using System;
using Lacjam.Core.Domain.MetadataDefinitions.Events;
using Lacjam.Framework.Exceptions;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class MetadataDefinition : AggregateRoot<MetadataDefinitionState>
    {
        public static readonly string[] KeywordDefinitions = {"keywords", "description", "tags" };

        protected MetadataDefinition(): base() // required for JSON
        {
        }

        public MetadataDefinition(Guid id, MetadataDefinitionName name, IDataType datatype)
        {
            ApplyChange(new MetadataDefinitionCreatedEvent(id, name, datatype));
        }

        public MetadataDefinition(Guid id, MetadataDefinitionName name, MetadataDefinitionDescription description, IDataType datatype, string regex)
        {
            ApplyChange(new MetadataDefinitionCreatedEvent(id, name, datatype, regex, description));
        }

        protected void Apply(MetadataDefinitionCreatedEvent @event)
        {
            ApplyIdentity(@event.AggregateIdentity);
            var dt = DataTypeBuilder.Create(@event.DataType);
            State = new MetadataDefinitionState(@event.Name, dt, @event.Regex);
        }

        public void ChangeRegularExpression(string regex)
        {
            ApplyChange(new MetadataDefinitionRegexChangedEvent(GetIdentity(), regex));
        }

        protected void Apply(MetadataDefinitionRegexChangedEvent @event)
        {
            State = State.ChangeRegularExpression(@event.Regex);
        }

        public void AddAllowableValue(AllowableValue value)
        {
            if (value == null)
                throw new InvariantGuardFailureException("value");

            ApplyChange(new MetadataDefinitionAddAllowableValueEvent(GetIdentity(), value));
        }

        protected void Apply(MetadataDefinitionAddAllowableValueEvent @event)
        {
            State = State.AssignAllowableValue(@event.Value);
        }

        public void ClearValues()
        {
            ApplyChange(new MetadataDefinitionClearAllowableValuesEvent(GetIdentity()));
        }

        protected void Apply(MetadataDefinitionClearAllowableValuesEvent @event)
        {
            State = State.ClearAllowableValues();
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

        public void ReLabel(MetadataDefinitionDescription description)
        {
            if (description == null)
                throw new InvariantGuardFailureException("description");

            ApplyChange(new ReLabelMetadataDefinitionDescriptionEvent(GetIdentity(), description.Description));
        }

        protected void Apply(ReLabelMetadataDefinitionEvent @event)
        {
            State = State.ReLabel(new MetadataDefinitionName(@event.Name));
        }

        protected void Apply(ReLabelMetadataDefinitionDescriptionEvent @event)
        {
            State = State.ReLabel(new MetadataDefinitionDescription(@event.Description));
        }

        public void ChangeDataType(string dataType)
        {
            var dt = DataTypeBuilder.Create(dataType);

            if(dt == null)
                throw new InvalidDataTypeException();

            var regex = String.Empty;

            if (dt is IRegexDataType)
                regex = ((IRegexDataType)dt).Regex; // Copy Default

            ApplyChange(new MetadataDefinitionDataTypeChangedEvent(GetIdentity(), dt.Tag, regex));

        }

        protected void Apply(MetadataDefinitionDataTypeChangedEvent @event)
        {
            var dt = DataTypeBuilder.Create(@event.DataType);
            State = State.ChangeDataType(dt);
        }

        
    }
}