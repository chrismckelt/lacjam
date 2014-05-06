using System;
using Lacjam.Worker.Batches;
using Quartz;

namespace Lacjam.Worker.Scheduler
{
    public interface IJobScheduler
    {
        IScheduler Scheduler { get; } 
        void ScheduleBatch(Batch batch, TimeSpan interval);
        void ProcessBatch(Batch batch);
    }
}
