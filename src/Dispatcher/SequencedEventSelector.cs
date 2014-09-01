using NHibernate.Linq;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Events;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Storage;

namespace Lacjam.Dispatcher
{
    public class SequencedEventSelector : ISequenceEventSelector
    {
        public SequencedEventSelector(ITransactor transactor,
                                      IEventStore eventStore,
                                      IHandlerSequenceRespository handlerSequenceRespository)
        {
            _transactor = transactor;
            _eventStore = eventStore;
            _handlerSequenceRespository = handlerSequenceRespository;
        }

        public IMaybe<SequencedEvent> GetSequencedEvent()
        {

            IMaybe<SequencedEvent> result = new None<SequencedEvent>();

            _transactor.ApplyTransactionForLambda(() =>
            {
                var @event = from seq in _handlerSequenceRespository.GetEventSequence("Dispatcher") 
                             from evt in _eventStore.GetNextEvent(seq)
                             select evt;

                result = from evt in @event
                         let t = evt.As<EventDescriptor>()
                         select new SequencedEvent(t);
            });

            return result;
        }

        private readonly ITransactor _transactor;
        private readonly IEventStore _eventStore;
        private readonly IHandlerSequenceRespository _handlerSequenceRespository;
    }
}