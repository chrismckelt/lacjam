using System;
using System.Diagnostics;
using Castle.Core.Logging;
using NHibernate;
using NHibernate.Context;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Utilities;

namespace Lacjam.Dispatcher
{
    public class Transactor : ITransactor
    {

        public Transactor(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        private void OpenSession()
        {
            if (!CurrentSessionContext.HasBind(_sessionFactory))
            {
                var session = _sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }

        }

        private void CloseSession()
        {
            if (CurrentSessionContext.HasBind(_sessionFactory))
            {
                var session = CurrentSessionContext.Unbind(_sessionFactory);
                try
                {
                    if (session.IsOpen)
                    {
                        session.Flush();
                    }
                    session.Dispose();
                }
                catch (Exception e)
                {
                    Logger.Error("Unable to close session: " + e);
                }
            }
        }

        [DebuggerStepThrough]
        public void EnlistOrCreateTransactionForLambda(Action action)
        {
            if (!CurrentSessionContext.HasBind(_sessionFactory))
            {
                OpenSession();
            }

            var currentSession = _sessionFactory.GetCurrentSession();

            if (currentSession.IsOpen)
                action();
            else
            {
                ApplyTransactionForLambda(action);
            }

        }

        [DebuggerStepThrough]
        public void ApplyTransactionForLambda(Action action)
        {
            FinallyGuarded.Apply(() =>
            {
                OpenSession();
                var session = _sessionFactory.GetCurrentSession();
                using (var transaction = session.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    action();
                    transaction.Commit();
                }
            }, CloseSession);
        }

        [DebuggerStepThrough]
        public TResult ApplyTransactionForLambda<TResult>(Func<TResult> function) where TResult : new()
        {
            var result = new TResult();
            FinallyGuarded.Apply(() =>
            {
                OpenSession();
                var session = _sessionFactory.GetCurrentSession();

                using (var transaction = session.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    result = function();
                    transaction.Commit();
                }


            }, CloseSession);
            return result;
        }

        private readonly ISessionFactory _sessionFactory;
        public ILogger Logger { get; set; }
    }
}