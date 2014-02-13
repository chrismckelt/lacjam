namespace Lacjam.ServiceBus

    open System
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
                                            let createG = Guid.NewGuid
                                            let guidId = createG()
                                            let swJob = CustomJobs.SwellNetRatingJob(Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>())
                                            let log = Ioc.Resolve<ILogWriter>()
                                            log.Write(Debug("StartupBatchJobs init"))

                                          
                                          
                                            swJob.BatchId <- guidId
                                            let swJobs = new Collections.Generic.List<Jobs.JobMessage>()
                                            swJobs.Add(StartUpJob(BatchId=guidId, Payload="SwellNet batch started") :> JobMessage)
                                            swJobs.Add(PageScraperJob(BatchId=guidId, Id=guidId, Url = "http://www.swellnet.com/reports/australia/new-south-wales/cronulla") :> JobMessage)
                                            swJobs.Add(swJob :> JobMessage)
                                            SendTweetJob(To="chris_mckelt") :> JobMessage  |> ignore
                                            //SendEmailJob(Email={To="Chris@mckelt.com";From="Chris@mckelt.com";Subject="SwellNet Rating: {0}";Body="SwellNet Rating: {0}"}) :> JobMessage
                                            
                                                                                        //http://quartz-scheduler.org/documentation/quartz-1.x/tutorials/crontrigger
                                            let ht = TriggerBuilder.Create().StartNow().WithDescription("hourly").WithSimpleSchedule(fun a->a.RepeatForever().WithIntervalInMinutes(15).WithMisfireHandlingInstructionFireNow() |> ignore).Build()             

                                            let surfReportBatch = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.UtcNow; Batch.Id=Guid.NewGuid(); Batch.Name="Surf-Report-Batch";Batch.Jobs=swJobs; Batch.Status=BatchStatus.Waiting;Batch.TriggerName=BatchSchedule.Hourly.ToString();}

                                            let jiraJobs = new Collections.Generic.List<JobMessage>()
                                            jiraJobs.Add(CustomJobs.JiraRoadMapOutputJob())

                                            let jiraRoadmapBatch = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.UtcNow; Batch.Id=Guid.NewGuid(); Batch.Name="Jira-Roadmap";Batch.Jobs=jiraJobs; Batch.Status=BatchStatus.Waiting;Batch.TriggerName=BatchSchedule.Hourly.ToString();}


                                            [surfReportBatch; jiraRoadmapBatch]
