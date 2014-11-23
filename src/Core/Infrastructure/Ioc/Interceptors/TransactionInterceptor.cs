using System.Data;
using System.Diagnostics;
using Castle.DynamicProxy;
using NHibernate;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Lacjam.Core.Infrastructure.Ioc.Interceptors
{
    public class TransactionInterceptor : IInterceptor
    {
        private readonly ISessionFactory _sessionFactory;

        public TransactionInterceptor(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        [DebuggerStepThrough]
        public void Intercept(IInvocation invocation)
        {
            using (var transaction = _sessionFactory.GetCurrentSession().BeginTransaction(IsolationLevel.ReadCommitted))
            {
                invocation.Proceed();
                transaction.Commit();
            }
        }
    }
}