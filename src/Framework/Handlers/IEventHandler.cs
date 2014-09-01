using Lacjam.Framework.Events;

namespace Lacjam.Framework.Handlers
{
    public interface IEventHandler<in T> where T : IEvent
    {
        void Handle(T @event);
    }
}
