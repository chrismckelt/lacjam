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
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, string regex) : this(name,datatype)
        {
            _regex = regex;
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, IImmutableSet<AllowableValue> allowedValues)
            : this(name, datatype)
        {
            _allowedValues = allowedValues;
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, string regex, IImmutableSet<AllowableValue> allowedValues)
            : this(name, datatype,regex)
        {
            _allowedValues = allowedValues;
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, IImmutableSet<AllowableValue> allowedValues, bool isDeleted)
            : this(name, datatype, allowedValues)
        {
            IsDeleted = isDeleted;
        }

        public MetadataDefinitionState(MetadataDefinitionName name, IDataType datatype, string regex, IImmutableSet<AllowableValue> allowedValues, bool isDeleted)
            : this(name, datatype, regex, allowedValues)
        {
            IsDeleted = isDeleted;
        }

        public MetadataDefinitionState ReLabel(MetadataDefinitionName metadataDefinitionName)
        {
            return new MetadataDefinitionState(metadataDefinitionName, _datatype, _regex, _allowedValues);
        }

        public MetadataDefinitionState Delete()
        {
            return new MetadataDefinitionState(_name, _datatype, _regex, _allowedValues, true);
        }

        public MetadataDefinitionState AssignAllowableValue(AllowableValue value)
        {
            return new MetadataDefinitionState(_name, _datatype,_regex, _allowedValues.Add(value));
        }

        public MetadataDefinitionState ChangeRegularExpression(string regex)
        {
            return new MetadataDefinitionState(_name, _datatype, regex, _allowedValues);
        }

        public void GuardRegex()
        {
            if(!(_datatype is IRegexDataType))
                throw new InvariantGuardFailureException();
        }

        private readonly MetadataDefinitionName _name;
        private readonly IDataType _datatype;
        private readonly string _regex;
        private readonly IImmutableSet<AllowableValue> _allowedValues;

    }
}
