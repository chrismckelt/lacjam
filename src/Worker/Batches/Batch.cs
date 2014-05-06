using System;
using System.Collections.Generic;
using Lacjam.Worker.Jobs;

namespace Lacjam.Worker.Batches
{
    public abstract class Batch : WorkerBase, IContainBatches
    {   
        public string Name { get; set; }

        public IList<JobBase> Jobs { get; set; }
        public TimeSpan RepeatInterval { get; set; }

        public IEnumerable<IContainBatches> Batches { get; protected set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Jobs: {1}, RepeatInterval: {2}, Batches: {3}", Name, Jobs, RepeatInterval, Batches);
        }
    }

}