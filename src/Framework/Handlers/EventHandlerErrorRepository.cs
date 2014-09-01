using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Lacjam.Framework.FP;

namespace Lacjam.Framework.Handlers
{
    public class EventHandlerErrorRepository : IEventHandlerErrorRepository
    {
        private readonly ISessionFactory _sessionFactory;

        public EventHandlerErrorRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void AddError(EventHandlerError error)
        {
            _sessionFactory.GetCurrentSession().Save(error);
        }
        public void UpdateError(EventHandlerError error)
        {
            _sessionFactory.GetCurrentSession().Update(error);
        }

        public IMaybe<EventHandlerError> GetLastError()
        {
            return (from error in _sessionFactory.GetCurrentSession().Query<EventHandlerError>()
                    orderby error.CreatedUtcDate descending 
                    select error)
                    .Take(1)
                    .FirstOrDefault()
                    .ToMaybe();
        }

        public long GetTotalErrors()
        {
            return _sessionFactory.GetCurrentSession()
                                  .Query<EventHandlerError>()
                                  .Count();
        }
    }
}