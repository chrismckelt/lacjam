using System;

namespace Lacjam.Framework.Events
{
    public class DynamicEvent : IEvent
    {
        protected DynamicEvent()
        {
        }

        public DynamicEvent(string json)
        {
            Json = json;
        }
        
        public string Json { get; set; } 
        public string EventType { get; set; }
        public Guid AggregateIdentity { get; set; }
    }
}