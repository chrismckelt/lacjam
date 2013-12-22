namespace Lacjam.ServiceBus 
    open System
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduler
    open Lacjam.Core.Scheduler.Jobs

    module Startup = 
        open NServiceBus
        open NServiceBus.Features
        open Lacjam.Core
        open Lacjam.Core.Runtime
        open Lacjam.Core.Scheduler
        open Lacjam.Core.Scheduler.Jobs

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
                     Configure.With()
                        .DefineEndpointName("lacjam.servicebus")
                        .Log4Net()
                        .AutofacBuilder(Ioc)                   
                        .InMemorySagaPersister()
                        .InMemoryFaultManagement()
                        .InMemorySubscriptionStorage()
                        .UseInMemoryTimeoutPersister()  
                        .UseTransport<Msmq>()
                        .DoNotCreateQueues()
                        .PurgeOnStartup(false)
                        .UnicastBus() |> ignore
                          
         type ServiceBusStartUp() =              
            interface IWantToRunWhenBusStartsAndStops with
                member this.Start() = 
                    let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                    Console.WriteLine("-- Service Bus Started --")
                    
                    let message = new SiteScraper("Bedlam", ("http://www.bedlam.net.au"))                 
                    let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                    //let cb = bus.Send(message).Register(CallBackReceiver)
                    let thirty = Convert.ToDouble(30)
                    Schedule.Every(System.TimeSpan.FromSeconds(thirty)).Action(fun a->
                        try
                            log.Write(LogMessage.Debug("Another 30 seconds have elapsed."))
                            let cb = bus.Send(message:>IMessage).Register(CallBackReceiver)
                            cb.Start()
                        with 
                        | ex ->  log.Write(LogMessage.Error("Schedule ACTION startup:",ex, true)) 
                     ) 
                    
                    Console.WriteLine("-- Schedule Started --")

                member this.Stop() = 
                    (Console.WriteLine("-- Service Bus Stopped --"))                    
                    Ioc.Dispose()