using Lacjam.Worker.Batches;
using NServiceBus;
using System;

namespace Lacjam.Worker.Jobs
{
    public abstract class JobBase : WorkerBase, IMessage
    {
        public Batch Batch { get; set; }

        public string Payload { get; set; }

        public override string ToString()
        {
            return string.Format("Batch: {0}, Payload: {1}, IsComplete: {2}", Batch, Payload, IsComplete);
        }

        public bool IsComplete { get; set; }

    }
}