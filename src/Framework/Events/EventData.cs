using System;

namespace Lacjam.Framework.Events
{
    [Serializable]
    public class EventData
    {
        public virtual string Type { get; protected set; }
        public virtual IEvent Event { get; protected set; }

        protected EventData()
        {
        }

        public EventData(string type, IEvent @event)
        {
            Type = type;
            Event = @event;
        }

        protected bool Equals(EventData other)
        {
            return string.Equals(Type, other.Type) && Equals(Event, other.Event);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EventData) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type != null ? Type.GetHashCode() : 0)*397) ^ (Event != null ? Event.GetHashCode() : 0);
            }
        }

        public static bool operator ==(EventData left, EventData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EventData left, EventData right)
        {
            return !Equals(left, right);
        }
    }
}