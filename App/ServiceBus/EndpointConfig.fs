namespace Lacjam
open Lacjam.Core
open Lacjam.Core.Jobs
module ServiceBus =
    open System
    open NServiceBus

        let Bus =  (NServiceBus.Configure.With()
                        .DefaultBuilder()
                        .UseTransport<Msmq>()
                        .DefineEndpointName("lacjam.worker")
                        //.DefiningMessagesAs(fun t -> t.Namespace.StartsWith("Lacjam.Core"))
                        .Log4Net()
                        .PurgeOnStartup(false)
                        .DisableTimeoutManager()
                        .UnicastBus())

    
            type EndpointConfig() =
                interface AsA_Server  
                interface IConfigureThisEndpoint 
                interface IWantCustomInitialization with
                    member this.Init() = 
                        Bus
                        |> ignore
                interface IWantToRunWhenBusStartsAndStops with
                    member this.Start() = 
                        Console.WriteLine("-- Service Bus Started --")
                        do Bus.CreateBus().Send(new Lacjam.Core.Messages.BedlamPoll() :> IMessage)
                        |> ignore
                    member this.Stop() = (Console.WriteLine("-- Service Bus Stopped --"))
                   
                  

   

