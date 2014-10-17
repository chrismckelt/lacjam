using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using Castle.DynamicProxy;

using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Lacjam.Core.Infrastructure.Ioc.Interceptors
{
    public class TransactionInterceptor : IInterceptor
    {
        private readonly DbContext _sessionFactory;

        public TransactionInterceptor(DbContext sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        [DebuggerStepThrough]
        public void Intercept(IInvocation invocation)
        {
            using (var transaction = _sessionFactory.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                invocation.Proceed();
                transaction.Commit();
            }
        }
    }
}