using System;
using System.Collections.Generic;
using System.Linq;
using Lacjam.Framework.Extensions;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public static class DataTypeBuilder
    {
        public static IDataType Create(string tag)
        {
            foreach (var dt in GetDataTypes().Where(dt => dt.Value.Tag != null && String.Equals(tag, dt.Value.Tag, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (dt.Value != null)
                    return Activator.CreateInstance(dt.Key).As<IDataType>();
                return dt.Value;
            }

            return null;
        }

        public static IEnumerable<KeyValuePair<Type, IDataType>> GetDataTypes()
        {
            var results = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.Contains("Lacjam"))
               .SelectMany(assembly => assembly.GetExportedTypes())
               .Where(type => !type.IsInterface)
               .Where(x => typeof(IDataType).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            IDictionary<Type,IDataType>  dic = new Dictionary<Type, IDataType>();

            foreach (var x in results)
            {
                dic.Add(new KeyValuePair<Type, IDataType>(x, Activator.CreateInstance(x).As<IDataType>()));
            }

            return dic;
        }

    }
}