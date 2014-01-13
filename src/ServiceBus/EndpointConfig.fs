namespace Lacjam.ServiceBus 
    open System
    open System.IO
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Utility
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduler
    open Lacjam.Core.Scheduler.Jobs
    open Lacjam.Integration
    open StartupBatchJobs
    open Quartz
    open Quartz.Spi
    open Autofac
    open NServiceBus.ObjectBuilder
    open NServiceBus.ObjectBuilder.Common

    module Startup = 

        let CallBackReceiver (result:CompletionResult) = 
                Console.WriteLine("--- CALLBACK ---")
                let msg = (Seq.head result.Messages) :?> Lacjam.Core.Scheduler.Jobs.JobResult
                let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                try
                    log.Write(LogMessage.Debug("--- Message Received ---"))
                    log.Write(LogMessage.Debug(msg.Id.ToString()))
                with | ex -> log.Write(LogMessage.Warn("Callback failed for " + result.ErrorCode.ToString(), ex))


        type SchedulerSetup<'a when 'a :> IJob>(scheduler:IScheduler) = 
                
                let run = 
                        let typeOfJob = typedefof<'a>
                        let jobName = typeOfJob.Name
                        let jobKey = new JobKey(jobName)

                        let jobDetail = JobBuilder.Create<'a:>IJob>().WithIdentity(jobKey).Build()
                        let trigger = SchedulerSetup<'a>(scheduler).createTrigger.ForJob(jobDetail).Build()
                        match scheduler.GetJobDetail(jobKey) with 
                        | null -> scheduler.ScheduleJob(jobDetail, trigger)
                        | _ -> 
                             let triggerName = (typedefof<'a>.Name + "-CronTrigger")
                             let result = scheduler.RescheduleJob(new TriggerKey(triggerName), trigger)
                             if (result.HasValue) then 
                                result.Value
                             else
                                DateTimeOffset.Now
                abstract member createTrigger : TriggerBuilder 

                interface IWantToRunWhenBusStartsAndStops with
                    member this.Start() =  run |> ignore
                    member this.Stop() =   run |> ignore

                default val createTrigger = TriggerBuilder.Create().StartNow()
                

        type EndpointConfig() =
            interface IConfigureThisEndpoint
            interface AsA_Server
            interface IWantCustomInitialization with
                member this.Init() = 
                     Configure.Transactions.Enable() |> ignore
                     Configure.Serialization.Json() |> ignore
                     Configure.ScaleOut(fun a-> a.UseSingleBrokerQueue() |> ignore)
                     Configure.Component<IJobFactory>(DependencyLifecycle.InstancePerUnitOfWork) |> ignore
                     Configure.Component<IScheduler>(fun (x:<'a>) -> new Func<'a>(),DependencyLifecycle.InstancePerUnitOfWork) |> ignore
//                                                            let factoryx = new StdSchedulerFactory()
//                                                            factoryx.Initialize()
//                                                            let scheduler = factoryx.GetScheduler()
//                                                            scheduler.JobFactory = Configure.Instance.Builder.Build<IJobFactory>()
//                                                            scheduler
                                                  
                     Configure.With()
                        .DefineEndpointName("lacjam.servicebus")
                        .Log4Net()
                        .AutofacBuilder(Ioc)                   
                        .InMemorySagaPersister()
                        .InMemoryFaultManagement()
                        .InMemorySubscriptionStorage()
                        .UseInMemoryTimeoutPersister()  
                        .UseTransport<Msmq>()
                       // .DoNotCreateQueues()
                        .PurgeOnStartup(true)
                        .UnicastBus() |> ignore
                          
         type ServiceBusStartUp() =              
            interface IWantToRunWhenBusStartsAndStops with
                member this.Start() = 
                    Console.WriteLine("-- Service Bus Started --")          
                    let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                    let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                    let thirty = Convert.ToDouble(30)

//                    let now = System.DateTime.Now
//                    let batch = StartupBatchJobs.Batches
//                    let shed = batch.RunOnSchedule
//                    Schedule.Every(shed).Action(fun a->
//                        try
//                            log.Write(LogMessage.Debug("Another 30 seconds have elapsed."))
//                            
//                            for wl in batch.Jobs do
//                                let cb = bus.Send(wl)
//                                cb.Register(CallBackReceiver) |> ignore                                          
//
//                        with 
//                        | ex ->  log.Write(LogMessage.Error("Schedule ACTION startup:",ex, true)) 
                    System.Net.ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true) //four underscores (and seven years ago?)
                    log.Write(Info("-- Quartz Schedule Started --"))
                    Lacjam.Core.Runtime.Ioc.Resolve<IScheduler>().Start()
//                    StartupBatchJobs.surfReportBatch.Jobs
//                    |> Seq.iter(fun a -> 
//                                        let x = downcast a
//                                        try
//                                            bus.Send(x).Register(CallBackReceiver) |> ignore
//                                        with | ex -> log.Write(Error("Batch failed", ex, true)))

                    let kickOff = Seq.head StartupBatchJobs.surfReportBatch.Jobs
                    try
                       bus.Send(kickOff).Register(
                                                    fun (result:CompletionResult) -> (
                                                                                        try
                                                                                            let msg = (Seq.head result.Messages) :?> Lacjam.Core.Scheduler.Jobs.JobResult
                                                                                            log.Write(LogMessage.Debug("--- Message Received ---"))
                                                                                            log.Write(LogMessage.Debug(msg.Id.ToString()))

                                                                                            StartupBatchJobs.surfReportBatch.Jobs
                                                                                            |> Seq.skipWhile(fun a -> a.Id <> msg.ResultForJobId)
                                                                                            |> Seq.skip 1
                                                                                            |> Seq.head
                                                                                            |> (fun a -> 
                                                                                                        a.Payload <- msg.Result
                                                                                                        bus.Send(a).Register(CallBackReceiver))  |> ignore

                                                                                        with | ex -> log.Write(LogMessage.Warn("Callback failed for " + result.ErrorCode.ToString(), ex))
                                                                  )
                       ) |> ignore
                    with | ex -> log.Write(Error("Batch failed", ex, true))

                member this.Stop() = 
                    let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                    log.Write(Info("-- Quartz Scheduler Stopped --"))
                    Lacjam.Core.Runtime.Ioc.Resolve<IScheduler>().Shutdown(true);
                                       
                    Ioc.Dispose()