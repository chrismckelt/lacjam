using System;

namespace Lacjam.Framework.Events
{
    public interface IEvent : IMessage
    {
        Guid AggregateIdentity { get; }
    }
}

