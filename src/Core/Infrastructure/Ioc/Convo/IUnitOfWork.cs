using System;
using System.Data;
using NHibernate;

namespace Lacjam.Core.Infrastructure.Ioc.Convo
{
    public interface IUnitOfWork
    {
        ISessionFactory SessionFactory { get; }
        ISession Session { get; set; }
        IUnitOfWork WithIsolationLevel(IsolationLevel level);
        T Run<T>(Func<T> guardedBlock);

        T Run<T>(Func<T> guardedBlock, Action<Exception> handleException, Action finallyBlock,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        void Start();
        void Resume();
        void Abort();
        void End();
       
    }
}