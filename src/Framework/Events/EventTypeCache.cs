using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lacjam.Framework.Events
{
    public static class EventTypeCache
    {
        private static readonly ConcurrentDictionary<string, Type> Types = new ConcurrentDictionary<string, Type>();
        private static readonly List<Assembly> Assemblies = new List<Assembly>();

        public static void AddAssembly(Assembly assembly)
        {
            Assemblies.Add(assembly);
        }
        public static void AddAssemblyFromType(Type type)
        {
            Assemblies.Add(type.Assembly);
        }
        public static void AddAssemblyFromType<T>()
        {
            Assemblies.Add(typeof(T).Assembly);
        }

        public static Type GetType(string typeName)
        {
            Type type;
            if (Types.TryGetValue(typeName, out type)) 
                return type;

            type = Assemblies.SelectMany(a => a.GetTypes()).FirstOrDefault(x => string.Equals(x.Name, typeName, StringComparison.InvariantCultureIgnoreCase));
            Types[typeName] = type;
            return type;
        }
    }
}