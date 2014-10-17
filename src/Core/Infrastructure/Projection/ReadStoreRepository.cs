using System;
using System.Data.Entity;
using System.Linq;


using Lacjam.Core.Infrastructure.Database;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.FP;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Infrastructure.Projection
{
    public class ReadStoreRepository<T> : IReadStoreRepository<T> where T : class 
    {
     
        public ReadStoreRepository(DbContext sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Save(IMaybe<T> readmodel)
        {
            //var session = _sessionFactory;
            //readmodel.Foreach(x =>
            //{
            //    var ttt = x.GetType();

            //    foreach (var kkk in session.Statistics.EntityKeys)
            //    {
            //        if (kkk.EntityName == ttt.FullName)
            //        {
            //            // 2 projection updates will blow up the session with a message - "a different object with the same identifier value was already associated with the session: }
                       
            //            session.Evict(session.Get(kkk.EntityName, kkk.Identifier));
            //            //session.Merge(x);
            //        }
            //    }
            //    session.Save(x);

            //});

            //session.Flush();
            //session.Clear();
        }

        public void Update(IMaybe<T> readmodel)  
        {
            //var session = _sessionFactory;
            //readmodel.Foreach(x =>
            //{
            //    var ttt = x.GetType();

            //    foreach (var kkk in session.Statistics.EntityKeys)
            //    {
            //        if (kkk.EntityName == ttt.FullName)
            //        {
            //             // 2 projection updates will blow up the session with a message - "a different object with the same identifier value was already associated with the session: }
            //            //session.Save(x);
            //            //session.Flush();
            //            session.Evict(session.Get(kkk.EntityName, kkk.Identifier));
            //        }
            //    }

            //    session.Update(x);

            //});
        }

        public void Remove(IMaybe<T> readmodel)
        {
            return;
            //readmodel.Foreach(x => _sessionFactory.Delete(x));
        }

        public T Reference(Guid identity)
        {
            return null;
            //return _sessionFactory.Load<T>(identity);
        }

        public IMaybe<T> Get(Guid identity)
        {
            return null; //_sessionFactory.<T>(identity).ToMaybe();
        }

        public IQueryable<T> ToQueryable()
        {
            return _sessionFactory.Set<T>();
        }

        public bool Exists(Guid identity)
        {
            return Get(identity).Fold(x => true, () => false);
        }

        private readonly DbContext _sessionFactory;
    }
}