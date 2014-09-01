using System;
using Lacjam.Framework.FP;

namespace Lacjam.Framework.Model
{
    public interface IRepository<TAggregate> where TAggregate : IAggregateRoot
    {
        IMaybe<TAggregate> Get(Guid identity);

        void Save(IMaybe<TAggregate> aggregrate, bool dispatchImmediately = false);
    }
}
