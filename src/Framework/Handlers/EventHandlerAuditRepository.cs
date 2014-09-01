using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Lacjam.Framework.FP;

namespace Lacjam.Framework.Handlers
{
    public class EventHandlerAuditRepository : IEventHandlerAuditRepository
    {
        private readonly ISessionFactory _sessionFactory;

        public EventHandlerAuditRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Add(EventHandlerAudit audit)
        {
            _sessionFactory.GetCurrentSession().Save(audit);
        }

        public void Save(EventHandlerAudit audit)
        {
            _sessionFactory.GetCurrentSession().SaveOrUpdate(audit);
        }

        public IMaybe<EventHandlerAudit> Get(Guid id)
        {
            return
                _sessionFactory.GetCurrentSession()
                    .Query<EventHandlerAudit>()
                    .FirstOrDefault(a => a.Id == id)
                    .ToMaybe();
        }

        public IMaybe<EventHandlerAudit> GetBySeq(long seq)
        {
            return
              _sessionFactory.GetCurrentSession()
                  .Query<EventHandlerAudit>()
                  .FirstOrDefault(a => a.Seq == seq)
                  .ToMaybe();
        }

        public long GetSkipped(long seq)
        {
            return
                _sessionFactory.GetCurrentSession()
                    .Query<EventHandlerAudit>()
                    .Count(a => a.Seq < seq &&  a.Result == EventHandlerAuditResult.Skipped);
        }

        public long GetProcessed()
        {
            return
              _sessionFactory.GetCurrentSession()
                  .Query<EventHandlerAudit>()
                  .Count(a => a.Result == EventHandlerAuditResult.Success);
        }


        public long GetLast()
        {
            return
                _sessionFactory.GetCurrentSession()
                    .Query<EventHandlerAudit>()
                    .OrderByDescending(x => x.Seq)
                    .Select(x=>x.Seq)
                    .First();
        }

        public long GetTotal()
        {
            return
               _sessionFactory.GetCurrentSession()
                   .Query<EventHandlerAudit>()
                   .Count();
        }
    }
}