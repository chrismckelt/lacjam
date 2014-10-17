using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Castle.MicroKernel.Lifestyle;

using Lacjam.Core.Infrastructure.Database;
using Lacjam.Framework.Logging;
using uNhAddIns.Adapters;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;

namespace Lacjam.Core.Infrastructure.Ioc.Convo
{
    [PersistenceConversational]
    public class UnitOfWork : IUnitOfWork
    {
       
        public bool UseSupportForOutsidePersistentCall { get; set; }

        public DbContext Session
        {
            get
            {
                if (_session == null) Start();
                if (_session.Database.Connection ==null)
                {
                    _logWriter.Debug("UnitOfWork - CheckSession");
                    CheckSession();
                }
               
                return _session;
            }
            set { _session = value; }
        }

        private DbContext _session;
        private readonly ILogWriter _logWriter;
        private IsolationLevel _level = IsolationLevel.ReadUncommitted;
        private bool _hasEnded = false;

        static UnitOfWork()
        {
        }

        public UnitOfWork(ILogWriter logWriter, DbContext session)
            //: base(new uNhAddIns.SessionEasier.SessionFactoryProvider(provider), wrapper)
        {
            this.Session = session;
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
            //if (!_wrapper.IsWrapped(_session)) Bind();

            //if (_session != null && _session.IsOpen)
            //{
            //    if (!_session.IsConnected)
            //        _session.Reconnect();
            //}

            //_session.FlushMode = FlushMode.Auto;

        }

        private void CheckTransaction()
        {
            //if (_session.Transaction == null || (!_session.Transaction.IsActive) && _session.IsOpen)
            //{
            //    _session.BeginTransaction(_level);
            //}
        }

        public void Start()
        {
            if (_session == null)
            {
                _logWriter.Debug("UnitOfWork - new session");
                this.UseSupportForOutsidePersistentCall = true;
               // _session = SessionFactory.OpenSession();
                Bind();
            }
            DoStart();
        }

        public void Resume()
        {
           DoResume();
            
        }

        public void Pause()
        {
            DoResume();
            Commit(_session);
        }

        public void Abort()
        {
            DoAbort();
        }

        public void End()
        {
             DoEnd();
            _hasEnded = true;
        }

        private void Commit(DbContext session)
        {
            //if (session.Transaction == null || !session.Transaction.IsActive)
            //    return;

            //try
            //{
            //    session.Transaction.Commit();
            //}
            //catch (Exception)
            //{
            //    session.Transaction.Rollback();
            //    throw;
            //}

        }

        private void FlushAndCommit(DbContext session)
        {
            //if (session.Transaction == null || !session.Transaction.IsActive)
            //    return;
            //try
            //{
            //    session.Flush();
            //    session.Transaction.Commit();
            //}
            //catch (Exception)
            //{
            //    session.Transaction.Rollback();
            //    throw;
            //}
        }


        protected  void DoStart()
        {
            WindsorAccessor.Instance.Container.RequireScope();
            this.DoResume();
        }

        protected  void DoPause()
        {
            Commit(_session);
        }

        protected  void DoFlushAndPause()
        {
            //if (_session != null && _session.IsOpen)
            //{
            //    FlushAndCommit(Session);
            //}
        }


        protected  void DoResume()
        {
            CheckSession();
            
            CheckTransaction();
        }

        protected  void DoEnd()
        {
            WindsorAccessor.Instance.Container.RequireScope();
            //FlushAndCommit(_session);

            //if (_session != null && _session.IsOpen)
            //{
            //    _session.Close();
            //}
        }

        protected  void DoAbort()
        {

            //if (_session != null && _session.Transaction!=null)
            //    _session.Transaction.Rollback();

            //if (_session != null && _session.IsOpen)
            //    _session.Close();
        }


        protected virtual DbContext Wrap(DbContext session)
        {
            //if (this.UseSupportForOutsidePersistentCall)
            //    return this._wrapper.WrapWithAutoTransaction(session, (SessionCloseDelegate)null, new SessionDisposeDelegate(this.Unbind));
            //else
            //    return this._wrapper.Wrap(session, (SessionCloseDelegate)null, new SessionDisposeDelegate(this.Unbind));
            return null;
        }


        public void Bind()
        {
            //DbContext sessionFactory = _session.SessionFactory;
            
           // this.DoBind(_session, sessionFactory);
        }

        private void CleanupAnyOrphanedSession(DbContext factory)
        {
            // this.DoUnbind(factory);
            // if (_session == null || !_session.IsOpen)
            //    return;
            //_logWriter.Warn(EventIds.NHibernateSession,"Already session bound on call to Bind(); make sure you clean up your sessions!");
            //try
            //{
            //    if (_session.Transaction != null)
            //    {
            //        if (_session.Transaction.IsActive)
            //        {
            //            try
            //            {
            //                _session.Transaction.Rollback();
            //            }
            //            catch (Exception ex)
            //            {
            //                _logWriter.Warn(EventIds.NHibernateSession,"Unable to rollback transaction for orphaned session", ex);
            //            }
            //        }
            //    }
            //    _session.Close();
            //}
            //catch (Exception ex)
            //{
            //    _logWriter.Warn(EventIds.NHibernateSession,"Unable to close orphaned session", ex);
            //}
        }

        private void DoUnbind(DbContext factory)
        {
          //  HybridWebSessionContext.Unbind(factory);
          
        }

        public void Unbind(DbContext session)
        {
            //if (HybridWebSessionContext.HasBind(session.SessionFactory))
            //{
            //    this.DoUnbind(session.SessionFactory);
            //}
            //else
            //{
            //    CleanupAnyOrphanedSession(session.SessionFactory);
            //}
        }

        private void DoBind(DbContext session, DbContext factory)
        {
            //if (!HybridWebSessionContext.HasBind(factory))
            //{
            //    HybridWebSessionContext.Bind(session);
            //    this.Wrap(session);
            //}
           
        }

        protected  void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_hasEnded) End();
                if (_session != null) _session.Dispose();
            }
            else
            {
                
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
