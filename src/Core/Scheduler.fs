namespace Lacjam.Core

module Scheduler =
    open Autofac
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain
    open Lacjam.Core.Runtime
    open NServiceBus
    open System
    open System.Collections.Concurrent
    open System.Collections.Generic
    open System.IO
    open System.Net
    open System.Net.Http
    open System.Runtime.Serialization
    open System.Text.RegularExpressions
    open Quartz
    open Quartz.Spi

    [<AbstractClass>]
    type SchedulerSetup<'a when 'a :> IJob>(scheduler:IScheduler, log:ILogWriter)= 
            
            abstract createTrigger : TriggerBuilder 
            default val createTrigger = TriggerBuilder.Create().WithCalendarIntervalSchedule(fun a-> (a.WithInterval(1, IntervalUnit.Minute).Build() |> ignore))
            
            abstract member JobDetail : IJobDetail
            default val JobDetail =  (JobBuilder.Create<'a:>IJob>().WithIdentity(typedefof<'a>.Name + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + Guid.NewGuid().ToString()).Build())
              
            abstract member Run :  unit -> unit
            default this.Run() =
                                if not <| (scheduler.IsStarted) then
                                    scheduler.Start()
                                    log.Write(Info("-- Quartz Schedule Started --"))
                                    let jf = Lacjam.Core.Runtime.Ioc.Resolve<IJobFactory>()
                                    log.Write(Info("IJobFactory = " + jf.ToString()))

                                scheduler.ScheduleJob(this.JobDetail, this.createTrigger.Build())  |> ignore

            interface IWantToRunWhenBusStartsAndStops with
                member this.Start() =
                                do log.Write(Debug("SwellNetRatingJobScheduler.IWantToRunWhenBusStartsAndStops.Start -- Run"))  
                                do this.Run()
                member this.Stop() =  ()

    type QuartzJobFactory(container:NServiceBus.ObjectBuilder.IBuilder) = 
        interface IJobFactory with 
            override x.NewJob((bundle:TriggerFiredBundle), scheduler:IScheduler) = container.Build(bundle.JobDetail.JobType) :?> IJob
            override x.ReturnJob(job:IJob) =  ()

    let callBackReceiver (result:CompletionResult) = 
            Console.WriteLine("--- CALLBACK ---")
//            let msg = (Seq.head result.Messages) :?> Jobs.JobResult
//            let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
//            try
//                log.Write(LogMessage.Debug("--- Message Received ---"))
//                log.Write(LogMessage.Debug(msg.Id.ToString()))
//            with | ex -> log.Write(LogMessage.Warn("Callback failed for " + result.ErrorCode.ToString(), ex))

    /// Fantomas
    /// Ctrl + K D   -- format document
    /// Ctrl + K F   -- format selection / format cursor position
    module Jobs =
        open Lacjam
        open Lacjam.Core
        open Lacjam.Core.Domain
        open Lacjam.Core.Runtime
        open NServiceBus
        open System
        open System.Collections.Concurrent
        open System.Collections.Generic
        open System.IO
        open System.Net
        open System.Net.Http
        open System.Runtime.Serialization
        open System.Text.RegularExpressions
        open Quartz
        open Quartz.Spi
        open Autofac
        open NServiceBus.ObjectBuilder
        open NServiceBus.ObjectBuilder.Common

        [<Serializable>]
        [<AbstractClass>]
        type JobMessage() =
            member val Id = Guid.Empty with get,set
            member val BatchId = Guid.Empty with get,set
            member val CreatedDate = DateTime.UtcNow with get
            member val Payload = "" with get, set
            member val Status = false with get, set
//            member val JobDetail =   { new IJobDetail with 
//                                         member this.Description = "job default description"
//                                         member this.Key = new JobKey("job")
//                                         member this.ConcurrentExecutionDisallowed = false
//                                         member this.GetJobBuilder() = (JobBuilder.Create())
//                                         member this.JobType =  typedefof<JobMessage>
//                                         member this.JobDataMap = null
//                                         member this.RequestsRecovery = false
//                                         member this.PersistJobDataAfterExecution = false
//                                         member this.Durable = false
//                                         member this.Clone() = new obj()
//                                     }  with get, set
            interface IMessage


//        type ScheduledJobMessage(jobMessage) =
//            interface IJobDetail with 
//                member this.Description = name
//                member this.Key = new JobKey(name)
//                member this.ConcurrentExecutionDisallowed = false
//                member this.GetJobBuilder() = (JobBuilder.Create())
//                member this.JobType =  typedefof<JobMessage>
//                member this.JobDataMap = null
//                member this.RequestsRecovery = false
//                member this.PersistJobDataAfterExecution = false
//                member this.Durable = false
                member this.Clone() = new obj()

        [<Serializable>]
        type JobResult(id : Guid, resultForJobId : Guid, success : bool, result : string) =
            let mutable i = id
            let mutable r = resultForJobId
            let mutable suc = success
            let mutable res = result
            member x.Id with get () = i and set(v) = i <- v
            member x.ResultForJobId with get () = r and set(value) = r <- value
            member val CreatedDate = DateTime.UtcNow with get

            member x.Success with get () = suc and set (v : bool) = suc <- v

            member x.Result with get () = res and set(v) = res <- v

            override x.ToString() =
                String.Format
                    ("{0} {1} {2} {3}", x.Id, x.ResultForJobId, x.CreatedDate,
                     x.Success.ToString())
            interface IMessage

        [<Serializable>]
        type PageScraperJob() =
            inherit JobMessage()
            interface IMessage

     type BatchJob() =
            member val Id = Guid.Empty with get,set
            member val Name = String.Empty with get,set
            member val BatchId = Guid.Empty with get,set
            member val CreatedDate = DateTime.UtcNow with get
            member val Jobs = new List<Jobs.JobMessage>() with get,set

    module JobHandlers =
        open Autofac
        open Lacjam
        open Lacjam.Core
        open Lacjam.Core.Domain
        open Lacjam.Core.Runtime
        open NServiceBus
        open NServiceBus.MessageInterfaces
        open System
        open System.Collections.Concurrent
        open System.Collections.Generic
        open System.IO
        open System.Net
        open System.Net.Http
        open System.Runtime.Serialization
        open System.Text.RegularExpressions
        open log4net

        type JobResultHandler(log : Lacjam.Core.Runtime.ILogWriter) =
            interface NServiceBus.IHandleMessages<Jobs.JobResult> with
                member x.Handle(jr) =
                    try
                        log.Write(LogMessage.Debug(jr.ToString()))
                    with ex ->
                        log.Write(LogMessage.Error(jr.ToString(), ex, true))

        type PageScraperJobHandler(log : ILogWriter) =
            interface IHandleMessages<Jobs.PageScraperJob> with
                member x.Handle(job) =
                    match job.Payload with
                    | "" -> failwith "Job.Payload empty"
                    | _ ->
                        log.Write
                            (LogMessage.Debug
                                 (job.CreatedDate.ToString() + "   "
                                  + job.GetType().ToString()))
                        let html =
                            match Some(job.Payload) with
                            | None -> failwith "URL to job scrape required"
                            | Some(a) ->
                                let client = new System.Net.WebClient()
                                let result = client.DownloadString(a)
                                result

                        let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                        try
                            let jr = Jobs.JobResult(Guid.NewGuid(),job.Id, true, html)
                            bus.Reply(jr)
                        with ex ->
                            log.Write
                                (LogMessage.Error
                                     (job.GetType().ToString(), ex, true)) //Console.WriteLine(html)
        
