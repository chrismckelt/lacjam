using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Lacjam.Core.Domain.Entities
{
    public class EntityState : TrackingBase
    {
        public EntityState(Guid group, EntityName name) : this(group, name, ImmutableHashSet.Create<EntityValueSetState>())
        {
            CreatedUtcDate = DateTime.UtcNow;
        }

        private EntityState(Guid group, EntityName name, IImmutableSet<EntityValueSetState> values) : this(group, name, values, false)
        {
        }

        private EntityState(Guid group, EntityName name, IImmutableSet<EntityValueSetState> values, bool deleted)
        {
            _group = group;
            _name = name;
            _values = values;
            LastModifiedUtcDate = DateTime.UtcNow;
            IsDeleted = deleted;
        }

        public EntityState AddOrUpdate(Guid metadataDefinitionIdentity, MetadataDefinitionName name, IDataType dataType, string regex, IValue supplied)
        {

            if (_values.All(x => x.MetadataDefinitionIdentity != metadataDefinitionIdentity))
                return new EntityState(_group, _name, _values.Add(new EntityValueSetState(metadataDefinitionIdentity, name, dataType, regex, supplied)));

            var element = _values.First(x => x.MetadataDefinitionIdentity == metadataDefinitionIdentity);

            element.Name = name;
            element.DataType = dataType;
            element.Regex = regex;
            element.Values = supplied;

            return this;

        }

        public EntityState Delete()
        {
            return new EntityState(_group, _name, _values, true);
        }

        public EntityState ChangeName(EntityName name)
        {
            return new EntityState(_group, name, _values, false);
        }

        public EntityState ChangeGroup(Guid definitionGroupId)
        {
            return new EntityState(definitionGroupId, _name, _values, false);
        }

        public IEnumerable<Guid> GetSelectedDefintionIds()
        {
            return _values.Select(x => x.MetadataDefinitionIdentity);
        }

        public EntityState ClearAllValues()
        {
            return new EntityState(_group, _name, ImmutableHashSet.Create<EntityValueSetState>(), false);
        }

        public EntityState UpdateValue(Guid definitionId, IValue supplied)
        {
            var element = _values.First(x => x.MetadataDefinitionIdentity == definitionId);
            element.Values = supplied;

            return this;
        }

        private readonly Guid _group;
        private readonly EntityName _name;
        private readonly IImmutableSet<EntityValueSetState> _values;

        public EntityState RemoveValue(Guid definitionId)
        {
            foreach (var val in  _values.Where(x => x.MetadataDefinitionIdentity == definitionId).ToArray())
            {
                _values.Remove(val);
            }

            return this;
        }
    }
}