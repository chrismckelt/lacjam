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

    module Startup = 

        let CallBackReceiver (result:CompletionResult) = 
                Console.WriteLine("--- CALLBACK ---")
                let msg = (Seq.head result.Messages) :?> Lacjam.Core.Scheduler.Jobs.JobResult
                let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                try
                    log.Write(LogMessage.Debug("--- Message Received ---"))
                    log.Write(LogMessage.Debug(msg.Id.ToString()))
                with | ex -> log.Write(LogMessage.Warn("Callback failed for " + result.ErrorCode.ToString(), ex))

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
                    log.Write(Info("-- Schedule Started --"))

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
                    (Console.WriteLine("-- Service Bus Stopped --"))                    
                    Ioc.Dispose()