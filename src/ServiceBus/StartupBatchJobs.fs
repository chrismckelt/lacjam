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
                                            let log = Ioc.Resolve<ILogWriter>()
                                            log.Write(Debug("StartupBatchJobs init"))
                                            let guidId = Guid.NewGuid()
                                            let swJob = CustomJobs.SwellNetRatingJob(Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>())
                                            swJob.BatchId <- guidId
                                            let swJobs = new Collections.Generic.List<Jobs.JobMessage>()
                                            swJobs.Add(StartUpJob(BatchId=guidId, Payload="SwellNet batch started") :> JobMessage)
                                            swJobs.Add(PageScraperJob(BatchId=guidId, Id=guidId, Url = "http://www.swellnet.com/reports/australia/new-south-wales/cronulla") :> JobMessage)
                                            swJobs.Add(swJob :> JobMessage)
                                            swJobs.Add(SendTweetJob(To="chris_mckelt") :> JobMessage)  |> ignore
                                            //SendEmailJob(Email={To="Chris@mckelt.com";From="Chris@mckelt.com";Subject="SwellNet Rating: {0}";Body="SwellNet Rating: {0}"}) :> JobMessage
                                            
                                                                                                           
                                            let surfReportBatch = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.UtcNow; Batch.Id=Guid.NewGuid(); Batch.Name="Surf-Report-Batch";Batch.Jobs=swJobs; Batch.Status=BatchStatus.Waiting; Batch.TriggerName="";}

                                            let jiraJobs = new Collections.Generic.List<JobMessage>()
                                            jiraJobs.Add(CustomJobs.JiraRoadMapOutputJob())

                                            let jiraRoadmapBatch = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.UtcNow; Batch.Id=Guid.NewGuid(); Batch.Name="Jira-Roadmap";Batch.Jobs=jiraJobs; Batch.Status=BatchStatus.Waiting;Batch.TriggerName="";}

                                            let sjobDetail = new JobDetailImpl(surfReportBatch.GetType().Name,  surfReportBatch.Name, typedefof<ProcessBatch>)
                                            sjobDetail.Durable <- true
                                            sjobDetail.Name <- surfReportBatch.Name
                                            sjobDetail.RequestsRecovery <- true
                                            sjobDetail.Description <- surfReportBatch.Name + "--" + DateTime.Now.ToString("yyyyMMddHHmmss")
                                            let jjobDetail = new JobDetailImpl(jiraRoadmapBatch.GetType().Name,  jiraRoadmapBatch.Name, typedefof<ProcessBatch>)
                                            jjobDetail.Durable <- true
                                            jjobDetail.Name <- surfReportBatch.Name
                                            jjobDetail.RequestsRecovery <- true
                                            jjobDetail.Description <- surfReportBatch.Name + "--" + DateTime.Now.ToString("yyyyMMddHHmmss")
                                            

                                             //http://quartz-scheduler.org/documentation/quartz-1.x/tutorials/crontrigger
                                            let dt = TriggerBuilder.Create().ForJob(sjobDetail).StartAt(DateBuilder.FutureDate(10,IntervalUnit.Minute)).WithDescription("daily").WithSimpleSchedule(fun a->a.RepeatForever().WithIntervalInMinutes(24).WithMisfireHandlingInstructionFireNow() |> ignore).Build()             
                                            let ht = TriggerBuilder.Create().ForJob(jjobDetail).StartNow().WithDescription("hourly").WithSimpleSchedule(fun a->a.RepeatForever().WithIntervalInMinutes(15).WithMisfireHandlingInstructionFireNow() |> ignore).Build()             
                                            
                                            surfReportBatch.TriggerName <- dt.Key.Name
                                            jiraRoadmapBatch.TriggerName <- ht.Key.Name
                                            
                                            let js = Ioc.Resolve<IJobScheduler>();
                                            
                                         //   js.Scheduler.AddJob(sjobDetail,true) |> ignore
                                            js.Scheduler.AddJob(jjobDetail,true) |> ignore
                                            js.Scheduler.ScheduleJob(ht) |> ignore
                                          //  js.Scheduler.ScheduleJob(dt) |> ignore
                                            [jiraRoadmapBatch]
