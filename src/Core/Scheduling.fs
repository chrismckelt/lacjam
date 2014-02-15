namespace Lacjam.Core

module Scheduling =
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
    open System.Linq
    open System.Net.Http
    open System.Runtime.Serialization
    open System.Text.RegularExpressions
    open Quartz
    open Quartz.Spi
    open Quartz.Impl
                                                                 
   
    type IJobScheduler = 
            abstract scheduleBatch : Lacjam.Core.Batch -> unit
            abstract member processBatch : Batch -> unit
            abstract member Scheduler : IScheduler with get 
         //   abstract member createTrigger : TriggerBuilder  with get, set
    

    type BatchMessage = ILogWriter * IBus * Jobs.JobMessage * AsyncReplyChannel<Jobs.JobResult>

    type JobSchedulerListener(log:ILogWriter, bus:IBus) =
        interface Quartz.ISchedulerListener with
            override this.JobAdded(jd) = log.Write(Debug("JobAdded: " + jd.JobType.Name.ToString() + " " + jd.Key.Group + " " + jd.Key.Name))
            override this.JobDeleted(jd) = log.Write(Debug("JobDeleted: " + jd.Group + " " + jd.Name))
            override this.JobPaused(jd) = log.Write(Debug("JobPaused: " + jd.Group + " " + jd.Name))
            override this.JobsPaused(jd) = log.Write(Debug("JobsPaused: " + jd))
            override this.JobResumed(jd) = log.Write(Debug("JobResumed: " + jd.Group + " " + jd.Name))
            override this.JobsResumed(jd) = log.Write(Debug("JobsResumed: " + jd))
            override this.JobScheduled(jd) = log.Write(Debug("JobScheduled: " + jd.JobKey.Group + " " + jd.JobKey.Name + " " + jd.Key.Group + " " + jd.Key.Name))
            override this.JobUnscheduled(jd) = log.Write(Debug("JobUnScheduled: " + jd.Group + " " + jd.Name))
            override this.TriggerFinalized(trg) = log.Write(Debug("TriggerFinalized: " + trg.Key.Group + " " + trg.Key.Name))
            override this.TriggerPaused(trg) = log.Write(Debug("TriggerPaused: " + trg.Group + " " + trg.Name))
            override this.TriggersPaused(trg) = log.Write(Debug("TriggersPaused: " + trg))
            override this.TriggerResumed(trg) = log.Write(Debug("TriggerResumed: " + trg.Group + " " + trg.Name))
            override this.TriggersResumed(trg) = log.Write(Debug("TriggersResumed: " + trg))
            override this.SchedulerError(msg, ex) = log.Write(Error("SchedulerError: " + msg,ex,false))
            override this.SchedulerInStandbyMode() = log.Write(Info("SchedulerInStandbyMode: "))
            override this.SchedulerStarted() = log.Write(Info("SchedulerStarted: "))
            override this.SchedulerStarting() = log.Write(Info("SchedulerStarting: "))
            override this.SchedulerShutdown() = log.Write(Info("SchedulerShutdown: "))
            override this.SchedulerShuttingdown() = log.Write(Info("SchedulerShuttingdown: "))
            override this.SchedulingDataCleared() = log.Write(Info("SchedulingDataCleared: "))
    
    type BatchFinder =
        static member FindBatches:Collections.Generic.List<IContainBatches> =               let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                                                                                            let asses = AppDomain.CurrentDomain.GetAssemblies().Where(fun a-> a.FullName.Contains("Lacjam"))
                                                                                            let batchList = new Collections.Generic.List<IContainBatches>()
                                                                                            for ass in asses do
                                                                                                let types = ass.GetTypes()
                                                                                                for ty in types do
                                                                                                    let cb = ty.GetInterface(typedefof<IContainBatches>.FullName)
                                                                                                    match cb with
                                                                                                        | null ->  ()//log.Write(Warn("ProcessBatch.Execute: IContainBatches NO BATCH FILES FOUND", new InvalidOperationException("ProcessBatch.Execute: IContainBatches NO BATCH FILES FOUND")))
                                                                                                        | _ -> 
                                                                                                                log.Write(Debug("BatchFinder  IContainBatches --. " + cb.FullName))
                                                                                                                let batches = Activator.CreateInstance(ty) :?> IContainBatches
                                                                                                                batchList.Add(batches)
                                                                                            log.Write(Info("FindBatches count :" +  batchList.Count.ToString()))
                                                                                            batchList       
    
    type ProcessBatch() =
        interface IJob with
            member this.Execute(context : IJobExecutionContext)  =      let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                                                                        let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                                                                        let js = Ioc.Resolve<IJobScheduler>()
                                                                        try
                                                                            log.Write(Info("Scheduling.ProcessBatch IJob Execute - " + DateTime.Now.ToString()))
                                                                            log.Write(Info("context.JobDetail.Key.Group" + context.JobDetail.Key.Group))
                                                                            log.Write(Info("context.JobDetail.Key.Name" + context.JobDetail.Key.Name))
                                                                            let batchName = context.JobDetail.Key.Name
                                                                            log.Write(Info("Scheduling.ProcessBatch.Execute : BatchName = " + batchName))
                                                                           
                                                                            for icb in BatchFinder.FindBatches.ToList() do
                                                                                for b in icb.Batches do
                                                                                    if (b.Name = batchName) then // -------------------------- match batch name on job name to run
                                                                                        log.Write(Info("ProcessBatch.Execute:" + b.Name))
                                                                                        try
                                                                                            js.processBatch(b) 
                                                                                        with | ex ->    log.Write(Error("ProcessBatch.Execute: Individual batch failed: " + b.Name, ex, false)) 
                                                                                                        log.Write(Debug(b.ToString()))
                                                                                                
                                                                                        log.Write(Debug("ProcessBatch.Execute: SUCCESS"))
                                                                                        context.Result <- true     
          
                                                                        with | ex ->    log.Write(Debug("ProcessBatch.Execute: FAILED"))
                                                                                        log.Write(Error("Job failed", ex, false))         

    type JobScheduler(log:ILogWriter, bus:IBus, scheduler:IScheduler) =        
       
        do log.Write(Info("-- Scheduler started --"))  
        let mutable triggerBuilder = TriggerBuilder.Create().WithCalendarIntervalSchedule(fun a-> (a.WithInterval(1, IntervalUnit.Minute) |> ignore))
        let handleBatch (batch:Batch)         =                         let jobDetail = new JobDetailImpl(batch.Name,  batch.TriggerName, typedefof<ProcessBatch>,true,true)
                                                                        let found = scheduler.GetJobDetail(jobDetail.Key)
                                                                        
                                                                        let tk = new TriggerKey(batch.TriggerName)
                                                                        log.Write(Debug("JobScheduler.handleBatch : TriggerKey(batch.TriggerName) " + tk.Name ))
//                                                                        let trigger = match scheduler.GetTrigger(tk) with
//                                                                                          | null -> 
//                                                                                                    log.Write(Debug("JobScheduler.handleBatch : Trigger not found - creating new one " + batch.TriggerName ))
//                                                                                                    TriggerBuilder.Create().ForJob(jobDetail).WithIdentity(Lacjam.Core.BatchSchedule.Hourly.ToString()).StartNow().WithDescription("hourly").WithSimpleSchedule(fun a->a.RepeatForever().WithIntervalInMinutes(15).WithMisfireHandlingInstructionFireNow() |> ignore).Build()             
//                                                                                          | trg -> 
//                                                                                                    log.Write(Debug("JobScheduler.handleBatch : Trigger found : " + batch.TriggerName ))
//                                                                                                    trg
                                                                        log.Write(Debug("scheduler.GetTriggerGroupNames()"))
                                                                        for grp in scheduler.GetTriggerGroupNames() do
                                                                            log.Write(Debug("Group = " + grp))
                                                                       
                                                                        jobDetail.Durable <- true
                                                                        jobDetail.Name <- batch.Name
                                                                        jobDetail.RequestsRecovery <- true
                                                                        jobDetail.Description <- batch.TriggerName

                                                                        let mutable trigger = scheduler.GetTrigger(tk)
                                                                        match trigger with 
                                                                        | null ->   log.Write(Debug("handleBatch - trigger = scheduler.GetTrigger(tk) - trigger null  - trigger key = " + tk.Name))
                                                                                    trigger <- scheduler.GetTriggersOfJob(jobDetail.Key).FirstOrDefault()
                                                                                    log.Write(Debug("scheduler.GetTriggersOfJob(jobDetail.Key) - trigger.Key.Name - = " + trigger.Key.Name))
                                                                        | _ -> trigger <- trigger.GetTriggerBuilder().ForJob(jobDetail).Build()

                                                                        match found with 
                                                                            | null -> 
                                                                                log.Write(Debug("JobScheduler.handleBatch : calling scheduler.ScheduleJob(jobDetail, trigger) " + jobDetail.FullName + "  " + trigger.Key.Name )) 
                                                                                if not <| (scheduler.CheckExists(jobDetail.Key))  then
                                                                                    log.Write(Debug("---  scheduler.ScheduleJob(jobDetail, trigger.GetTriggerBuilder().ForJob(jobDetail).Build())  ---"))
                                                                                    scheduler.ScheduleJob(jobDetail, trigger) |> ignore
                                                                                else
                                                                                    log.Write(Debug("---  scheduler.RescheduleJob(tk,trigger) |> ignore  ---"))
                                                                                    scheduler.RescheduleJob(tk,trigger) |> ignore
                                                                            | _ -> 
                                                                                    scheduler.RescheduleJob(tk,trigger) |> ignore
                                                                                    log.Write(Debug("--- 2 scheduler.RescheduleJob(tk,trigger) |> ignore  ---"))
                                                                        let dto = trigger.GetNextFireTimeUtc()
                                                                        match dto.HasValue with
                                                                        | true -> log.Write(Info(jobDetail.Name + " next fire time (local) " + dto.Value.ToLocalTime().ToString()))
                                                                        | false -> log.Write(Info(jobDetail.Name + " not scheduled triggers" ))
        interface IJobScheduler with 
                                                                
                override this.scheduleBatch(batch:Lacjam.Core.Batch) =  handleBatch batch 
                                                           
                
                member this.processBatch(batch) =   
                                                    let agent = MailboxProcessor<BatchMessage>.Start(fun proc -> 
                                                                                                            let rec loop n =
                                                                                                                async {
                                                                                                                        try
                                                                                                                                            let! (log, bus, jobMessage, replyChannel) = proc.Receive()
                                                                                                                                            log.Write(Debug("Sending -- " + jobMessage.GetType().Name))
                                                                                                                                            bus.Send(jobMessage).Register(fun (a:CompletionResult) ->           
                                                                                                                                                                                                        match a with
                                                                                                                                                                                                            | null ->   let msg = "No Completion Result reply for NServiceBus Job"
                                                                                                                                                                                                                        log.Write(Info(msg))  
                                                                                                                                                                                                                        replyChannel.Reply(new Jobs.JobResult(jobMessage,false, msg))   
                                                                                                                                                                                                            | _     ->
                                                                                                                                                                                                                
                                                                                                                                                                                
                                                                                                                                                                                                                try
                                                                                                                                                                                                                            if (a.ErrorCode > 0 ) then
                                                                                                                                                                                                                                log.Write(Info("ErrorCode-returned: " + a.ErrorCode.ToString()))  

                                                                                                                                                                                                                            match a.Messages.FirstOrDefault() with
                                                                                                                                                                                                                            | null ->  
                                                                                                                                                                                                                                    log.Write(Debug("JobResult -- not returned messages for JobResult")) 
                                                                                                                                                                                                                                    log.Write(Debug("Async State -- " + a.State.ToString()))
                                                                                                                                                                                                                                    replyChannel.Reply(new Jobs.JobResult(jobMessage,true, "No messages results")) 
                                                                                                                                                                                                
                                                                                                                                                                                                                            | b ->
                                                                                                                                                                                                                                    let jr = (b :?> Jobs.JobResult)
                                                                                                                                                                                                                                    log.Write(Debug("JobResult -- " + jr.GetType().Name))
                                                                                                                                                                                                                                    log.Write(Debug("JobResult -- " + jr.ToString())) 
                                                                                                                                                                                                                                    //TODO send original job message update
                                                                                                                                                                                                                                    replyChannel.Reply(jr) 
                                                                                                                                                                                                                with | ex -> log.Write(Error("Job failed", ex, false))
                                                                                                                                                                                                                             replyChannel.Reply(new Jobs.JobResult(jobMessage, false, "Error: " + ex.Message))   
                                                                                                                                                                           )  |> ignore
                                                                                                                                                                           
                                                                                                                        with | ex ->    log.Write(Info("Scheduling.processBatch -- MailboxProcessor async loop failure"))
                                                                                                                                        log.Write(Info("--------------------------------------------------"))
                                                                                                                                        log.Write(Info("--------------------------------------------------"))
                                                                                                                                        log.Write(Error("Job failed", ex, false))
                                                                                                                                        log.Write(Info("--------------------------------------------------"))
                                                                                                                                        log.Write(Info("--------------------------------------------------"))
                                                                       
                                                                                                                        do! loop (n + 1)
                                                                                                                }
                                                                                                            loop 0)
                                                                                                
                                                    let mutable payload = batch.Jobs.FirstOrDefault().Payload
                                                    for job in batch.Jobs do
                                                        try
                                                            job.Payload <- payload
                                                            log.Write(Info(job.GetType().Name))
                                                            log.Write(Info("-- OLD Payload --"))
                                                            log.Write(Info(job.Payload))
                                                            let reply = agent.PostAndReply(fun replyChannel -> log, bus, job, replyChannel)
                                                            log.Write(Info("JobResult received for " + job.GetType().Name))                                                            
                                                            if (reply.Success) then
                                                                log.Write(Info("JobResult.Success=true"))
                                                                payload <- reply.Result
                                                                log.Write(Info("-- NEW Payload --"))
                                                                log.Write(Info(payload))
                                                                log.Write(Info("Reply: %s" + reply.ToString()))
                                                            else
                                                                log.Write(Info("JobResult.Success=false"))
                                                                log.Write(Info(job.ToString()))
                                                                handleBatch batch                 //rescheduled?
                                                        with | ex -> log.Write(Error("Job failed", ex, false))
                                                    
                                                    ()
                
                member this.Scheduler =  scheduler
      
      
             
     type BatchSubmitterJobHandler(log : Lacjam.Core.Runtime.ILogWriter, js : IJobScheduler) =
            do log.Write(Info("BatchSubmitterJob"))
            interface NServiceBus.IHandleMessages<BatchSubmitterJob> with
                member x.Handle(job) =
                    try
                        log.Write(LogMessage.Info("Handling Batch  : " + job.ToString()))
                        log.Write(Info("EndpointConfig.Init :: SchedulerName = " + js.Scheduler.SchedulerName))
                        log.Write(Info("EndpointConfig.Init :: IsStarted = " + js.Scheduler.IsStarted.ToString()))
                        log.Write(Info("EndpointConfig.Init :: SchedulerInstanceId = " + js.Scheduler.SchedulerInstanceId.ToString()))
                        js.scheduleBatch(job.Batch)
                    with ex ->
                        log.Write(LogMessage.Error("ERROR- BatchSubmitterJobHandler : " + job.ToString(), ex, true))        

    
    let callBackReceiver (result:CompletionResult) = 
            Console.WriteLine("--- CALLBACK ---")
            // TODO Audit

   
        
  