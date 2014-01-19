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
            abstract member scheduleBatch<'a when 'a :> IJob> : unit -> unit
            abstract member processBatch : Batch -> unit
            abstract member Scheduler : IScheduler with get 
            abstract createTrigger : unit -> TriggerBuilder 

    type BatchMessage = ILogWriter * IBus * Jobs.JobMessage * AsyncReplyChannel<Jobs.JobResult>

    type JobScheduler(log:ILogWriter, sched:IScheduler, bus:IBus) = 
//        let sf = new StdSchedulerFactory()
//        do sf.Initialize() |> ignore
//        let sc = sf.GetScheduler()
        do sched.Start() |> ignore
        //new() = new JobScheduler()
        interface IJobScheduler with 
                member this.createTrigger() = TriggerBuilder.Create().WithCalendarIntervalSchedule(fun a-> (a.WithInterval(1, IntervalUnit.Minute).Build() |> ignore))
                member this.scheduleBatch<'a when 'a :> IJob>() = 
                                                                let jobDetail = new JobDetailImpl(typedefof<'a>.Name, typedefof<'a>.Name, typedefof<'a>)
                                                                let trigger = TriggerBuilder.Create().WithSimpleSchedule(fun a-> (a.WithInterval(TimeSpan.FromSeconds(Convert.ToDouble(10))).Build() |> ignore)).ForJob(jobDetail).StartNow().Build()
                                                                let found = sched.GetJobDetail(jobDetail.Key)
                                                                match found with 
                                                                    | null -> sched.ScheduleJob(jobDetail, trigger) |> ignore
                                                                    | _ -> sched.RescheduleJob(new TriggerKey(trigger.Key.Name), trigger) |> ignore
                
                member this.processBatch(batch) =   
                                                    let agent = MailboxProcessor<BatchMessage>.Start(fun proc -> 
                                                                                                   async {
                                                                                                        try
                                                                                                            let! (log, bus, jobMessage, replyChannel) = proc.Receive()
                                                                                                            log.Write(Debug("Sending -- " + jobMessage.GetType().Name))
                                                                                                            bus.Send(jobMessage).Register(fun (a:CompletionResult) ->   let b = (a.Messages.First())
                                                                                                                                                                        let jr = (b :?> Jobs.JobResult)
                                                                                                                                                                        log.Write(Debug("JobResult -- " + jr.ToString()))|> ignore
                                                                                                                                                                        replyChannel.Reply(jr)
                                                                                                                                           )                            |> ignore
                                                                                                            ()
                                                                                                         with | ex -> printf "%s" ex.Message
                                                                                                    })
                                                                                                
                                                    let mutable payload = batch.Jobs.Head.Payload
                                                    for job in batch.Jobs do
                                                        job.Payload <- payload
                                                        let reply = agent.PostAndReply(fun replyChannel -> log, bus, job, replyChannel)
                                                        payload <- reply.Result
                                                        log.Write(Info("Reply: %s" + reply.ToString()))
                                                    

                                                    ()
                
                member this.Scheduler = sched


      type ProcessBatch() =
        interface IJob with
            member this.Execute(context : IJobExecutionContext)  =      let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                                                                        let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                                                                        let tw = context.JobDetail.Key.Name
                                                                        Console.WriteLine(tw)
                                                                        Console.WriteLine("Job is executing - {0}.", DateTime.Now)
                                                                      //  let msg = AppDomain.CurrentDomain.CreateInstanceFrom(@"Lacjam.Core.dll", tw).CreateObjRef(typedefof<Jobs.SwellNetRatingJob>)
                                                                        let act = (Activator.CreateInstance("Lacjam.Integration", "Lacjam.Integration.Jobs.SwellNetRatingJob").InitializeLifetimeService()  :?> IMessage)
                                                                        let batchProcessor = MailboxProcessor<Batch>.Start(fun batch ->        
                                                                            async {
                                                                                try
                                                                                    let! ba = batch.Receive()
                                                                                    Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>().Write(Info("Processing Batch " + ba.BatchId.ToString()))
//                                                                                    if (ba > 5) then
//                                                                                        replyChannel.Reply(message+1)
//                                                                                    else
//                                                                                        replyChannel.Reply(message+10)
                                                                                 with | ex -> printf "%s" ex.Message
                                                                            })
                                                                        ()
             
            

//    [<AbstractClass>]
//    type SchedulerSetup<'a when 'a :> IJob>(scheduler:IScheduler, log:ILogWriter)= 
//            
//            abstract createTrigger : TriggerBuilder 
//            default val createTrigger = TriggerBuilder.Create().WithDescription("10-Second-Intervals-Forever").WithSimpleSchedule(fun a-> a.WithIntervalInSeconds((10)).RepeatForever().Build() |> ignore).StartNow()
//            
//            abstract member JobDetail : IJobDetail
//            default val JobDetail =  (JobBuilder.Create<'a:>IJob>().WithIdentity(typedefof<'a>.Name + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + Guid.NewGuid().ToString()).Build())
//              
//            abstract member Run :  unit -> unit
//            default this.Run() = scheduler.ScheduleJob(this.JobDetail, this.createTrigger.ForJob(this.JobDetail).Build())  |> ignore
//
//            interface IWantToRunWhenBusStartsAndStops with
//                member this.Start() =
//                                do log.Write(Debug(typedefof<'a>.Name + ".IWantToRunWhenBusStartsAndStops.Start -- Run"))  
//                                do this.Run()
//                member this.Stop() =  ()

//    type QuartzJobFactory(container:NServiceBus.ObjectBuilder.IBuilder) = 
//        interface IJobFactory with 
//            override x.NewJob((bundle:TriggerFiredBundle), scheduler:IScheduler) = container.Build(bundle.JobDetail.JobType) :?> IJob
//            override x.ReturnJob(job:IJob) =  ()

    let callBackReceiver (result:CompletionResult) = 
            Console.WriteLine("--- CALLBACK ---")
//            let msg = (Seq.head result.Messages) :?> Jobs.JobResult
//            let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
//            try
//                log.Write(LogMessage.Debug("--- Message Received ---"))
//                log.Write(LogMessage.Debug(msg.Id.ToString()))
//            with | ex -> log.Write(LogMessage.Warn("Callback failed for " + result.ErrorCode.ToString(), ex))

   
        
  