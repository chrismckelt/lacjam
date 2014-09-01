using System;
using Lacjam.Framework.Time;
using Lacjam.Framework.Utilities;

namespace Lacjam.Framework.Handlers
{
    public class EventHandlerError
    {
        protected EventHandlerError()
        {

        }

        public EventHandlerError(string eventType, long seq, Exception exception)
        {
            Id = Uuid.NewGuid();
            CreatedUtcDate = ClockProvider.Current.GetUtcDateTime();
            EventType = eventType;
            Exception = exception.GetType().Name;
            Message = exception.Message;
            StackTrace = exception.StackTrace;
            Seq = seq;
            Count = 1;
        }

        public virtual Guid Id { get; protected set; }
        public virtual DateTime CreatedUtcDate { get; protected set; }
        public virtual string EventType { get; protected set; }
        public virtual string Exception { get; protected set; }
        public virtual string Message { get; protected set; }
        public virtual string StackTrace { get; protected set; }
        public virtual long Seq { get; protected set; }
        public virtual int Count { get; protected set; }

        public virtual void Increment()
        {
            Count++;
        }

        protected bool Equals(EventHandlerError other)
        {
            return string.Equals(Message, other.Message) && string.Equals(StackTrace, other.StackTrace) && Seq == other.Seq;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EventHandlerError)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Message != null ? Message.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StackTrace != null ? StackTrace.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Seq.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(EventHandlerError left, EventHandlerError right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EventHandlerError left, EventHandlerError right)
        {
            return !Equals(left, right);
        }
    }
}