using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Events;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Storage;
using System;
using System.Linq;

namespace Lacjam.Framework.Model
{

    public class Repository<TAggregate> : IRepository<TAggregate>
        where TAggregate : IAggregateRoot
    {

        public Repository(IEventStore eventStore, IDispatcherEventPublisher publisher)
        {
            _eventStore = eventStore;
            _publisher = publisher;
        }

        public IMaybe<TAggregate> Get(Guid identity)
        {
            var stream = _eventStore.FetchStream(identity);

            if (!stream.Any())
                return new None<TAggregate>();

            var item = AggregateBuilder.LoadFromEvents<TAggregate>(identity, stream.Select(a=>a.Event));

            return new Just<TAggregate>(item);
        }

        public void Save(IMaybe<TAggregate> aggregrate, bool dispatchImmediately=true)
        {
            aggregrate.Foreach(x =>
            {
                var uncommitedChanges = x.GetUncommitedChanges();
                var enumerable = uncommitedChanges as IEvent[] ?? uncommitedChanges.ToArray();
                _eventStore.SaveStream(x.GetIdentity(), enumerable, x.GetVersion());

                enumerable.Each(a => _publisher.Publish(a,-1, dispatchImmediately));
            });
        }

        private readonly IEventStore _eventStore;
        private readonly IDispatcherEventPublisher _publisher;
    }

}
