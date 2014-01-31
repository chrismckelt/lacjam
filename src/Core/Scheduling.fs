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
            abstract scheduleBatch<'a when 'a :> IJob> : Lacjam.Core.Batch * TriggerBuilder -> unit
            abstract scheduleBatch<'a when 'a :> IJob> : Lacjam.Core.Batch -> unit
            abstract member processBatch : Batch -> unit
            abstract member Scheduler : IScheduler with get 
            abstract member createTrigger : TriggerBuilder  with get, set
    

    type BatchMessage = ILogWriter * IBus * Jobs.JobMessage * AsyncReplyChannel<Jobs.JobResult>

    type JobScheduler(log:ILogWriter, sched:IScheduler, bus:IBus) = 
        do sched.Start() |> ignore
        do log.Write(Info("-- Scheduler started --"))   
        let mutable triggerBuilder = TriggerBuilder.Create().WithCalendarIntervalSchedule(fun a-> (a.WithInterval(1, IntervalUnit.Minute) |> ignore))
        interface IJobScheduler with 
                override this.createTrigger with get() = triggerBuilder  and set(v) = triggerBuilder <- v           
                override this.scheduleBatch<'a when 'a :> IJob>(batch:Lacjam.Core.Batch, trgBuilder:TriggerBuilder) = 
                                                                let jobDetail = new JobDetailImpl(batch.Name,  batch.BatchId.ToString(), typedefof<'a>)
                                                                let found = sched.GetJobDetail(jobDetail.Key)
                                                                let trigger = trgBuilder.Build()
                                                                match found with 
                                                                    | null -> sched.ScheduleJob(jobDetail, trigger) |> ignore
                                                                    | _ -> sched.RescheduleJob(new TriggerKey(trigger.Key.Name), trigger) |> ignore
                override this.scheduleBatch<'a when 'a :> IJob>(batch:Lacjam.Core.Batch) = 
                                                                let jobDetail = new JobDetailImpl(batch.Name,  batch.BatchId.ToString(), typedefof<'a>)
                                                                let found = sched.GetJobDetail(jobDetail.Key)
                                                                let triggerBuilder = match batch.TriggerBuilder with | null -> triggerBuilder | _ -> batch.TriggerBuilder
                                                                let trigger = triggerBuilder.Build()
                                                                match found with 
                                                                    | null -> sched.ScheduleJob(jobDetail, trigger) |> ignore
                                                                    | _ -> sched.RescheduleJob(new TriggerKey(trigger.Key.Name), trigger) |> ignore
                                                           
                
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
                                                                                                                                                                                                                        replyChannel.Reply(new Jobs.JobResult(Guid.NewGuid(), Guid.NewGuid(), false, msg))   
                                                                                                                                                                                                            | _     ->
                                                                                                                                                                                                                
                                                                                                                                                                                
                                                                                                                                                                                                                try
                                                                                                                                                                                                                            if (a.ErrorCode > 0 ) then
                                                                                                                                                                                                                                log.Write(Info("ErrorCode-returned: " + a.ErrorCode.ToString()))  

                                                                                                                                                                                                                            match a.Messages.FirstOrDefault() with
                                                                                                                                                                                                                            | null ->  
                                                                                                                                                                                                                                    log.Write(Debug("JobResult -- not returned messages for JobResult")) 
                                                                                                                                                                                                                                    log.Write(Debug("Async State -- " + a.State.ToString()))
                                                                                                                                                                                                                                    replyChannel.Reply(new Jobs.JobResult(jobMessage.Id, jobMessage.Id, true, "No messages results")) 
                                                                                                                                                                                                
                                                                                                                                                                                                                            | b ->
                                                                                                                                                                                                                                    let jr = (b :?> Jobs.JobResult)
                                                                                                                                                                                                                                    log.Write(Debug("JobResult -- " + jr.GetType().Name))
                                                                                                                                                                                                                                    log.Write(Debug("JobResult -- " + jr.ToString())) 
                                                                                                                                                                                                                                    //TODO send original job message update
                                                                                                                                                                                                                                    replyChannel.Reply(jr) 
                                                                                                                                                                                                                with | ex -> log.Write(Error("Job failed", ex, false))
                                                                                                                                                                                                                             replyChannel.Reply(new Jobs.JobResult(Guid.NewGuid(), Guid.NewGuid(), false, "Error: " + ex.Message))   
                                                                                                                                                                           )  |> ignore
                                                                                                                                                                           
                                                                                                                                          with | ex -> log.Write(Error("Job failed", ex, false))
                                                                                                                        do! loop (n + 1)
                                                                                                                }
                                                                                                            loop 0)
                                                                                                
                                                    let mutable payload = batch.Jobs.Head.Payload
                                                    for job in batch.Jobs do
                                                        try
                                                            job.Payload <- payload
                                                            let reply = agent.PostAndReply(fun replyChannel -> log, bus, job, replyChannel)
                                                            payload <- reply.Result
                                                            log.Write(Info("Reply: %s" + reply.ToString()))
                                                        with | ex -> log.Write(Error("Job failed", ex, false))
                                                    
                                                    ()
                
                member this.Scheduler = sched


      type ProcessBatch() =
        interface IJob with
            member this.Execute(context : IJobExecutionContext)  =      let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                                                                        let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                                                                        let batchName = context.JobDetail.Key.Name
                                                                        Console.WriteLine(batchName)
                                                                        Console.WriteLine("Job is executing - {0}.", DateTime.Now)
                                                                        try
                                                                            let js = Ioc.Resolve<IJobScheduler>()
                                                                            let asses = AppDomain.CurrentDomain.GetAssemblies().Where(fun a-> a.FullName.Contains("Lacjam"))
                                                                            for ass in asses do
                                                                                let types = ass.GetTypes()
                                                                                for ty in types do
                                                                                    let cb = ty.GetInterface(typedefof<IContainBatches>.FullName)
                                                                                    match cb with
                                                                                        | null -> ()
                                                                                        | _ -> 
                                                                                                let batches = Activator.CreateInstance(ty) :?> IContainBatches
                                                                                                let b = batches.Batches.Head
                                                                                                js.processBatch(b) 
                                                                               
                                                                        with | ex -> log.Write(Error("Job failed", ex, false)) 
             
            

    let callBackReceiver (result:CompletionResult) = 
            Console.WriteLine("--- CALLBACK ---")
            // TODO Audit

   
        
  