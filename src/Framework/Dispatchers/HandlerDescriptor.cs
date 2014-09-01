using System;
using Lacjam.Framework.Events;

namespace Lacjam.Framework.Dispatchers
{
    public class HandlerDescriptor
    {
        public Action<IMessage, EventHeader> Handler { get; private set; }
        public Type EventType { get; private set; }

        public HandlerDescriptor(Type eventType, Action<IMessage, EventHeader> handler)
        {
            Handler = handler;
            EventType = eventType;
        }

        public void Handle<T>(T @event, EventHeader header) where T : IEvent
        {
            Handler(@event, header);
        }
    }
}