using System;
using Lacjam.Framework.Events;
using Lacjam.Framework.Utilities;

namespace Lacjam.Framework.Handlers
{
    public class EventHandlerAudit
    {
        public virtual Guid Id { get; protected set; }
        public virtual long Seq { get; protected set; }
        public virtual Guid EventId { get; protected set; }
        public virtual DateTime EventProcessedUtcDate { get; protected set; }
        public virtual string Message { get; protected set; }
        public virtual DateTime? CreatedUtcDate { get; protected set; }
        public virtual EventHandlerAuditResult Result { get; protected set; }
        
        protected EventHandlerAudit()
        {
            Id = Uuid.NewGuid();
            CreatedUtcDate = DateTime.UtcNow;
        }

        public EventHandlerAudit(EventDescriptor rawEvent, DateTime processed,EventHandlerAuditResult result, string message)
        {
            Id = Uuid.NewGuid();
            CreatedUtcDate = DateTime.UtcNow;
            Seq = rawEvent.Seq;
            EventId = rawEvent.EventId;
            EventProcessedUtcDate = processed;
            Result = result;
            Message = message;
           
        }
    }
}