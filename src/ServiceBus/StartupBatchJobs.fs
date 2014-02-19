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

                                            if (System.Environment.MachineName.ToLower() = "earth") then
                                                [surfReportBatch;statsBatch]
                                            else
                                                [surfReportBatch;statsBatch;jiraRoadmapBatch]
