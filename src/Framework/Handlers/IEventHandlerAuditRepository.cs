using System;
using Lacjam.Framework.FP;

namespace Lacjam.Framework.Handlers
{
    public interface IEventHandlerAuditRepository
    {
        void Add(EventHandlerAudit audit);
        void Save(EventHandlerAudit audit);
        IMaybe<EventHandlerAudit> Get(Guid id);
        IMaybe<EventHandlerAudit> GetBySeq(long seq);
        long GetSkipped(long seq);
        long GetProcessed();
        long GetTotal();
        long GetLast();
    }
}