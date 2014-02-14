open Autofac
open HtmlAgilityPack
open Lacjam
open Lacjam.Core
open Lacjam.Core.Jobs
open Lacjam.Core.Runtime
open Lacjam.Core.Scheduling
open Lacjam.Core.Settings
open Lacjam.Core.Utility
open Lacjam.Integration
open NServiceBus
open NServiceBus
open NServiceBus.Features
open NServiceBus.Features
open NServiceBus.ObjectBuilder
open NServiceBus.ObjectBuilder.Common
open Quartz
open Quartz.Impl
open Quartz.Spi
open System
open System.IO
open System.Linq
open System.Net
open System.Net.Http

let configureBus =   

                     do 
                     (
                            let cb = new ContainerBuilder()
                            cb.Register(fun _ -> 
                                                                        let fac = new StdSchedulerFactory()
                                                                        fac.Initialize()
                                                                        let scheduler = fac.GetScheduler()
                                                                        scheduler.ListenerManager.AddSchedulerListener(new Scheduling.JobSchedulerListener(Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>(), Lacjam.Core.Runtime.Ioc.Resolve<IBus>()))
                                                                        scheduler.Start()
                                                                        scheduler
                                                                    ).As<Quartz.IScheduler>().SingleInstance() |> ignore
                            cb.RegisterType<JobScheduler>().As<Scheduling.IJobScheduler>().SingleInstance() |> ignore
                            cb.Update(Ioc)
                     )
                     Configure.Transactions.Enable() |> ignore
                     Configure.Serialization.Json() |> ignore
                     Configure.ScaleOut(fun a-> a.UseSingleBrokerQueue() |> ignore) 
                     
                     try
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
                            .UnicastBus() 
                            .CreateBus()
                            .Start()
                            |> ignore
                     with | ex -> printfn "%A" ex    

[<EntryPoint>]
let main argv = 
    printfn "%A" argv    
    do System.Net.ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true) //four underscores (and seven years ago?)
    configureBus
    let guidId = Guid.NewGuid()
    let swJob = CustomJobs.SwellNetRatingJob(Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>())
    swJob.BatchId <- guidId
    let swJobs = new Collections.Generic.List<Jobs.JobMessage>()
    swJobs.Add(StartUpJob(BatchId=guidId, Payload="SwellNet batch started") :> JobMessage)
    swJobs.Add(PageScraperJob(BatchId=guidId, Id=guidId, Url = "http://www.swellnet.com/reports/australia/new-south-wales/cronulla") :> JobMessage)
    swJobs.Add(swJob :> JobMessage)
    SendTweetJob(To="chris_mckelt") :> JobMessage  |> ignore
    //SendEmailJob(Email={To="Chris@mckelt.com";From="Chris@mckelt.com";Subject="SwellNet Rating: {0}";Body="SwellNet Rating: {0}"}) :> JobMessage
    
    let startup = new Lacjam.ServiceBus.StartupBatchJobs() :> IContainBatches                                       
                                                                                                           
    let js = new JobScheduler(Ioc.Resolve<ILogWriter>(),Ioc.Resolve<IBus>(),Ioc.Resolve<IScheduler>())  :> IJobScheduler
    js.processBatch(startup.Batches.Head)
    Console.ReadLine()  |> ignore
    0 // return an integer exit code