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
    open Lacjam.Integration
    open Quartz
    open Quartz.Spi
    open Quartz.Impl
    open Autofac
    open NServiceBus.ObjectBuilder
    open NServiceBus.ObjectBuilder.Common

    module Startup = 
       

        type EndpointConfig() =
            interface IConfigureThisEndpoint
            interface AsA_Server
            interface IWantCustomInitialization with
                member this.Init() = 
                     Configure.Transactions.Enable() |> ignore
                     Configure.Serialization.Json() |> ignore
                     Configure.ScaleOut(fun a-> a.UseSingleBrokerQueue() |> ignore)
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
         
       type CustomInitialization() =
           let sf = new System.Func<IScheduler>(fun _->  let fac = new StdSchedulerFactory()
                                                         fac.Initialize()
                                                         let scheduler = fac.GetScheduler()
                                                         scheduler.JobFactory <- Configure.Instance.Builder.Build<IJobFactory>()
                                                         scheduler.Start()
                                                         scheduler
                                                         )
           interface IWantCustomInitialization with
                member this.Init() = 
                    // Configure.Instance.Configurer.ConfigureComponent<IJobFactory>(new System.Func<IJobFactory>(fun a-> new QuartzJobFactory(Configure.Instance.Builder):>IJobFactory), DependencyLifecycle.InstancePerUnitOfWork) |> ignore
                   //  Configure.Instance.Configurer.ConfigureComponent<QuartzJobFactory>(new System.Func<QuartzJobFactory>(fun a-> new QuartzJobFactory(Configure.Instance.Builder)), DependencyLifecycle.InstancePerUnitOfWork) |> ignore
                     Configure.Instance.Configurer.ConfigureComponent<IScheduler>(sf, DependencyLifecycle.SingleInstance) |> ignore 


//
//                     Configure.Instance.Configurer.ConfigureComponent<BatchJobs.SwellNetRatingJob>(DependencyLifecycle.InstancePerUnitOfWork)  |> ignore 
//                     Configure.Instance.Configurer.ConfigureComponent<SchedulingBatchJobs.SwellNetRatingJobScheduler>(DependencyLifecycle.InstancePerUnitOfWork)  |> ignore 
                          
       type ServiceBusStartUp() =     
            let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()         
            interface IWantToRunWhenBusStartsAndStops with
                member this.Start() = 
                    log.Write(Info("-- Service Bus Started --"))   
                    System.Net.ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true) //four underscores (and seven years ago?)                       
                    let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                    let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                    let sched = Lacjam.Core.Runtime.Ioc.Resolve<IScheduler>()
                    let con = new ContainerBuilder()
                    con.Register(fun x -> new JobScheduler(log,sched,bus)).As<Scheduling.IJobScheduler>() |> ignore
                    con.Update(Ioc)
                    log.Write(Info("-- Scheduler added --"))   
                    // schedule startup jobs
                    let js = new Scheduling.JobScheduler(log,sched,bus) :> IJobScheduler
                    //http://quartz-scheduler.org/documentation/quartz-1.x/tutorials/crontrigger
                    //let trig = TriggerBuilder.Create().WithSchedule(CronScheduleBuilder.CronSchedule("0 0/5 5 * * ?").WithMisfireHandlingInstructionFireAndProceed()).StartNow()
                    //let trig = TriggerBuilder.Create().WithSimpleSchedule(fun a-> a.WithIntervalInSeconds(30)|>ignore).StartNow().Build()
                    //let trig = TriggerBuilder.Create().WithSchedule(CronScheduleBuilder.CronSchedule("0 0/5 5 * * ?").WithMisfireHandlingInstructionFireAndProceed()).StartNow().Build()
                  
                    let trig = new Quartz.Impl.Triggers.DailyTimeIntervalTriggerImpl()
                    trig.Name <- "trig-daily " + Guid.NewGuid().ToString()
                    trig.StartTimeUtc <- DateTimeOffset.UtcNow
                    trig.StartTimeOfDay <- TimeOfDay.HourMinuteAndSecondOfDay(8, 50, 0)
                    trig.RepeatIntervalUnit <- IntervalUnit.Minute
                    trig.RepeatInterval <- 1
                    trig.RepeatCount <- 10
                    trig.TimeZone <- TimeZoneInfo.Utc
                    
                    let suJobs = new StartupBatchJobs() :> IContainBatches
                    for batch in suJobs.Batches do
                        js.scheduleBatch<ProcessBatch>(batch,trig.GetTriggerBuilder())
                         
                    ()

                member this.Stop() = 
                    Lacjam.Core.Runtime.Ioc.Resolve<IScheduler>().Shutdown(true);    
                    log.Write(Info("-- Scheduler Stopped --"))
                    Ioc.Dispose()

//            interface ISpecifyMessageHandlerOrdering  with
//                member x.SpecifyOrder(order)  = order.Specify(First<NServiceBus.Timeout.TimeoutMessageHandler>.Then<SagaMessageHandler>())
