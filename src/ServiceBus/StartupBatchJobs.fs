namespace Lacjam.ServiceBus

module StartupBatchJobs =
    open System
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduling
    open Lacjam.Core.Jobs
    open Lacjam.Integration

    let j1 = PageScraperJob(Payload="http://www.bedlam.net.au") :> JobMessage
    let j2 = PageScraperJob(Payload="http://www.mckelt.com")  :> JobMessage
    let j3 = PageScraperJob(Payload="http://www.mckelt.com/blog") :> JobMessage
    let Batchs = seq [j1; j2; j3;]
       
//    let pingBatches = {
//        Batch.Id = Guid.NewGuid(); 
//        Batch.Name = "site-wakeup" ; 
//        Batch.Jobs = Batchs 
//        Batch.RunOnSchedule =TimeSpan.FromMinutes(Convert.ToDouble(1))
//        }

    let scheduleJiraRoadmapOutput() =
                                let jiraJob = new CustomJobs.JiraRoadMapOutputJob() 
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
    let swJob = CustomJobs.SwellNetRatingJob(Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>())
    swJob.BatchId  <- guidId
    let swJobs = [
                                StartUpJob(BatchId=guidId) :> JobMessage
                                PageScraperJob(BatchId=guidId, Id=guidId, Url = "http://www.swellnet.com/reports/australia/new-south-wales/cronulla") :> JobMessage
                                swJob :> JobMessage
                                SendEmailJob(Email={To="Chris@mckelt.com";From="Chris@mckelt.com";Subject="SwellNet Rating: {0}";Body="SwellNet Rating: {0}"}) :> JobMessage
                 ]

    let surfReportBatch = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.UtcNow; Batch.Id=Guid.NewGuid(); Batch.Name="SwellNet";Batch.Jobs=swJobs; Batch.Status=BatchStatus.Waiting}
    
        