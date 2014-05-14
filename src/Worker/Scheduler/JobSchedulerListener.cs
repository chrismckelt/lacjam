using Lacjam.Core;
using NServiceBus;
using Quartz;
using System;
using System.Globalization;

namespace Lacjam.Worker.Scheduler
{
    public class JobSchedulerListener : Quartz.ISchedulerListener
    {
        private readonly Runtime.ILogWriter _logger;
        private readonly IBus _bus;

        public JobSchedulerListener(Runtime.ILogWriter logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public void JobScheduled(ITrigger trigger)
        {
            string log = string.Concat("JobScheduled: ", trigger.JobKey.Group, trigger.JobKey.Name,
                trigger.Key.Group, trigger.Key.Name);
            _logger.Debug(log);
        }

        public void JobUnscheduled(TriggerKey jd)
        {
            string log = string.Concat("JobUnScheduled", jd.ToString());
            _logger.Debug(log);
        }

        public void TriggerFinalized(ITrigger trigger)
        {
            _logger.Debug(string.Concat("TriggerFinalized", trigger.ToString()));
        }

        public void TriggerPaused(TriggerKey triggerKey)
        {
            _logger.Debug(string.Concat("TriggerPaused", triggerKey.ToString()));
        }

        public void TriggersPaused(string tg)
        {
            _logger.Debug(string.Concat("TriggersPaused", tg.ToString(CultureInfo.InvariantCulture)));
        }

        public void TriggerResumed(TriggerKey triggerKey)
        {
            _logger.Debug(string.Concat("TriggerResumed", triggerKey.ToString()));
        }

        public void TriggersResumed(string tg)
        {
            _logger.Debug(string.Concat("TriggersPaused", tg.ToString(CultureInfo.InvariantCulture)));
        }

        public void JobAdded(IJobDetail jd)
        {
            _logger.Debug(string.Concat("JobAdded", jd.ToString()));
        }

        public void JobDeleted(JobKey jobKey)
        {
            _logger.Debug(string.Concat("JobDeleted", jobKey.ToString()));
        }

        public void JobPaused(JobKey jobKey)
        {
            _logger.Debug(string.Concat("JobDeleted", jobKey.ToString()));
        }

        public void JobsPaused(string jobGroup)
        {
            _logger.Debug(string.Concat("JobsPaused", jobGroup.ToString(CultureInfo.InvariantCulture)));
        }

        public void JobResumed(JobKey jobKey)
        {
            _logger.Debug(string.Concat("JobDeleted", jobKey.ToString()));
        }

        public void JobsResumed(string jg)
        {
            _logger.Debug(string.Concat("JobDeleted", jg.ToString(CultureInfo.InvariantCulture)));
        }

        public void SchedulerError(string msg, SchedulerException cause)
        {
            _logger.Error(String.Concat("SchedulerError", msg), cause);
        }

        public void SchedulerInStandbyMode()
        {
            _logger.Debug("SchedulerInStandbyMode");
        }

        public void SchedulerStarted()
        {
            _logger.Debug("SchedulerStarted");
        }

        public void SchedulerStarting()
        {
            _logger.Debug("SchedulerStarting");
        }

        public void SchedulerShutdown()
        {
            _logger.Debug("SchedulerShutdown");
        }

        public void SchedulerShuttingdown()
        {
            _logger.Debug("SchedulerShuttingdown");
        }

        public void SchedulingDataCleared()
        {
            _logger.Debug("SchedulingDataCleared");
        }
    }
}