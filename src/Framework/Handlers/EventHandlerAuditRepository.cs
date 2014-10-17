using System;
using System.Data.Entity;
using System.Linq;


using Lacjam.Framework.FP;

namespace Lacjam.Framework.Handlers
{
    public class EventHandlerAuditRepository : IEventHandlerAuditRepository
    {
        private readonly DbContext _sessionFactory;

        public EventHandlerAuditRepository(DbContext sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Add(EventHandlerAudit audit)
        {
          //  _sessionFactory.Save(audit);
        }

        public void Save(EventHandlerAudit audit)
        {
          //  _sessionFactory.SaveOrUpdate(audit);
        }

        public IMaybe<EventHandlerAudit> Get(Guid id)
        {
            return null;
            //return
            //    _sessionFactory.GetCurrentSession()
            //        .Set<EventHandlerAudit>()
            //        .FirstOrDefault(a => a.Id == id)
            //        .ToMaybe();
        }

        public IMaybe<EventHandlerAudit> GetBySeq(long seq)
        {
            return null;
            //return
            //  _sessionFactory.GetCurrentSession()
            //      .Set<EventHandlerAudit>()
            //      .FirstOrDefault(a => a.Seq == seq)
            //      .ToMaybe();
        }

        public long GetSkipped(long seq)
        {
            return 1;
        //    return
        //        _sessionFactory.GetCurrentSession()
        //            .Set<EventHandlerAudit>()
        //            .Count(a => a.Seq < seq &&  a.Result == EventHandlerAuditResult.Skipped);
        }

        public long GetProcessed()
        {
             return 1;
            //return
            //  _sessionFactory.GetCurrentSession()
            //      .Set<EventHandlerAudit>()
            //      .Count(a => a.Result == EventHandlerAuditResult.Success);
        }


        public long GetLast()
        {
            return 1;
            //return
            //    _sessionFactory.GetCurrentSession()
            //        .Set<EventHandlerAudit>()
            //        .OrderByDescending(x => x.Seq)
            //        .Select(x=>x.Seq)
            //        .First();
        }

        public long GetTotal()
        {
            return 12;
            //return
            //   _sessionFactory.GetCurrentSession()
            //       .Set<EventHandlerAudit>()
            //       .Count();
        }
    }
}