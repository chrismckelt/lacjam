namespace Lacjam.Framework.Handlers
{
    public interface IEventHandlerTransactor
    {
        void TransactionallyApply<TEvent, TEventHandler>(TEvent @event, TEventHandler handler);
    }
}
