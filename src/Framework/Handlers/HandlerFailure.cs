namespace Lacjam.Framework.Handlers
{
    public class HandlerFailure<THandler,TKey, TEvent>
    {
        public HandlerFailure(THandler handler, TKey objectId, TEvent @event)
        {
            this.handler = handler;
            this.objectId = objectId;
            this.@event = @event;
        }

        private readonly THandler handler;
        private readonly TKey objectId;
        private readonly TEvent @event;
    }

}
