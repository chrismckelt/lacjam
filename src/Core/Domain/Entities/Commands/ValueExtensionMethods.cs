using System.Collections.Generic;
using System.Linq;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public static class ValueExtensionMethods
    {
        public static IValue ToValue(this IEnumerable<string> values)
        {
            if(values.Count() != 1)
                return new MultiValue(values);
            return new SingleValue(values.First());
        }
    }
}