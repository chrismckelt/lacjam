using System;

namespace Lacjam.Framework.Events
{
    public class SequencedEvent
    {
        public SequencedEvent(EventDescriptor @event)
        {
            Sequence = @event.Seq;
            Event = @event;
            AggregateId = Event.Header.AggregateId;
        }

        public bool HasEvent()
        {
           return  Event != null;
        }

        public EventDescriptor Event { get; private set; }
        public long Sequence { get; private set; }
        public Guid AggregateId { get; private set; }

        public string EventType
        {
            get { return Event.EventData.Type; }
        }

    }
}