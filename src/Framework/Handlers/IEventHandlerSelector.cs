using System.Collections.Generic;
using Lacjam.Framework.Events;

namespace Lacjam.Framework.Handlers
{
    public interface IEventHandlerSelector
    {
        IEnumerable<IEventHandler<TEvent>> GetHandlersForEvent<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
