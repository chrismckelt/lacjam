using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lacjam.Core;
using Lacjam.Worker.Jobs;
using NServiceBus;
using NServiceBus.Logging.Loggers.Log4NetAdapter;
using NServiceBus.Unicast;

namespace Lacjam.Worker.Handlers
{
    public abstract class HandlerBase<T> : IHandleMessages<T> where T : JobBase
    {
        public IBus Bus { get; protected set; }
        public Runtime.ILogWriter Logger { get; protected  set; }

        protected HandlerBase(IBus bus, Runtime.ILogWriter logger)
        {
            Bus = bus;
            Logger = logger;
        }


        protected void Reply(JobResult result)
        {
            Logger.Info("Handler:Reply:" + this.GetType().Name);
            Logger.Debug(result.ToString());
            Bus.Reply(result);
        }


        protected async Task<JobResult> Run(T jobToProcess)
        {
            return await Run(jobToProcess, false);
        }

        protected async Task<JobResult> Run(T job, bool resubmitOnFailure)
        {
            try
            {   
                Logger.Debug(job.ToString());
                return await Bus.Send(job).Register<JobResult>(result => ProcessResult(job, result));
            }
            catch (Exception ex)
            {
                Logger.Error(job.GetType().Name, ex);
                if (job.GetType() != typeof (JobResult))
                {
                    var jr = new JobResult(job, false, ex.Message, resubmitOnFailure, ex);
                    return jr;
                }
                throw;
            }
        }

        private JobResult ProcessResult(JobBase job, CompletionResult completionResult)
        {
            Logger.Info(string.Format("Job Completed: JobId:{0}  Result:{1}", job.Id,completionResult.ErrorCode==0));

            if (completionResult.Messages != null  && completionResult.Messages.Any())
            {
                object first = completionResult.Messages.FirstOrDefault();
                var jr = first as JobResult;
                if (jr != null)
                {
                    Logger.Debug(jr.ToString()); //TODO audit
                    return jr;
                }
                else
                {
                    Logger.Info("ProcessResult - message ??? : " + jr.GetType());
                }
            }
            throw new NotImplementedException();
        }

        public abstract void Handle(T message);
    }
}
