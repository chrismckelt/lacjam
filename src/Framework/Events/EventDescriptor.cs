using System;

namespace Lacjam.Framework.Events
{
    public class EventDescriptor
    {
        protected EventDescriptor()
        {
        }

        public EventDescriptor(IEvent eventData, EventHeader header)
            : this()
        {
            var type = eventData.GetType();
            var name = String.Format("{0},{1}", type.FullName, type.Assembly.GetName().Name);
            EventData = new EventData(name, eventData);
            Header = header;
        }

        public EventDescriptor(DynamicEvent eventData, string eventType, EventHeader header)
            : this()
        {
            EventData = new EventData(eventType, eventData);
            Header = header;
        }

        public virtual EventErrorDescriptor ToErrorEventDescriptor()
        {
            var clone = new EventErrorDescriptor(EventData.Event, Header);
            return clone;
        }

        public virtual Guid EventId { get; set; }
        public virtual long Seq { get; set; }
        public virtual EventData EventData { get; set; }
        public virtual EventHeader Header { get; protected set; }

    }
}