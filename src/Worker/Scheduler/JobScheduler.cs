using System;
using Lacjam.Worker.Batches;
using Quartz;
using Lacjam.Core;

namespace Lacjam.Worker.Scheduler
{
    public class JobScheduler : IJobScheduler
    {
        private readonly Runtime.ILogWriter _logger;

        public JobScheduler(Runtime.ILogWriter logger, IScheduler scheduler )
        {
            _logger = logger;
            Scheduler = scheduler;
        }

        public IScheduler Scheduler { get; protected set; }
        public void ScheduleBatch(Batch batch, TimeSpan interval)
        {
            _logger.Info("ScheduleBatch..." + batch.ToString());
        }

        public void ProcessBatch(Batch batch)
        {
            _logger.Info("ProcessBatch..." + batch.ToString());
        }
    }
}
