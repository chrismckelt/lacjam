using System;
using System.Collections.Generic;
using System.Reflection;
using Lacjam.Framework.Events;

namespace Lacjam.Framework.Model
{
    public static class AggregateBuilder
    {
        public static TAggregate LoadFromEvents<TAggregate>(Guid identity, IEnumerable<IEvent> events) where TAggregate : IAggregateRoot
        {
            var aggregateType = typeof (TAggregate);
            var ctor = aggregateType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
                new Type[] {}, null);

            var result = (TAggregate)(ctor.Invoke(new object[] {}));

            var setIdentityMethod = aggregateType.GetMethod("ApplyIdentity", BindingFlags.Instance | BindingFlags.NonPublic);
            setIdentityMethod.Invoke(result, new object[] { identity });

            var loadEventsMethod = aggregateType.GetMethod("ApplyChanges", BindingFlags.Instance | BindingFlags.NonPublic);
            loadEventsMethod.Invoke(result, new object[] {@events});

            return result;
        }
    }
}