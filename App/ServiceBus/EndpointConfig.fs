namespace Lacjam.ServiceBus 
    open System
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.Jobs

    module Startup = 

        let CallBackReceiver (result:CompletionResult) = 
                Console.WriteLine("--- CALLBACK ---")
                let msg = (Seq.head result.Messages) :?> SiteRetrieverResult
                let log = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
                try
                    log.Write(LogMessage.Debug("--- Message Received ---"))
                    log.Write(LogMessage.Debug(msg.Html))
                with | ex -> printf "%A" ex
            
        [<Serializable>]
        type TestPoll() = 
            member val JobName = "TestPoll" with get, set
            //member this.Schedule = EveryFiveMinutes
            interface IMessage

        type EndpointConfig() =
            interface IConfigureThisEndpoint
            interface AsA_Server
            interface IWantCustomInitialization with
                member this.Init() = 
                    Configure.ScaleOut(new Action<Settings.ScaleOutSettings>(fun a -> (a.UseSingleBrokerQueue()|>ignore)))
                    Configure.Transactions.Enable() |> ignore
                    Configure.Transactions.Enable() |> ignore
                    Configure.Serialization.Xml() |> ignore
                    Configure.Features.Enable<Sagas>() |> ignore
                    Configure.With()
                        .DefineEndpointName("lacjam.servicebus")
                        .Log4Net()
                        .AutofacBuilder(Ioc)                   
                        .InMemorySagaPersister()
                        .InMemoryFaultManagement()      
                        .UseTransport<Msmq>()
                        .DoNotCreateQueues()
                        .PurgeOnStartup(false)
                        .UnicastBus() |> ignore

         type ServiceBusStartUp() =              
          
            interface IWantToRunWhenBusStartsAndStops with
                member this.Start() = 
                    Console.WriteLine("-- Service Bus Started --")
                    let message = Lacjam.Core.Jobs.BedlamPoll()
                    message.JobName <- "BedlamPoll"                    
                    //let message = new TestPoll()
                    let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                    let cb = bus.Send(message :> IMessage).Register(CallBackReceiver)
                    
                    let d = Convert.ToDouble(30)
                    Schedule.Every(System.TimeSpan.FromSeconds(d)).Action(fun a->
                                                                Console.WriteLine("Another 30 seconds have elapsed.")
                                                                do bus.Send(message :> IMessage) |> ignore
                                                                )
                member this.Stop() = (Console.WriteLine("-- Service Bus Stopped --"))

              
   

