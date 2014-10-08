using System;
using System.Threading;
using Castle.Core.Logging;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Events;
using Lacjam.Framework.FP;
using Lacjam.Framework.Utilities;

namespace Lacjam.Dispatcher
{
    public class DispatcherEngine : IDispatcherEngine
    {
        public DispatcherEngine(ISequenceEventSelector sequencedEventSelector, IErrorQueueLoader errorQueueLoader, IHandlerExecutor handlerExecutor, IErrorReporter errorReporter)
        {
            _sequencedEventSelector = sequencedEventSelector;
            _errorQueueLoader = errorQueueLoader;
            _handlerExecutor = handlerExecutor;
            _errorReporter = errorReporter;
        }

        public void Process(Func<bool> terminate, bool exitAtEndOfStream, bool includeImmediate)
        {
            try
            {
                EventLoop.Run(() => LoopBody(terminate, exitAtEndOfStream, includeImmediate));
            }
            catch (Exception e)
            {
                Logger.Error("Application Terminated Due To Error: " + e);
            }
        }

        private bool LoopBody(Func<bool> terminate, bool exitAtEndOfStream, bool includeImmediate)
        {
            try
            {
                if (ServiceClosing(terminate, exitAtEndOfStream))
                    return false;

                var sequencedEvent = GetSequencedEvent();

                sequencedEvent.Foreach(seq =>
                {
                    if (IsBadSequencedEvent(seq))
                        PushEventImmediatelyToErrorQueue(seq);
                    else
                        ProcessEvent(exitAtEndOfStream, seq, includeImmediate);
                });

                sequencedEvent.OnEmpty(Sleep);

                return true;
            }
            catch (Exception e)
            {
                Logger.Error("Event Loop Received an Error: " + e);
                return true;
            }
        }

        private void ProcessEvent(bool exitAtEndOfStream, SequencedEvent sequencedEvent, bool includeImmediate)
        {
            if (FinishedReplayingEvents(exitAtEndOfStream, sequencedEvent))
                Terminate();

            Process(sequencedEvent, includeImmediate);
        }

        private void PushEventImmediatelyToErrorQueue(SequencedEvent sequencedEvent)
        {
            _errorQueueLoader.PushBack(sequencedEvent.Sequence);
        }

        private IMaybe<SequencedEvent> GetSequencedEvent()
        {
            IMaybe<SequencedEvent> sequencedEvent = new None<SequencedEvent>();
            _errorReporter.ReportErrorForScope(() => sequencedEvent = _sequencedEventSelector.GetSequencedEvent());
            return sequencedEvent;
        }

        private bool IsBadSequencedEvent(SequencedEvent sequencedEvent)
        {
            if (!LastBadAggregateId.HasValue) return false;
            if (sequencedEvent == null) return false;

            return LastBadAggregateId.Value == sequencedEvent.AggregateId;
        }

        private static bool ServiceClosing(Func<bool> terminate, bool exitAtEndOfStream)
        {
            return !exitAtEndOfStream && terminate();
        }

        private static bool FinishedReplayingEvents(bool exitAtEndOfStream, SequencedEvent sequencedEvent)
        {
            return exitAtEndOfStream && sequencedEvent != null && !sequencedEvent.HasEvent();
        }

        private void Process(SequencedEvent sequencedEvent, bool includeImmediate)
        {
            if (sequencedEvent.AggregateId != LastBadAggregateId)
                _errorReporter.ReportErrorForScope(sequencedEvent, () => _handlerExecutor.HandleSequencedEvent(sequencedEvent, includeImmediate? (bool?)null: false), UpdateLastFailedAggregateId);
            else
                _errorReporter.ReportErrorForScope(() => _errorQueueLoader.PushBack(sequencedEvent.Sequence));
        }

        private void UpdateLastFailedAggregateId(Guid? aggregateId)
        {
            if (!aggregateId.HasValue)
                return;

            LastBadAggregateId = aggregateId;
        }

        private static void Terminate()
        {
            Environment.Exit(0);
        }

        private static void Sleep()
        {
            Thread.Sleep(300);
        }

        private Guid? LastBadAggregateId { get; set; }
        public ILogger Logger { get; set; }
        private readonly ISequenceEventSelector _sequencedEventSelector;
        private readonly IErrorQueueLoader _errorQueueLoader;
        private readonly IHandlerExecutor _handlerExecutor;
        private readonly IErrorReporter _errorReporter;
    }
}