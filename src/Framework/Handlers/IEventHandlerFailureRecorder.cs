namespace Lacjam.Framework.Handlers
{
    public interface IEventHandlerFailureRecorder
    {
        void RecordFailureFor<TKey, TEvent, THandler>(HandlerFailure<THandler, TKey, TEvent> failure);
    }
}
