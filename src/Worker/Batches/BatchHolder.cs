using Lacjam.Worker.Jobs;
using System;
using System.Collections.Generic;

namespace Lacjam.Worker.Batches
{
    public abstract class BatchHolder : WorkerBase, IContainBatches
    {
        public IList<Batch> Batches { get; protected set; }
    }
}