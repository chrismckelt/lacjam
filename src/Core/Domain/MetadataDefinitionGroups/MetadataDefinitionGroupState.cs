using System;
using System.Collections.Immutable;
using Lacjam.Core.Infrastructure;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{
    public class MetadataDefinitionGroupState : TrackingBase
    {
        private readonly MetadataDefinitionGroupName _name;
        private readonly MetadataDefinitionGroupDescription _description;
        private readonly IImmutableSet<Guid> _attributes;

        protected MetadataDefinitionGroupState()
        {
        }

        public MetadataDefinitionGroupState(MetadataDefinitionGroupName name, MetadataDefinitionGroupDescription description)
        {
            if (name == null)
                throw new InvariantGuardFailureException();

            if (description == null)
                throw new InvariantGuardFailureException();

            _name = name;
            _description = description;
            _attributes = ImmutableHashSet.Create<Guid>();
        }

        public MetadataDefinitionGroupState(MetadataDefinitionGroupName name, MetadataDefinitionGroupDescription description, IImmutableSet<Guid> set)
            : this(name, description)
        {
            _attributes = set;
        }

        public MetadataDefinitionGroupState(MetadataDefinitionGroupName name, MetadataDefinitionGroupDescription description, IImmutableSet<Guid> set, bool deleted)
            : this(name, description,set)
        {
            this.IsDeleted = deleted;
        }

        public bool AttributeExists(Guid attribute)
        {
            return _attributes.Contains(attribute);
        }

        public MetadataDefinitionGroupState AttachAttribute(Guid attribute)
        {
            return new MetadataDefinitionGroupState(_name, _description, _attributes.Add(attribute));
        }

        public MetadataDefinitionGroupState MarkDeleted()
        {
            return new MetadataDefinitionGroupState(_name, _description, _attributes);
        }

        public MetadataDefinitionGroupState ChangeName(MetadataDefinitionGroupName name)
        {
            return new MetadataDefinitionGroupState(name, _description, _attributes);
        }

        public MetadataDefinitionGroupState ChangeDescription(MetadataDefinitionGroupDescription entityDescription)
        {
            return new MetadataDefinitionGroupState(_name, entityDescription, _attributes);
        }

        public MetadataDefinitionGroupState ClearAttributes()
        {
            return new MetadataDefinitionGroupState(_name, _description, ImmutableHashSet.Create<Guid>());
        }
    }

}
