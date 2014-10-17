using System.Data.Entity;
using System.Linq;


using Lacjam.Framework.FP;

namespace Lacjam.Framework.Handlers
{
    public class EventHandlerErrorRepository : IEventHandlerErrorRepository
    {
        private readonly DbContext _sessionFactory;

        public EventHandlerErrorRepository(DbContext sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void AddError(EventHandlerError error)
        {
            _sessionFactory.SaveChanges();
        }
        public void UpdateError(EventHandlerError error)
        {
            _sessionFactory.SaveChanges();
        }

        public IMaybe<EventHandlerError> GetLastError()
        {
            return (from error in _sessionFactory.Set<EventHandlerError>()
                    orderby error.CreatedUtcDate descending 
                    select error)
                    .Take(1)
                    .FirstOrDefault()
                    .ToMaybe();
        }

        public long GetTotalErrors()
        {
            return _sessionFactory
                                  .Set<EventHandlerError>()
                                  .Count();
        }
    }
}