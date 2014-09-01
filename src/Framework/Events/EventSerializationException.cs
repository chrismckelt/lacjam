using System;

namespace Lacjam.Framework.Events
{
    public class EventSerializationException : Exception
    {
        public EventSerializationException()
        {
        }

        public EventSerializationException(string message) : base(message)
        {
        }

        public EventSerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}