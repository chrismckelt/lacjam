using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Lacjam.Framework.Events;
using Lacjam.Framework.FP;
using Lacjam.Framework.Storage;
using Lacjam.Core.Infrastructure.Database;

namespace Lacjam.Core.Infrastructure.Storage
{

    public class EventStore : IEventStore
    {
       

        public EventStore(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public IEnumerable<EventDescriptor> GetErrorEventsMatchingAggregateIdFromSeq(long seq, Guid aggregateId)
        {
            
            return _sessionFactory.GetCurrentSessionOrOpen().Query<EventDescriptor>()
                           .Where(x => x.Seq >= seq)
                           .Where(x => x.Header.AggregateId == aggregateId)
                           .ToList();
        }

        public IEnumerable<EventErrorDescriptor> GetEventsMatchingAggregateIdFromSeq(long seq)
        {
            
            var match =
                _sessionFactory.GetCurrentSessionOrOpen().Query<EventErrorDescriptor>().FirstOrDefault(a => a.Seq == seq);

            if (match == null)
                yield break;

            if (match.Header.AggregateId == Guid.Empty)
                yield return match;
            else
            {
                foreach (var item in _sessionFactory.GetCurrentSessionOrOpen().Query<EventErrorDescriptor>()
                                                    .Where(x => x.Header.AggregateId == match.Header.AggregateId)
                                                    .ToList())
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<IEvent> GetBatch(int batchSize, int start)
        {

            var result = (from @event in _sessionFactory.GetCurrentSessionOrOpen().Query<EventDescriptor>()
                          where @event.Seq >= start
                          select @event.EventData)
                          .Take(batchSize);

            return from item in result select item.Event;
        }

        public IMaybe<EventDescriptor> GetNextEvent(long seq)
        {
           
            var result = (from @event in _sessionFactory.GetCurrentSessionOrOpen().Query<EventDescriptor>()
                          where @event.Seq > seq
                          select @event)
                          .FirstOrDefault();

            return result.ToMaybe();
        }

        public void SaveStream(Guid streamId, IEnumerable<IEvent> events, int expectedVersion)
        {
            
            var version = ++expectedVersion;

            var stream = from @event in events
                         let header = new EventHeader(@event.AggregateIdentity, version, DateTime.UtcNow, "Todo - Put Author here")
                         select new EventDescriptor(@event, header);

            var session = _sessionFactory.GetCurrentSessionOrOpen();
            foreach (var descriptor in stream)
            {
                // as we use the same GUIDs for IDs across multiple entities - NH will get confused & load the wrong query plan if we dont evict by ID from the current session
                session.Evict(descriptor);
                session.SaveOrUpdate(descriptor);
            }

            CheckAggregateVersion(streamId, version);
        }

        private void CheckAggregateVersion(Guid streamId, int expectedVersion)
        {
            
            var currentVersion = (
                                   from @event in _sessionFactory.GetCurrentSessionOrOpen().Query<EventDescriptor>()
                                   where @event.Header.AggregateId == streamId
                                   orderby @event.Seq descending
                                   select @event.Header.Version
                                  ).FirstOrDefault();


            if (currentVersion == 0)
                return;

            if (currentVersion != expectedVersion)
                throw new Exception();
        }

        public IEnumerable<EventData> FetchStream(Guid streamId)
        {
           
            return from @event in _sessionFactory.GetCurrentSessionOrOpen().Query<EventDescriptor>()
                   where @event.Header.AggregateId == streamId
                   orderby @event.Seq ascending 
                   select @event.EventData;

        }
        private readonly ISessionFactory _sessionFactory;
    }
}
