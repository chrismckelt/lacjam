using Lacjam.Framework.Events;

namespace Lacjam.Framework.Dispatchers
{
    public interface IDispatcherEventPublisher
    {
        void Publish<T>(T @event, long seq = -1, bool? runImmediately = true) where T : IEvent;
    }
}