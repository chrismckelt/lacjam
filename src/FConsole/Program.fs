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
open System.Text

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
    
    let js = Ioc.Resolve<IJobScheduler>()
    let sb = new StringBuilder()
    let meta = js.Scheduler.GetMetaData()
    sb.AppendLine("-- Quartz MetaData --")  |> ignore
    sb.AppendLine("NumberOfJobsExecuted : " + meta.NumberOfJobsExecuted.ToString()) |> ignore
    sb.AppendLine("Started : " + meta.Started.ToString()) |> ignore
    sb.AppendLine("RunningSince : " + meta.RunningSince.ToString())    |> ignore
    sb.AppendLine("SchedulerInstanceId : " + meta.SchedulerInstanceId.ToString())  |> ignore
    sb.AppendLine("ThreadPoolSize : " + meta.ThreadPoolSize.ToString()) |> ignore
    sb.AppendLine("InStandbyMode : " + meta.InStandbyMode.ToString())  |> ignore 
    sb.AppendLine("InStandbyMode : " + meta.ToString())  |> ignore 
    sb.AppendLine("GetCurrentlyExecutingJobs ")  |> ignore 
    let jobs = js.Scheduler.GetCurrentlyExecutingJobs()
    for job in jobs do
        sb.AppendLine(job.JobDetail.Key.Name)    |> ignore 

    let jobGroupsNames = js.Scheduler.GetJobGroupNames()
    for jobGroupName in jobGroupsNames do
        let groupMatcher = Quartz.Impl.Matchers.GroupMatcher<JobKey>.GroupContains(jobGroupName)
        let keys = js.Scheduler.GetJobKeys(groupMatcher) 
        for jobKey in keys do
            let detail = js.Scheduler.GetJobDetail(jobKey);
            let triggers = js.Scheduler.GetTriggersOfJob(jobKey);
            sb.AppendLine(jobKey.Name) |> ignore 
            sb.AppendLine(detail.Description)   |> ignore 
            for trig in triggers do
                sb.AppendLine(trig.Key.Group + " " + trig.Key.Name)   |> ignore 
                let nextFireTime = trig.GetNextFireTimeUtc()
                if (nextFireTime.HasValue) then
                    sb.AppendLine(nextFireTime.Value.LocalDateTime.ToString()) |> ignore

    Console.WriteLine(sb.ToString())


    Console.ReadLine()  |> ignore
    0 // return an integer exit code