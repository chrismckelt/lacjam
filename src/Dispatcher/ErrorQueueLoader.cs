using System;
using System.Linq;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Storage;

namespace Lacjam.Dispatcher
{
    public class ErrorQueueLoader : IErrorQueueLoader
    {

        public ErrorQueueLoader(ITransactor transactor, IEventStore eventStore, IEventErrorStoreRepository eventErrorStoreRepository, IHandlerSequenceRespository handlerSequenceRepRespository)
        {
            _transactor = transactor;
            _eventStore = eventStore;
            _eventErrorStoreRepository = eventErrorStoreRepository;
            _handlerSequenceRepRespository = handlerSequenceRepRespository;
        }

        public void SeedFromIncluding(long seq, Guid objectId)
        {
            var @events = _eventStore.GetErrorEventsMatchingAggregateIdFromSeq(seq, objectId);

            if (!@events.Any())
                return;

            var lastSeq = Int64.MinValue;

            foreach (var @event in @events)
            {
                _eventErrorStoreRepository.Transfer(@event);
                lastSeq = @event.Seq;
            }

            if(lastSeq != Int64.MinValue)
                _handlerSequenceRepRespository.Save(lastSeq, "Dispatcher");
        }

        public void PushBack(long seq)
        {
            _transactor.EnlistOrCreateTransactionForLambda(() =>
            {
                var updated = _eventErrorStoreRepository.PushBack(seq);
                if(updated)
                    _handlerSequenceRepRespository.Save(seq, "Dispatcher");
            });
        }

        public void ReScheduleEventStore(long[] seq)
        {
            foreach (var s in seq)
            {
                var @events = _eventStore.GetEventsMatchingAggregateIdFromSeq(s).ToList();
                foreach (var @event in @events)
                {
                    _eventErrorStoreRepository.Transfer(@event);
                }
            }
        }

        public void Delete(long seq)
        {
            _eventErrorStoreRepository.Delete(seq);
        }

        private readonly ITransactor _transactor;
        private readonly IEventStore _eventStore;
        private readonly IEventErrorStoreRepository _eventErrorStoreRepository;
        private readonly IHandlerSequenceRespository _handlerSequenceRepRespository;
    }
}