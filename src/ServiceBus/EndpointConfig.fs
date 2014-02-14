namespace Lacjam.ServiceBus 
    open System
    open System.IO
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Utility
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduling
    open Lacjam.Core.Jobs
    open Lacjam.Core.Runtime
    open Lacjam.Integration
    open Quartz
    open Quartz.Spi
    open Quartz.Impl
    open Autofac
    open NServiceBus.ObjectBuilder
    open NServiceBus.ObjectBuilder.Common

    module Startup = 
       

        type EndpointConfig() =
         
            
            do 
                (
                    let cb = new ContainerBuilder()
                    cb.Register(fun _ -> 
                                                                let fac = new StdSchedulerFactory()
                                                                fac.Initialize()
                                                                let scheduler = fac.GetScheduler()
                                                                scheduler.ListenerManager.AddSchedulerListener(new Scheduling.JobSchedulerListener(Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>(), Lacjam.Core.Runtime.Ioc.Resolve<IBus>()))
                                                                scheduler.Start()
                                                                scheduler
                                                            ).As<Quartz.IScheduler>().SingleInstance() |> ignore
                    cb.RegisterType<JobScheduler>().As<Scheduling.IJobScheduler>().SingleInstance() |> ignore
                    cb.Update(Ioc)
                )

            interface IConfigureThisEndpoint
            interface AsA_Server
            interface IWantCustomInitialization with
                member this.Init() = 
                     Configure.Transactions.Enable() |> ignore
                     Configure.Serialization.Json() |> ignore
                     Configure.ScaleOut(fun a-> a.UseSingleBrokerQueue() |> ignore) 
                     
                     try
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
                     with | ex -> printfn "%A" ex

         
//       type CustomInitialization() =
//          
//           interface IWantCustomInitialization with
//                member this.Init() = 
//                        Runtime.ContainerBuilder.Register(fun _ -> sf).As<Quartz.IScheduler>().SingleInstance() |> ignore
//                        Runtime.ContainerBuilder.RegisterType<JobScheduler>().As<Scheduling.IJobScheduler>().SingleInstance() |> ignore
//                        Runtime.ContainerBuilder.Update(Ioc)
//                        //Configure.Instance.Configurer.ConfigureComponent<IScheduler>(DependencyLifecycle.SingleInstance) |> ignore 
                          
      
       
       type ServiceBusStartUp() =     
            let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>() 
            let bus = Ioc.Resolve<IBus>() 
            let js = Ioc.Resolve<IJobScheduler>()
            let addJob (batch:Batch) (trg:ITrigger) =           let found = js.Scheduler.GetTrigger(new TriggerKey(trg.Key.Name))
                                                                match found with 
                                                                | null ->   log.Write(Info("Adding trigger"))
                                                                            let job = new JobDetailImpl(batch.Name,  batch.BatchId.ToString(), typedefof<ProcessBatch>,true,true)
                                                                            job.Durable <- true
                                                                            js.Scheduler.AddJob(job,true)
                                                                            js.Scheduler.ScheduleJob(job,trg)  |> ignore
                                                                | _ ->      log.Write(Info("NOT Adding hourly trigger as it already exists"))
                
            interface IWantToRunWhenBusStartsAndStops with
                member this.Start() = 
                    log.Write(Info("-- Service Bus Started --"))   
                    System.Net.ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true) //four underscores (and seven years ago?)                       
                    Configure.Instance.Configurer.ConfigureComponent<Quartz.IScheduler>(DependencyLifecycle.SingleInstance) |> ignore
                    
                    

                    Schedule.Every(TimeSpan.FromMinutes(Convert.ToDouble(5))).Action(fun a->
                        try
                            log.Write(LogMessage.Debug("NSB -- Schedule.Every elapsed"))
                            Lacjam.Integration.Jira.outputRoadmap()                                      
                        with 
                        | ex ->  log.Write(LogMessage.Error("Schedule ACTION startup:",ex, true)) 
                    )

                    let startup = new StartupBatchJobs() :> IContainBatches
                    let surfReportBatch = startup.Batches.Head                    
                    let jiraRoadmapBatch = startup.Batches.Tail.Head
                    let sjobDetail = new JobDetailImpl(surfReportBatch.GetType().Name,  surfReportBatch.Name, typedefof<ProcessBatch>)
                    sjobDetail.Durable <- true
                    sjobDetail.Name <- surfReportBatch.Name
                    sjobDetail.RequestsRecovery <- true
                    sjobDetail.Description <- surfReportBatch.Name + "--" + DateTime.Now.ToString("yyyyMMddHHmmss")
                    let jjobDetail = new JobDetailImpl(jiraRoadmapBatch.GetType().Name,  jiraRoadmapBatch.Name, typedefof<ProcessBatch>)
                    jjobDetail.Durable <- true
                    jjobDetail.Name <- jiraRoadmapBatch.Name
                    jjobDetail.RequestsRecovery <- true
                    jjobDetail.Description <- jiraRoadmapBatch.Name + "--" + DateTime.Now.ToString("yyyyMMddHHmmss")
                    
                    //http://quartz-scheduler.org/documentation/quartz-1.x/tutorials/crontrigger
                    let dt = TriggerBuilder.Create().ForJob(sjobDetail).WithIdentity(Lacjam.Core.BatchSchedule.Daily.ToString()).StartAt(DateBuilder.TodayAt(6,30,00)).WithDescription("daily").WithSimpleSchedule(fun a->a.RepeatForever().WithIntervalInMinutes(24).WithMisfireHandlingInstructionFireNow() |> ignore).Build()             
                    let ht = TriggerBuilder.Create().ForJob(jjobDetail).WithIdentity(Lacjam.Core.BatchSchedule.Hourly.ToString()).StartNow().WithDescription("hourly").WithSimpleSchedule(fun a->a.RepeatForever().WithIntervalInMinutes(15).WithMisfireHandlingInstructionFireNow() |> ignore).Build()             

                    try
                        addJob surfReportBatch dt
                        addJob jiraRoadmapBatch dt
                    with | ex -> log.Write(Error(" addJob surfReportBatch dt", ex,true))

                    if (System.Environment.MachineName.ToLower() = "earth") then
                        if not <| (js.Scheduler.CheckExists(new TriggerKey(surfReportBatch.TriggerName)))  then
                            js.Scheduler.AddJob(sjobDetail,true) |> ignore
                            js.Scheduler.ScheduleJob(dt) |> ignore                                          
                        
                        if not <| (js.Scheduler.CheckExists(new TriggerKey(jiraRoadmapBatch.TriggerName)))  then
                            js.Scheduler.AddJob(jjobDetail,true) |> ignore
                            js.Scheduler.ScheduleJob(ht) |> ignore

                    try
                        // schedule startup jobs
                        
                        log.Write(Info("EndpointConfig.Init :: SchedulerName = " + js.Scheduler.SchedulerName))
                        log.Write(Info("EndpointConfig.Init :: IsStarted = " + js.Scheduler.IsStarted.ToString()))
                        log.Write(Info("EndpointConfig.Init :: SchedulerInstanceId = " + js.Scheduler.SchedulerInstanceId.ToString()))
                        let suJobs = new StartupBatchJobs() :> IContainBatches
                        for batch in suJobs.Batches do
                            let startBatchJob = new BatchSubmitterJob() 
                            startBatchJob.Batch <- batch
                            bus.Send(startBatchJob) |> ignore
                        ()
                        
                    with | ex -> log.Write(Error("ServiceBusStartUp", ex,true))
                   
                    //Schedule.Every(TimeSpan.FromHours(double 1)).Action(fun a-> Lacjam.Integration.Jira.outputRoadmap())

                member this.Stop() = 
                    Lacjam.Core.Runtime.Ioc.Resolve<IScheduler>().Shutdown(true);    
                    log.Write(Info("-- Service Bus Stopped --"))
                    Ioc.Dispose()
                      

//            interface ISpecifyMessageHandlerOrdering  with
//                member x.SpecifyOrder(order)  = order.Specify(First<NServiceBus.Timeout.TimeoutMessageHandler>.Then<SagaMessageHandler>())
