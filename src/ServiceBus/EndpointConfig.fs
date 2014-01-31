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
                    let suJobs = new StartupBatchJobs() :> IContainBatches
                    for batch in suJobs.Batches do
                        js.scheduleBatch<ProcessBatch>(batch)   
                    ()

                member this.Stop() = 
                    Lacjam.Core.Runtime.Ioc.Resolve<IScheduler>().Shutdown(true);    
                    log.Write(Info("-- Scheduler Stopped --"))
                    Ioc.Dispose()

//            interface ISpecifyMessageHandlerOrdering  with
//                member x.SpecifyOrder(order)  = order.Specify(First<NServiceBus.Timeout.TimeoutMessageHandler>.Then<SagaMessageHandler>())
