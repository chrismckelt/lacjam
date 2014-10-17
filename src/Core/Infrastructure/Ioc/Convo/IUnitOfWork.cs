using System;
using System.Data;
using System.Data.Entity;


namespace Lacjam.Core.Infrastructure.Ioc.Convo
{
    public interface IUnitOfWork
    {
       
        DbContext Session { get; set; }
        IUnitOfWork WithIsolationLevel(IsolationLevel level);
        T Run<T>(Func<T> guardedBlock);

        T Run<T>(Func<T> guardedBlock, Action<Exception> handleException, Action finallyBlock,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        void Start();
        void Resume();
        void Abort();
        void Pause();
        void End();
       
    }
}