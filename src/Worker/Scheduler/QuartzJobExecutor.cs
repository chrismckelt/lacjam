using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Castle.Core.Internal;
using Castle.Core.Logging;
using Lacjam.Core;
using Lacjam.Worker.Batches;
using NServiceBus;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lacjam.Worker.Scheduler
{
    /// <summary>
    /// Used internally to kick off a scheduled job - dont call directly
    /// </summary>
    public class QuartzJobExecutor : IJob
    {
        private readonly Runtime.ILogWriter _logger;
        private readonly IJobScheduler _jobScheduler;

        public QuartzJobExecutor()
        {
            _logger = Runtime.Ioc.Resolve<Runtime.ILogWriter>();
            _jobScheduler = Runtime.Ioc.Resolve<IJobScheduler>();
        }

        public QuartzJobExecutor(Runtime.ILogWriter logger, IJobScheduler jobScheduler)
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
            _logger.Debug("QuartzJob::batchName:" + batchName);
            try
            {
                foreach (var holder in GetBatches())
                {
                    foreach (var batch in holder.Batches)
                    {
                        _logger.Info("QuartzJobExecutor::Execute: " + batchName);
                        _logger.Debug(batch.ToString());
                        _jobScheduler.ProcessBatch(batch);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("QuartzJobExecutor:: Execute:Exception: ", ex);
                throw;
            }
        }

        public static IEnumerable<IContainBatches> GetBatches()
        {
            var type = typeof(IContainBatches);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && type.IsAssignableFrom(typeof(IContainBatches)) && p.IsClass && !p.IsAbstract)
                .ToList();

            foreach (var ty in types)
            {
                yield return GetInstance<IContainBatches>(ty);
            }
            
        }

        public static T GetInstance<T>(Type type)
        {
            return (T)Activator.CreateInstance(type);
        }
    }
}