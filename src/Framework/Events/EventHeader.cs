using System;

namespace Lacjam.Framework.Events
{
   
    [Serializable]
    public class EventHeader
    {
        protected EventHeader()
        {
        }

        public EventHeader(Guid aggregateId,int schemaVersion, int version, DateTime createdUtcDate,string author)
        {
            AggregateId = aggregateId;
            SchemaVersion = schemaVersion;
            Version = version;
            CreatedUtcDate = createdUtcDate;
            Author = author;
        }

        public EventHeader(Guid aggregateId,  int version, DateTime createdUtcDate, string author)
            : this( aggregateId,1, version, createdUtcDate, author)
        {
        }

        public virtual Guid AggregateId { get; protected set; }
        public virtual int SchemaVersion { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual DateTime CreatedUtcDate { get; protected set; }
        public virtual string Author { get; protected set; }

    }
}