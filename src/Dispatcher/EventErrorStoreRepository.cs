using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Events;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.Logging;

namespace Lacjam.Dispatcher
{
    public class EventErrorStoreRepository : IEventErrorStoreRepository
    {
        
        public EventErrorStoreRepository(ISessionFactory sessionFactory, ILogWriter logger)
        {
            _sessionFactory = sessionFactory;
            _logger = logger;
        }

        public void Transfer(EventDescriptor @event)
        {
            var currentSession = _sessionFactory.GetCurrentSession();
            currentSession.Save(@event.ToErrorEventDescriptor());
            currentSession.Delete(@event);
        }

        public void Transfer(EventErrorDescriptor @event)
        {
            var currentSession = _sessionFactory.GetCurrentSession();
            currentSession.Save(@event.ToEventDescriptor());
            currentSession.Delete(@event);
        }

        public void Delete(long seq)
        {
            var currentSession = _sessionFactory.GetCurrentSession();
            var @event = currentSession.Query<EventErrorDescriptor>().FirstOrDefault(x => x.Seq == seq);
            _logger.Warn((int)EventIds.DeletingEvent, "Deleting event " + @event.Serialize());
            currentSession.Delete(@event);
        }

        public bool PushBack(long seq)
        {
            var currentSession = _sessionFactory.GetCurrentSession();
            var @event = currentSession.Query<EventDescriptor>().FirstOrDefault(x => x.Seq == seq);

            if (@event == null) return false;

            _logger.Info((int)EventIds.DeletingEvent, "PushBack event " + @event.Serialize());
            currentSession.Save(@event.ToErrorEventDescriptor());
            currentSession.Delete(@event);

            return true;
        }

        public EventErrorDescriptor Get(long seq)
        {
            return _sessionFactory.GetCurrentSession().Query<EventErrorDescriptor>().FirstOrDefault(x => x.Seq == seq);
        }

        private readonly ISessionFactory _sessionFactory;
        private readonly ILogWriter _logger;
    }
}