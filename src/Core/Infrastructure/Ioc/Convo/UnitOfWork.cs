using System;
using System.Collections.Generic;
using System.Data;
using Castle.MicroKernel.Lifestyle;
using NHibernate;
using Lacjam.Core.Infrastructure.Database;
using Lacjam.Framework.Logging;
using uNhAddIns.Adapters;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;

namespace Lacjam.Core.Infrastructure.Ioc.Convo
{
    [PersistenceConversational]
    public class UnitOfWork : AbstractConversation, IUnitOfWork, ISupportOutsidePersistentCall
    {
        public ISessionWrapper _wrapper { get; private set; }
        public bool UseSupportForOutsidePersistentCall { get; set; }
        public ISessionFactory SessionFactory { get; private set; }

        public ISession Session
        {
            get
            {
                if (_session == null) Start();
                if (!_session.IsOpen || _session.IsConnected)
                {
                    _logWriter.Debug("UnitOfWork - CheckSession");
                    CheckSession();
                }
               
                return _session;
            }
            set { _session = value; }
        }

        private ISession _session;
        private readonly ILogWriter _logWriter;
        private IsolationLevel _level = IsolationLevel.ReadCommitted;
        private bool _hasEnded = false;

        static UnitOfWork()
        {
        }

        public UnitOfWork(ILogWriter logWriter, ISessionFactory sessionFactory, ISessionWrapper wrapper)
            //: base(new uNhAddIns.SessionEasier.SessionFactoryProvider(provider), wrapper)
        {
            SessionFactory = sessionFactory;
            _wrapper = wrapper;
            _logWriter = logWriter;
            
        }

        public IUnitOfWork WithIsolationLevel(IsolationLevel level)
        {
            _level = level;
            return this;
        }

        public T Run<T>(Func<T> guardedBlock)
        {
            return Run(guardedBlock, ex => Abort(), End);
        }

        public T Run<T>(Func<T> guardedBlock, Action<Exception> handleException, Action finallyBlock,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            try
            {
                Start();
                return guardedBlock();
            }
            catch (Exception ex)
            {
                handleException(ex);
                Abort();
            }
            finally
            {
                finallyBlock();
                End();
            }
            return default(T);
        }


        private void CheckSession()
        {
            if (!_wrapper.IsWrapped(_session)) Bind();

            if (_session != null && _session.IsOpen)
            {
                if (!_session.IsConnected)
                    _session.Reconnect();
            }

            _session.FlushMode = FlushMode.Auto;

        }

        private void CheckTransaction()
        {
            if (_session.Transaction == null || (!_session.Transaction.IsActive) && _session.IsOpen)
            {
                _session.BeginTransaction(_level);
            }
        }

        public override void Start()
        {
            if (_session == null)
            {
                _logWriter.Debug("UnitOfWork - new session");
                this.UseSupportForOutsidePersistentCall = true;
                _session = SessionFactory.OpenSession();
                Bind();
            }
            DoStart();
        }

        public override void Resume()
        {
           DoResume();
            
        }

        public override void Abort()
        {
            DoAbort();
        }

        public override void End()
        {
             DoEnd();
            _hasEnded = true;
        }

        private static void Commit(ISession session)
        {
            if (session.Transaction == null || !session.Transaction.IsActive)
                return;
            session.Transaction.Commit();
        }

        private static void FlushAndCommit(ISession session)
        {
            if (session.Transaction == null || !session.Transaction.IsActive)
                return;
            session.Flush();
            session.Transaction.Commit();
        }


        protected override void DoStart()
        {
            WindsorAccessor.Instance.Container.RequireScope();
            this.DoResume();
        }

        protected override void DoPause()
        {
            UnitOfWork.Commit(_session);
        }

        protected override void DoFlushAndPause()
        {
            if (_session != null && _session.IsOpen)
            {
                UnitOfWork.FlushAndCommit(Session);
            }
        }


        protected override void DoResume()
        {
            CheckSession();
            
            CheckTransaction();
        }

        protected override void DoEnd()
        {
            WindsorAccessor.Instance.Container.RequireScope();
            FlushAndCommit(_session);

            if (_session != null && _session.IsOpen)
            {
                _session.Close();
            }
        }

        protected override void DoAbort()
        {
            if (_session != null && _session.IsOpen)
                _session.Close();
        }


        protected virtual ISession Wrap(ISession session)
        {
            if (this.UseSupportForOutsidePersistentCall)
                return this._wrapper.WrapWithAutoTransaction(session, (SessionCloseDelegate)null, new SessionDisposeDelegate(this.Unbind));
            else
                return this._wrapper.Wrap(session, (SessionCloseDelegate)null, new SessionDisposeDelegate(this.Unbind));
        }


        public void Bind()
        {
            ISessionFactory sessionFactory = _session.SessionFactory;
            
            this.DoBind(_session, sessionFactory);
        }

        private void CleanupAnyOrphanedSession(ISessionFactory factory)
        {
             this.DoUnbind(factory);
             if (_session == null || !_session.IsOpen)
                return;
            _logWriter.Warn(EventIds.NHibernateSession,"Already session bound on call to Bind(); make sure you clean up your sessions!");
            try
            {
                if (_session.Transaction != null)
                {
                    if (_session.Transaction.IsActive)
                    {
                        try
                        {
                            _session.Transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            _logWriter.Warn(EventIds.NHibernateSession,"Unable to rollback transaction for orphaned session", ex);
                        }
                    }
                }
                _session.Close();
            }
            catch (Exception ex)
            {
                _logWriter.Warn(EventIds.NHibernateSession,"Unable to close orphaned session", ex);
            }
        }

        private void DoUnbind(ISessionFactory factory)
        {
            HybridWebSessionContext.Unbind(factory);
          
        }

        public void Unbind(ISession session)
        {
            if (HybridWebSessionContext.HasBind(session.SessionFactory))
            {
                this.DoUnbind(session.SessionFactory);
            }
            else
            {
                CleanupAnyOrphanedSession(session.SessionFactory);
            }
        }

        private void DoBind(ISession session, ISessionFactory factory)
        {
            if (!HybridWebSessionContext.HasBind(factory))
            {
                HybridWebSessionContext.Bind(session);
                this.Wrap(session);
            }
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_hasEnded) End();
                if (_session != null) _session.Dispose();
            }
            else
            {
                Dispose();
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
