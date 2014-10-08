using Lacjam.Core.Domain.Entities.Commands;
using Lacjam.Core.Domain.Entities.Events;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lacjam.Core.Domain.Entities
{
    public class Entity : AggregateRoot<EntityState>
    {
        protected Entity() 
        {
        }

        public Entity(Guid identity, Guid metadataDefinitionGroupIdentity, EntityName name)
        {
            ApplyChange(new EntityCreatedEvent(identity, metadataDefinitionGroupIdentity, name));
        }

        protected void Apply(EntityCreatedEvent @event)
        {
            ApplyIdentity(@event.AggregateIdentity);
            State = new EntityState(@event.MetadataDefinitionGroupIdentity,@event.Name);
        }

        public void AddMetadataDefinitionValue(Guid metadataDefinitionIdentity, MetadataDefinitionName name, IDataType dataType, string regex, IValue supplied)
        {
            supplied.Validate(dataType, regex);
            ApplyChange(new EntityValueProvidedEvent(GetIdentity(), metadataDefinitionIdentity, name, dataType, regex, supplied));
        }

        public void Delete()
        {
            ApplyChange(new EntityDeletedEvent(GetIdentity()));
        }

        protected void Apply(EntityDeletedEvent @event)
        {
            State = State.Delete();
        }

        public virtual void Clear()
        {
            ApplyChange(new EntityMetadataRemovedEvent(GetIdentity()));
        }

        protected void Apply(EntityMetadataRemovedEvent @event)
        {
            State = State.ClearAllValues();
        }

        public void ChangeGroup(Guid definitionGroupId)
        {
            ApplyChange(new EntityChangedGroupEvent(GetIdentity(), definitionGroupId));
        }

        public void ChangeEntityName(EntityName name)
        {
            ApplyChange(new EntityRenamedEvent(GetIdentity(), name));
        }

        protected void Apply(EntityChangedGroupEvent @event)
        {
            State = State.ChangeGroup(@event.DefinitionGroupId);
        }
        
        protected void Apply(EntityRenamedEvent @event)
        {
            State = State.ChangeName(@event.Name);
        }

        public virtual void SynchronizeValues(IEnumerable<EntityValueBag> definitionValues)
        {
            var defns = definitionValues as EntityValueBag[] ?? definitionValues.ToArray();
            if (!defns.Any())
            {
                Clear();
                return;
            }

            var removed = State.GetSelectedDefintionIds().Where(defn => !defns.Select(x => x.MetadataDefinitionId).Contains(defn)).ToArray();
            var updated = defns.Where(x => State.GetSelectedDefintionIds().Contains(x.MetadataDefinitionId)).ToArray();
            var added = defns.Except(updated);


            foreach (var defn in removed)
                ApplyChange(new EntityMetadataDefinitionRemovedEvent(GetIdentity(), defn));

            foreach (var defn in updated)
                ApplyChange(new UpdatedEntityMetadataDefinitionValueEvent(GetIdentity(), defn.MetadataDefinitionId, new EntityName(defn.Name), DataTypeBuilder.Create(defn.DataType),
                    defn.Regex, defn.Selection.ToValue()));

            foreach (var defn in added)
            {
                AddMetadataDefinitionValue(defn.MetadataDefinitionId, new MetadataDefinitionName(defn.Name), DataTypeBuilder.Create(defn.DataType), defn.Regex, defn.Selection.ToValue());
            }
        }

        protected void Apply(EntityValueProvidedEvent @event)
        {
            State = State.AddOrUpdate(@event.MetadataDefinitionIdentity, @event.Name, @event.DataType, @event.Regex, @event.Value);
        }
        
        protected void Apply(UpdatedEntityMetadataDefinitionValueEvent @event)
        {
            State = State.UpdateValue(@event.DefinitionId, @event.Supplied);
        }

        protected void Apply(EntityMetadataDefinitionRemovedEvent @event)
        {
            State = State.RemoveValue(@event.DefinitionId);
        }
    }
}