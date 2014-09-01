using System;
using Lacjam.Framework.Events;

namespace Lacjam.Framework.Dispatchers
{
    public interface IErrorReporter
    {
        void ReportErrorForScope(SequencedEvent @event, Action scope, Action<Guid?> aggregateId);
        void ReportErrorForScope(Action scope);
    }
}