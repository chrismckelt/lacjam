using System;
using System.Collections.Immutable;
using Lacjam.Core.Infrastructure;
using Lacjam.Framework.Exceptions;

namespace Lacjam.Core.Domain.MetadataDefinitions
{

    public class MetadataDefinitionState : TrackingBase 
    {

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype)
        {
            _name = name;
            _datatype = datatype;
            _regex = String.Empty;
            _allowedValues = ImmutableHashSet.Create<AllowableValue>();
            CreatedUtcDate = DateTime.UtcNow;
            IsDeleted = false;
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, string regex) : this(name,datatype)
        {
            _regex = regex;
            CreatedUtcDate = DateTime.UtcNow;
            IsDeleted = false;
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, IImmutableSet<AllowableValue> allowedValues)
            : this(name, datatype)
        {
            _allowedValues = allowedValues;
            CreatedUtcDate = DateTime.UtcNow;
            IsDeleted = false;
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, string regex, IImmutableSet<AllowableValue> allowedValues)
            : this(name, datatype,regex)
        {
            _allowedValues = allowedValues;
            CreatedUtcDate = DateTime.UtcNow;
            IsDeleted = false;
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, IImmutableSet<AllowableValue> allowedValues, bool isDeleted)
            : this(name, datatype, allowedValues)
        {
            IsDeleted = isDeleted;
            CreatedUtcDate = DateTime.UtcNow;
            IsDeleted = false;
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, string regex, IImmutableSet<AllowableValue> allowedValues, MetadataDefinitionDescription desc)
            : this(name, datatype, regex, allowedValues)
        {
            _description = desc;
            CreatedUtcDate = DateTime.UtcNow;
            IsDeleted = false;
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, string regex, IImmutableSet<AllowableValue> allowedValues, MetadataDefinitionDescription desc, bool isDeleted)
            : this(name, datatype, regex, allowedValues)
        {
            _description = desc;
            CreatedUtcDate = DateTime.UtcNow;
            IsDeleted = isDeleted;
        }

        public MetadataDefinitionState ReLabel(MetadataDefinitionName metadataDefinitionName)
        {
            return new MetadataDefinitionState(metadataDefinitionName, _datatype, _regex, _allowedValues);
        }

        public MetadataDefinitionState ReLabel(MetadataDefinitionDescription desc)
        {
            return new MetadataDefinitionState(_name, _datatype, _regex, _allowedValues,desc);
        }

        public MetadataDefinitionState Delete()
        {
            return new MetadataDefinitionState(_name, _datatype, _regex, _allowedValues,_description, true);
        }

        public MetadataDefinitionState AssignAllowableValue(AllowableValue value)
        {
            return new MetadataDefinitionState(_name, _datatype,_regex, _allowedValues.Add(value));
        }

        public MetadataDefinitionState ChangeRegularExpression(string regex)
        {
            return new MetadataDefinitionState(_name, _datatype, regex, _allowedValues);
        }

        public MetadataDefinitionState ChangeDataType(IDataType dataType)
        {
            return new MetadataDefinitionState(_name, dataType, _regex, ImmutableHashSet.Create<AllowableValue>());
        }

        public MetadataDefinitionState ClearAllowableValues()
        {
            return new MetadataDefinitionState(_name, _datatype, _regex, ImmutableHashSet.Create<AllowableValue>());
        }


        private readonly MetadataDefinitionName _name;
        private readonly IDataType _datatype;
        private readonly string _regex;
        private readonly IImmutableSet<AllowableValue> _allowedValues;
        private readonly MetadataDefinitionDescription _description;
    }
}
