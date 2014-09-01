using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Events;

namespace Lacjam.Dispatcher
{
    public static class PublisherExtension
    {
        public static void PublishDynamic(this IDispatcherEventPublisher publisher, IEvent @event, long seq = -1)
        {
            var publish = publisher.GetType().GetMethod("Publish").MakeGenericMethod(@event.GetType());
            publish.Invoke(publisher, new object[] { @event, seq, true });
        }
    }
}