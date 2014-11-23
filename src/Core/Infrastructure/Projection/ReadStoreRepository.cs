using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Lacjam.Core.Infrastructure.Database;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.FP;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Infrastructure.Projection
{
    public class ReadStoreRepository<T> : IReadStoreRepository<T> where T : class 
    {
     
        public ReadStoreRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Save(IMaybe<T> readmodel)
        {
            var session = _sessionFactory.GetCurrentSessionOrOpen();
            readmodel.Foreach(x =>
            {
                var ttt = x.GetType();

                foreach (var kkk in session.Statistics.EntityKeys)
                {
                    if (kkk.EntityName == ttt.FullName)
                    {
                        // 2 projection updates will blow up the session with a message - "a different object with the same identifier value was already associated with the session: }
                       
                        session.Evict(session.Get(kkk.EntityName, kkk.Identifier));
                        //session.Merge(x);
                    }
                }
                session.Save(x);

            });

            session.Flush();
            session.Clear();
        }

        public void Update(IMaybe<T> readmodel)  
        {
            var session = _sessionFactory.GetCurrentSessionOrOpen();
            readmodel.Foreach(x =>
            {
                var ttt = x.GetType();

                foreach (var kkk in session.Statistics.EntityKeys)
                {
                    if (kkk.EntityName == ttt.FullName)
                    {
                         // 2 projection updates will blow up the session with a message - "a different object with the same identifier value was already associated with the session: }
                        //session.Save(x);
                        //session.Flush();
                        session.Evict(session.Get(kkk.EntityName, kkk.Identifier));
                    }
                }

                session.Update(x);

            });
        }

        public void Remove(IMaybe<T> readmodel)
        {
            readmodel.Foreach(x => _sessionFactory.GetCurrentSessionOrOpen().Delete(x));
        }

        public T Reference(Guid identity)
        {
            return _sessionFactory.GetCurrentSessionOrOpen().Load<T>(identity);
        }

        public IMaybe<T> Get(Guid identity)
        {
            return _sessionFactory.GetCurrentSessionOrOpen().Get<T>(identity).ToMaybe();
        }

        public IQueryable<T> ToQueryable()
        {
            return _sessionFactory.GetCurrentSessionOrOpen().Query<T>();
        }

        public bool Exists(Guid identity)
        {
            return Get(identity).Fold(x => true, () => false);
        }

        private readonly ISessionFactory _sessionFactory;
    }
}