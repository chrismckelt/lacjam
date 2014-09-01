namespace Lacjam.Framework.Events
{
    public class EventErrorDescriptor
    {
        public virtual long Seq { get; set; }
        public virtual EventData EventData { get; set; }
        public virtual EventHeader Header { get; protected set; }
        public virtual string EventType { get { return EventData.Type; }  }

        protected EventErrorDescriptor()
        {
        }

        public EventErrorDescriptor(IEvent eventData, EventHeader header)
            : this()
        {
            EventData = new EventData(eventData.GetType().Name, eventData);
            Header = header;
        }

        public EventErrorDescriptor(DynamicEvent eventData, string eventType, EventHeader header)
            : this()
        {
            EventData = new EventData(eventType, eventData);
            Header = header;
        }


        public virtual T GetEventData<T>() where T : IEvent
        {
            return (T)EventData.Event;
        }

        public virtual EventDescriptor ToEventDescriptor()
        {
            var result = new EventDescriptor(EventData.Event, Header);
            return result;
        }
    }
}