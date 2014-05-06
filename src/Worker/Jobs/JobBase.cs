using System;
using NServiceBus;

namespace Lacjam.Worker.Jobs
{
    public abstract class JobBase : WorkerBase,  IMessage, IEquatable<JobBase>
    {
        public Guid BatchId { get; set; }
        public string Payload { get; set; }
        public bool IsComplete { get; set; }

        public bool Equals(JobBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return BatchId.Equals(other.BatchId);
        }

        bool IEquatable<JobBase>.Equals(JobBase other)
        {
            return Equals(other);
        }

        public override string ToString()
        {
            return string.Format("BatchId: {0}, Payload: {1}, IsComplete: {2}", BatchId, Payload, IsComplete);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((JobBase) obj);
        }

        public override int GetHashCode()
        {
            return BatchId.GetHashCode();
        }

        public static bool operator ==(JobBase left, JobBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(JobBase left, JobBase right)
        {
            return !Equals(left, right);
        }

    }
}
