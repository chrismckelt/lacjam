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

        interface IContainBatches with
            override this.Batches = 
                                            let log = Ioc.Resolve<ILogWriter>()
                                            let js = Ioc.Resolve<IJobScheduler>();

                                            log.Write(Debug("StartupBatchJobs init"))

                                            // scheduler stats batch
                                            let stats = new Jobs.SchedulerStatsJob()
                                            let statsList = seq<Jobs.JobMessage>([stats])
                                            let statsBatch = {Batch.BatchId=Guid.NewGuid(); Batch.CreatedDate=DateTime.Now; Batch.Id=Guid.NewGuid(); Batch.Name="Scheduler-Stats";Batch.Jobs=(statsList.ToList()); Batch.Status=BatchStatus.Waiting;Batch.TriggerName=Lacjam.Core.BatchSchedule.Daily.ToString();}
                                            
                                            // swell net scraper batch
                                            //let guidId = Guid.NewGuid()
//                                            let swJob = SwellNet.Job(Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>())
//                                            swJob.Id = Guid.NewGuid() |> ignore
//                                            swJob.BatchId <- guidId
//                                            let swList = seq<Jobs.JobMessage>([ StartUpJob(BatchId=guidId, Payload="SwellNet batch started");
//                                                                                PageScraperJob(BatchId=guidId, Id=Guid.NewGuid(), Url = "http://www.swellnet.com/reports/australia/western-australia/perth");
//                                                                                swJob;
//                                                                                SendTweetJob(To="chris_mckelt")                                                             
//                                                                                //SendEmailJob(Email={Email.To="hello@smsfinder.com.au";Email.From="hello@smsfinder.com.au";Email.Subject="----";Email.Body="----"})
//                                                                               ])
//                                            let surfReportBatch = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.Now; Batch.Id=Guid.NewGuid(); Batch.Name="Surf-Report-Batch";Batch.Jobs=swList.ToList(); Batch.Status=BatchStatus.Waiting; Batch.TriggerName=Lacjam.Core.BatchSchedule.Daily.ToString();}
//
                                            let stock = new StockPrice.Job()
                                            stock.AlertIfPriceOver <- 34m
                                            stock.StockSymbol <- "WBC"      
                                            
                                            let stockTweet = SendTweetJob(To="chris_mckelt")                                                                                                                                                

                                            let stockBatch =  {Batch.BatchId=Guid.NewGuid(); Batch.CreatedDate=DateTime.Now; Batch.Id=Guid.NewGuid(); Batch.Name="stockBatch-Price-Batch";Batch.Jobs=(seq<Jobs.JobMessage>([stock;stockTweet]).ToList()); Batch.Status=BatchStatus.Waiting; Batch.TriggerName=Lacjam.Core.BatchSchedule.Daily.ToString();}


                                            // jira roadmap batch
//                                            let jiraList = seq<JobMessage>([CustomJobs.JiraRoadMapOutputJob()])
//                                            let jiraRoadmapBatch = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.Now; Batch.Id=Guid.NewGuid(); Batch.Name="Jira-Roadmap";Batch.Jobs=jiraList.ToList(); Batch.Status=BatchStatus.Waiting;Batch.TriggerName=Lacjam.Core.BatchSchedule.Daily.ToString();}

//                                            if (System.Environment.MachineName.ToLower() = "earth") then
//                                                [surfReportBatch;statsBatch]
//                                            else
//                                                [surfReportBatch;statsBatch;jiraRoadmapBatch]
                                            [statsBatch;stockBatch]