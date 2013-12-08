namespace Lacjam.ServiceBus 
    open System
    open NServiceBus
    open Lacjam.Core
    open Lacjam.Core.Jobs
    module Startup = 

        NServiceBus.Configure.ScaleOut(new Action<Settings.ScaleOutSettings>(fun a -> (a.UseSingleBrokerQueue()|>ignore)))
        NServiceBus.Configure.Transactions.Enable() |> ignore

        let Bus = NServiceBus.Configure.With()
                        .Log4Net()
                        .DefaultBuilder()                        
                        .InMemorySagaPersister()
                        .InMemoryFaultManagement()      
                        .UseTransport<Msmq>()
                        .DefineEndpointName("lacjam")
                        .UnicastBus()
                        .LoadMessageHandlers()
                        .DoNotCreateQueues()
                        .PurgeOnStartup(false)
                        .CreateBus()
                        .Start(fun _ -> Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install() |> ignore)


        [<Serializable>]
        type TestPoll() = 
            member val JobName = "TestPoll" with get, set
            //member this.Schedule = EveryFiveMinutes
            interface IMessage

        type EndpointConfig() =
            interface IConfigureThisEndpoint
            interface AsA_Server


         type ServiceBusStartUp() =              
            interface IWantToRunWhenBusStartsAndStops with
                member this.Start() = 
                    Console.WriteLine("-- Service Bus Started --")
                    //let message = new Lacjam.Core.Messages.BedlamPoll()
                    let message = new TestPoll()
                    do Bus.Send(message :> IMessage) |> ignore
                    let d = Convert.ToDouble(30)
                    Schedule.Every(System.TimeSpan.FromSeconds(d)).Action(fun a->Console.WriteLine("Another 30 seconds have elapsed."))
                member this.Stop() = (Console.WriteLine("-- Service Bus Stopped --"))
   

