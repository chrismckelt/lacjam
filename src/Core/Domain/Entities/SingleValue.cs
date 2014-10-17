using System;
using System.Collections.Generic;

using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Framework.Extensions;

namespace Lacjam.Core.Domain.Entities
{
    public class SingleValue : IValue
    {

        protected SingleValue(){}

        public SingleValue(object value)
        {
            _value = value;
        }

        public object Get()
        {
            return _value;
        }

        public void Validate(IDataType dataType, string regex)
        {
            string value = _value.As<String>();
            DataTypeValidator.ValidateValueAgainst(value, regex);
        }

        public IEnumerable<string> GetStream()
        {
            return new List<string>
            {
                _value.As<String>()
            };
        }

        private readonly object _value;
    }
}