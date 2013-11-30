namespace Lacjam
module ServiceBus =
open System
open NServiceBus

    let Bus =  (Configure.With()
                    .DefaultBuilder()
                    .DefineEndpointName("lacjam.worker")
                    .Log4Net()
                    .XmlSerializer()
                    .IsTransactional(true)
                    .PurgeOnStartup(false)
                    .Sagas()
                    .DisableTimeoutManager()
                    .UnicastBus()
                    .CreateBus()
                    .Start() ) 

    type EndpointConfig() =
        interface AsA_Server  
        interface IConfigureThisEndpoint 
        interface IWantCustomInitialization with
            member this.Init() = (Bus) |> ignore
                   
                  

   

