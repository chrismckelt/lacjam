using System;

namespace Lacjam.Framework.Dispatchers
{
    public interface IErrorQueueLoader
    {
        void SeedFromIncluding(long seq, Guid objectId);
        void PushBack(long seq);
        void ReScheduleEventStore(long[] seq);
        void Delete(long seq);
    }
}