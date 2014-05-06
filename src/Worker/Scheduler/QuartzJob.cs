using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lacjam.Core;
using Lacjam.Worker.Batches;
using NServiceBus;
using Quartz;

namespace Lacjam.Worker.Scheduler
{

    /// <summary>
    /// Used internally to kick off a scheduled job - dont call directly
    /// </summary>
    public class QuartzJob : IJob
    {
        private readonly Runtime.ILogWriter _logger;
        private readonly IJobScheduler _jobScheduler;

        public QuartzJob(Runtime.ILogWriter logger, IBus bus, IJobScheduler jobScheduler)
        {
            _logger = logger;
            _jobScheduler = jobScheduler;
        }

        public void Execute(IJobExecutionContext context)
        {
            _logger.Debug("Scheduling.ProcessBatch IJob Execute - " + DateTime.Now.ToShortTimeString());
            _logger.Debug("context.JobDetail.Key.Group" + context.JobDetail.Key.Group);
            _logger.Debug("context.JobDetail.Key.Name" + context.JobDetail.Key.Name);
            string batchName = context.JobDetail.Key.Name;
            _logger.Debug("QuartzJob::batchName:" +  batchName);
            try
            {
               



            }
            catch (Exception ex)
            {
                _logger.Error("QuartzJob::",ex);
                throw;
            }
          

        }

        public static IEnumerable<IContainBatches> GetBatches()
        {
            var type = typeof(IContainBatches);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && p.IsAssignableFrom(typeof(IContainBatches)))
                ;
            return types.Select(Activator.CreateInstance).Cast<IContainBatches>();
        }
    }
}
