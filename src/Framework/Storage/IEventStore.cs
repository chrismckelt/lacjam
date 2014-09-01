using System;
using System.Collections.Generic;
using Lacjam.Framework.Events;
using Lacjam.Framework.FP;

namespace Lacjam.Framework.Storage
{
    public interface IEventStore
    {
        void SaveStream(Guid streamId, IEnumerable<IEvent> events, int expectedVersion);
        IEnumerable<EventDescriptor> GetErrorEventsMatchingAggregateIdFromSeq(long seq, Guid aggregateId);
        IEnumerable<EventErrorDescriptor> GetEventsMatchingAggregateIdFromSeq(long seq);
        IEnumerable<IEvent> GetBatch(int batchSize, int from);
        IEnumerable<EventData> FetchStream(Guid streamId);
        IMaybe<EventDescriptor> GetNextEvent(long seq); 
    }
}
