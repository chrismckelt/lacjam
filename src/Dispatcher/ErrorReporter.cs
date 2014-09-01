using System;
using System.Reflection;
using System.Threading;
using Castle.Core.Logging;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Events;
using Lacjam.Framework.Exceptions;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.Handlers;

namespace Lacjam.Dispatcher
{
    public class ErrorReporter : IErrorReporter
    {
        public ErrorReporter(ITransactor transactor, IErrorQueueLoader errorQueueLoader, IEventHandlerAuditRepository auditRepository, IEventHandlerErrorRepository errorRepository)
        {
            _transactor = transactor;
            _errorQueueLoader = errorQueueLoader;
            _auditRepository = auditRepository;
            _errorRepository = errorRepository;
        }

        public void ReportErrorForScope(SequencedEvent @event, Action scope, Action<Guid?> aggregateId)
        {
            try
            {
                scope();
            }
            catch (EndOfEventStreamException)
            {
                Thread.Sleep(200);
                Logger.Info("End of stream....");
            }
            catch (TargetInvocationException e)
            {
                var unrolledException = e.UnrollDynamicallyInvokedException();

                try
                {
                    if (SqlTransientExceptionDetector.IsTransient(unrolledException.SourceException))
                    {
                        AwaitInfrastructure();
                        return;
                    }
                    _transactor.ApplyTransactionForLambda(() => HandleError(@event, unrolledException.SourceException, aggregateId));
                }
                catch
                {
                    Logger.Error("Unable to write exception to Database");
                }
            }
            catch (Exception e)
            {
                try
                {
                    if (SqlTransientExceptionDetector.IsTransient(e))
                    {
                        AwaitInfrastructure();
                        return;
                    }
                    _transactor.ApplyTransactionForLambda(() => HandleError(@event, e, aggregateId));
                }
                catch
                {
                    Logger.Error("Unable to write exception to Database");
                }
            }
        }

        private void AwaitInfrastructure()
        {
            Thread.Sleep(500);
            Logger.Error("Transient Sql Exception");
        }

        public void ReportErrorForScope(Action scope)
        {
            try
            {
                scope();
            }
            catch (EndOfEventStreamException)
            {
                Thread.Sleep(200); // Wait we are at the end of the stream
                Logger.Info("End of stream....");
            }
            catch (TargetInvocationException e)
            {
                var unrolledException = e.UnrollDynamicallyInvokedException();
                Logger.Error("Error Performing Action: ", unrolledException.SourceException);

            }
            catch (Exception e)
            {
                Logger.Error("Error Performing Action: ", e);
            }
        }

        private void HandleError(SequencedEvent @event, Exception e, Action<Guid?> aggregateId)
        {
            AddAuditRecord(@event, e);
            RecordException(@event, e);
            TransferOthersInSequence(@event);

            Logger.Error(String.Format("Error on event {0} {1} {2}", @event.EventType, @event.Sequence, e));

            CallbackDispatcherOnAggregateIdFailure(@event, aggregateId);
        }

        private static void CallbackDispatcherOnAggregateIdFailure(SequencedEvent @event, Action<Guid?> aggregateId)
        {
            aggregateId(@event.AggregateId);
        }

        private void TransferOthersInSequence(SequencedEvent @event)
        {
            _errorQueueLoader.SeedFromIncluding(@event.Sequence, @event.AggregateId);
        }

        private void RecordException(SequencedEvent @event, Exception e)
        {
            var error = new EventHandlerError(@event.EventType, @event.Sequence, e);
            if (LastError != null && LastError == error)
            {
                LastError.Increment();
                _errorRepository.UpdateError(LastError);
                Logger.Error(String.Format("Error on event {0} {1} (count : {2})", @event.EventType, @event.Sequence, LastError.Count));
            }
            else
            {
                _errorRepository.AddError(error);
                LastError = error;
                Logger.Error(String.Format("Error on event {0} {1} {2}", @event.EventType, @event.Sequence, e));
            }
        }

        private void AddAuditRecord(SequencedEvent @event, Exception e)
        {
            _auditRepository.Add(new EventHandlerAudit(@event.Event, DateTime.UtcNow, EventHandlerAuditResult.Failed, e.Message));
        }

        public ILogger Logger { get; set; }
        public EventHandlerError LastError { get; set; }
        private readonly ITransactor _transactor;
        private readonly IErrorQueueLoader _errorQueueLoader;
        private readonly IEventHandlerAuditRepository _auditRepository;
        private readonly IEventHandlerErrorRepository _errorRepository;
    }
}