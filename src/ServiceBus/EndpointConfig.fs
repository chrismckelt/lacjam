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
           let sf = new System.Func<IScheduler>(fun _->  let factoryx = new StdSchedulerFactory()
                                                         factoryx.Initialize()
                                                         let scheduler = factoryx.GetScheduler()
                                                         scheduler.JobFactory <- Configure.Instance.Builder.Build<IJobFactory>()
                                                         scheduler)
           interface IWantCustomInitialization with
                member this.Init() = 
                     Configure.Instance.Configurer.ConfigureComponent<IJobFactory>(new System.Func<IJobFactory>(fun a-> new QuartzJobFactory(Configure.Instance.Builder):>IJobFactory), DependencyLifecycle.InstancePerUnitOfWork) |> ignore
                     Configure.Instance.Configurer.ConfigureComponent<IScheduler>(sf, DependencyLifecycle.SingleInstance) |> ignore 
                     Configure.Instance.Configurer.ConfigureComponent<Jobs.SwellNetRatingJob>(DependencyLifecycle.InstancePerUnitOfWork)  |> ignore 
                          
       type ServiceBusStartUp() =              
            interface IWantToRunWhenBusStartsAndStops with
                member this.Start() = 
                    Console.WriteLine("-- Service Bus Started --")   
                    System.Net.ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true) //four underscores (and seven years ago?)
                           
                    let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                    let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                    let thirty = Convert.ToDouble(30)
                    

                    log.Write(Info("-- Quartz Schedule Started --"))
                    Lacjam.Core.Runtime.Ioc.Resolve<IScheduler>().Start()
                    

                    let kickOff = (StartupBatchJobs.surfReportBatch)
                    let ij = { new IJob with member x.Execute(ctx)=
                    
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
                                                                                                                                                            bus.Send(a).Register(Scheduler.callBackReceiver))  |> ignore

                                                                                                                                            with | ex -> log.Write(LogMessage.Warn("Callback failed for " + result.ErrorCode.ToString(), ex))
                                                                                                                      )
                                                                           ) |> ignore
                                                                        with | ex -> log.Write(Error("Batch failed", ex, true))
                                                                    }
              
                    ij  |> ignore

                member this.Stop() = 
                    let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                    log.Write(Info("-- Quartz Scheduler Stopped --"))
                    Lacjam.Core.Runtime.Ioc.Resolve<IScheduler>().Shutdown(true);    
                    Ioc.Dispose()

//            interface ISpecifyMessageHandlerOrdering  with
//                member x.SpecifyOrder(order)  = order.Specify(First<NServiceBus.Timeout.TimeoutMessageHandler>.Then<SagaMessageHandler>())
