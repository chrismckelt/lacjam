using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Framework.Extensions;

namespace Lacjam.Core.Domain.Entities
{
    public class MultiValue : IValue
    {

        protected MultiValue() { }

        public MultiValue(IEnumerable values)
        {
            _values = values;
        }

        public object Get()
        {
            return _values;
        }

        public void Validate(IDataType dataType, string regex)
        {
            
        }

        public IEnumerable<string> GetStream()
        {
            return from object value in _values select value.As<String>();
        }

        private readonly IEnumerable _values;
    }
}