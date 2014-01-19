open System
open System.Net
open System.Net.Http
open System.Linq
open Lacjam
open Lacjam.Core
open Lacjam.Integration
open HtmlAgilityPack
open System
open System.IO
open Autofac
open NServiceBus
open NServiceBus.Features
open Lacjam.Core
open Lacjam.Core.Utility
open Lacjam.Core.Runtime
open Lacjam.Core.Scheduling
open Lacjam.Core.Jobs
open Lacjam.Integration
open Quartz
open Quartz.Spi
open Quartz.Impl
open Autofac
open NServiceBus.ObjectBuilder
open NServiceBus.ObjectBuilder.Common
    

[<EntryPoint>]
let main argv = 
    printfn "%A" argv    
    do System.Net.ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true) //four underscores (and seven years ago?)

     //views-label views-label-field-surf-report-date
   
    //http://www.swellnet.com/reports/australia/new-south-wales/cronulla   #

    
    let sched = new JobScheduler(Ioc.Resolve<ILogWriter>(), Ioc.Resolve<IScheduler>(),Ioc.Resolve<IBus>()) :> IJobScheduler
    let sss = sched.Scheduler


    let createG = Guid.NewGuid
    let guidId = createG()
    let swJob = CustomJobs.SwellNetRatingJob(Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>())
    swJob.BatchId  <- guidId
    let swJobs = [
                                PageScraperJob(BatchId=guidId, Id=guidId, Payload = "http://www.swellnet.com/reports/australia/new-south-wales/cronulla") :> JobMessage
                                swJob :> JobMessage
                 ]

    let surfReportBatch = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.UtcNow; Batch.Id=Guid.NewGuid(); Batch.Name="SwellNet";Batch.Jobs=swJobs;Batch.Status=BatchStatus.Waiting}

         
    //(s:>IJobScheduler).scheduleJob<FireNServiceBusMessageJob>()
    Console.WriteLine("")

    let jobDetail = new JobDetailImpl(typedefof<CustomJobs.SwellNetRatingJob>.FullName, typedefof<ProcessBatch>)
    
    //sss.DeleteJobs(jobDetail.Key)
   // let trigger = (TriggerBuilder.Create().ForJob(jobDetail)).StartAt(DateTimeOffset.UtcNow).Build()
    let trigger = (Quartz.TriggerBuilder.Create().ForJob(jobDetail)).WithDescription("10-Second-Intervals-Forever").WithSimpleSchedule(fun a-> a.WithIntervalInSeconds((10)).RepeatForever().Build() |> ignore).StartNow().Build()
//    sss.DeleteJob(jobDetail.Key)
//    
//    sss.ScheduleJob(jobDetail,trigger)
//  
//
//    sss.TriggerJob(jobDetail.Key)

    let mutable nextFireTimeUtc = trigger.GetNextFireTimeUtc()
    let mutable nice = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTimeUtc.Value.DateTime)


 
//    
//    while not <| (Console.KeyAvailable) do
//             let oo = sss.GetTrigger(trigger.Key)
//             nextFireTimeUtc = oo.GetNextFireTimeUtc()
//             nice = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTimeUtc.Value.DateTime)
//             Console.WriteLine(nice)

    Console.ReadLine()  |> ignore
    0 // return an integer exit code