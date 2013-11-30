namespace Lacjam
module ServiceBus =
    open System
    open NServiceBus

        let Bus =  (NServiceBus.Configure.With()
                        .DefaultBuilder()
                        .UseTransport<Msmq>()
                        .DefineEndpointName("lacjam.worker")
                        .Log4Net()
                        .PurgeOnStartup(false)
                        .DisableTimeoutManager()
                        .UnicastBus())

        type EndpointConfig() =
            interface AsA_Server  
            interface IConfigureThisEndpoint 
            interface IWantCustomInitialization with
                member this.Init() = (Bus) |> ignore
                   
                  

   

