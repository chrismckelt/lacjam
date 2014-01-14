module StartupBatchJobs
    open System
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduler
    open Lacjam.Core.Scheduler.Jobs
    open Lacjam.Integration

    let j1 = PageScraperJob(Payload="http://www.bedlam.net.au") :> JobPayload
    let j2 = PageScraperJob(Payload="http://www.mckelt.com")  :> JobPayload
    let j3 = PageScraperJob(Payload="http://www.mckelt.com/blog") :> JobPayload
    let batchJobs = seq [j1; j2; j3;]
       
    let pingBatches = {
        Batch.Id = Guid.NewGuid(); 
        Batch.Name = "site-wakeup" ; 
        Batch.Jobs = batchJobs 
        Batch.RunOnSchedule =TimeSpan.FromMinutes(Convert.ToDouble(1))
        }

    let scheduleJiraRoadmapOutput() =
                                let jiraJob = new Jobs.JiraRoadMapOutputJob() 
                                Schedule.Every(TimeSpan.FromMinutes(Convert.ToDouble(3))).Action(fun a->
                                                                                            try
                                                                                                Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>().Write(LogMessage.Debug("Schedule running for JIRA Roadmap Job."))
                                                                                                Lacjam.Core.Runtime.Ioc.Resolve<IBus>().Send("lacjam.servicebus", jiraJob :> IMessage) |> ignore
                                                                                            with 
                                                                                            | ex ->  Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>().Write(LogMessage.Error("Schedule ACTION startup:",ex, true)) 
                                )
                                ()
    let createG = Guid.NewGuid
    let guidId = createG()
    let surfReportBatch = {
        Batch.Id = guidId; 
        Batch.Name = "surf-report" ; 
        Batch.Jobs = seq    [|
                                new PageScraperJob(BatchId=guidId, Id=guidId, Payload = "http://www.swellnet.com/reports/australia/new-south-wales/cronulla")
                                new Jobs.SwellNetRatingJob(BatchId=guidId,Id=guidId)
                            |] 
        Batch.RunOnSchedule =TimeSpan.FromMinutes(Convert.ToDouble(1))
    }    