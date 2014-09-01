using System;
using System.Linq;
using Lacjam.Framework.FP;

namespace Lacjam.Framework.Projection
{
    public interface IReadStoreRepository<T>
    {
        void Save(IMaybe<T> readmodel);
        void Update(IMaybe<T> readmodel);
        void Remove(IMaybe<T> readmodel);
        IMaybe<T> Get(Guid identity);
        IQueryable<T> ToQueryable();
        bool Exists(Guid identity);
    }
}
