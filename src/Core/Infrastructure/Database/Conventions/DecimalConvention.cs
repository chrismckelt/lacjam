using System;

namespace Lacjam.Core.Infrastructure.Database.Conventions
{
    public class DecimalConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Type == typeof (decimal) || Nullable.GetUnderlyingType(instance.Property.PropertyType) == typeof(decimal)) 
            {
                instance.Precision(27);
                instance.Scale(15);
            }
        }
    }
}