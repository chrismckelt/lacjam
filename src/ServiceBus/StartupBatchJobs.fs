namespace Lacjam.ServiceBus

    open System
    open System.Linq
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduling
    open Lacjam.Core.Jobs
    open Lacjam.Core.Settings
    open Lacjam.Integration
    open Quartz
    open Quartz.Impl
    open Quartz.Core

    type StartupBatchJobs() =
   
//        let j1 = PageScraperJob(Payload="http://www.bedlam.net.au") :> JobMessage
//        let j2 = PageScraperJob(Payload="http://www.mckelt.com")  :> JobMessage
//        let j3 = PageScraperJob(Payload="http://www.mckelt.com/blog") :> JobMessage
//        let Batchs = seq [j1; j2; j3;]
       
    //    let pingBatches = {
    //        Batch.Id = Guid.NewGuid(); 
    //        Batch.Name = "site-wakeup" ; 
    //        Batch.Jobs = Batchs 
    //        Batch.RunOnSchedule =TimeSpan.FromMinutes(Convert.ToDouble(1))
    //        }

//        let scheduleJiraRoadmapOutput() =
//                                    let jiraJob = new CustomJobs.JiraRoadMapOutputJob() 
//                                    Schedule.Every(TimeSpan.FromMinutes(Convert.ToDouble(3))).Action(fun a->
//                                                                                                try
//                                                                                                    Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>().Write(LogMessage.Debug("Schedule running for JIRA Roadmap Job."))
//                                                                                                    Lacjam.Core.Runtime.Ioc.Resolve<IBus>().Send("lacjam.servicebus", jiraJob :> IMessage) |> ignore
//                                                                                                with 
//                                                                                                | ex ->  Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>().Write(LogMessage.Error("Schedule ACTION startup:",ex, true)) 
//                                    )
//                                    ()
    
        let printTimes (x:ITrigger) =   let log = Ioc.Resolve<ILogWriter>()
                                        let spi = x :?> Spi.IOperableTrigger
                                        let times = TriggerUtils.ComputeFireTimes(spi, null,10)
                                        log.Write(Debug(x.Key.Name))
                                        log.Write(Debug("Next 10 fire times scheduled for..."))
                                        for time in times do
                                                log.Write(Debug(time.ToLocalTime().ToString()))

        interface IContainBatches with
            override this.Batches = 
                                            let log = Ioc.Resolve<ILogWriter>()
                                            let js = Ioc.Resolve<IJobScheduler>();

                                            log.Write(Debug("StartupBatchJobs init"))

                                            // scheduler stats batch
                                            let stats = new Jobs.SchedulerStatsJob()
                                            let statsList = seq<Jobs.JobMessage>([stats])
                                            let statsBatch = {Batch.BatchId=Guid.NewGuid(); Batch.CreatedDate=DateTime.Now; Batch.Id=Guid.NewGuid(); Batch.Name="Scheduler-Stats";Batch.Jobs=(statsList.ToList()); Batch.Status=BatchStatus.Waiting;Batch.TriggerName=Lacjam.Core.BatchSchedule.Hourly.ToString();}
                                            
                                            // swell net scraper batch
                                            let guidId = Guid.NewGuid()
                                            let swJob = CustomJobs.SwellNetRatingJob(Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>())
                                            swJob.BatchId <- guidId
                                            let swList = seq<Jobs.JobMessage>([ StartUpJob(BatchId=guidId, Payload="SwellNet batch started");
                                                                                PageScraperJob(BatchId=guidId, Id=guidId, Url = "http://www.swellnet.com/reports/australia/new-south-wales/cronulla");
                                                                                swJob;
                                                                                SendTweetJob(To="chris_mckelt")                                                             
                                                                               ])
                                            let surfReportBatch = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.Now; Batch.Id=Guid.NewGuid(); Batch.Name="Surf-Report-Batch";Batch.Jobs=swList.ToList(); Batch.Status=BatchStatus.Waiting; Batch.TriggerName=Lacjam.Core.BatchSchedule.Daily.ToString();}

                                            // jira roadmap batch
                                            let jiraList = seq<JobMessage>([CustomJobs.JiraRoadMapOutputJob()])
                                            let jiraRoadmapBatch = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.Now; Batch.Id=Guid.NewGuid(); Batch.Name="Jira-Roadmap";Batch.Jobs=jiraList.ToList(); Batch.Status=BatchStatus.Waiting;Batch.TriggerName=Lacjam.Core.BatchSchedule.Hourly.ToString();}

                                            [statsBatch;jiraRoadmapBatch]
