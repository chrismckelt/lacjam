using System;
using Castle.Windsor;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Events;
using Lacjam.Framework.Utilities;

namespace Lacjam.Framework.Handlers
{
    public class HandlerExecutor : IHandlerExecutor
    {
        private readonly ITransactor _transactor;

        public HandlerExecutor(ITransactor transactor, IWindsorContainer container, IEventHandlerAuditRepository auditRepository)
        {
            _transactor = transactor;
            _container = container;
            _auditRepository = auditRepository;
        }

        public void HandleSequencedEvent(SequencedEvent sequencedEvent, bool? runImmediately = true)
        {

            if (!sequencedEvent.HasEvent())
                return;

            IDispatcherEventPublisher publisher = null;
            _transactor.ApplyTransactionForLambda(() => 
                FinallyGuarded.Apply(() =>
            {
                publisher = DispatcherEventPublisher(sequencedEvent, runImmediately);
                AuditLogStatus(sequencedEvent);
            },
            () =>
            {

                if (publisher != null)
                    _container.Release(publisher);
            }));

        }

        private IDispatcherEventPublisher DispatcherEventPublisher(SequencedEvent sequencedEvent, bool? runImmediately)
        {
            var publisher = _container.Resolve<IDispatcherEventPublisher>();
            publisher.Publish(sequencedEvent.Event.EventData.Event, sequencedEvent.Event.Seq, runImmediately);
            return publisher;
        }

        private void AuditLogStatus(SequencedEvent sequencedEvent)
        {
            var audit = new EventHandlerAudit(sequencedEvent.Event, DateTime.UtcNow, EventHandlerAuditResult.Success, BuildAuditSuccessMessage(sequencedEvent));
            _auditRepository.Add(audit);
        }

        private static string BuildAuditSuccessMessage(SequencedEvent sequencedEvent)
        {
            return String.Format("Processed Event: {0}, For {1} with Identity: {2}", sequencedEvent.Event.EventId, sequencedEvent.EventType, sequencedEvent.AggregateId);
        }

        private readonly IWindsorContainer _container;
        private readonly IEventHandlerAuditRepository _auditRepository;
    }
}