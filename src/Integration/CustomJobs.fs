namespace Lacjam.Integration

module CustomJobs =

    open System
    open System.ServiceModel
    open System.Linq
    open System.Text
    open System.Runtime.Serialization.Json
    open System.Net.Mail;
    open System.Diagnostics
    open Microsoft.FSharp.Linq
    open Microsoft.FSharp.Data.TypeProviders
    open Newtonsoft.Json
    open HtmlAgilityPack
    open NServiceBus
    open NServiceBus.MessageInterfaces
    open NServiceBus.Mailer
    open log4net
    open Autofac
    open NServiceBus
    open Quartz
    open Quartz.Impl
    open Quartz.Spi
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.User
    open Lacjam.Core.Domain
    open Lacjam.Core.Scheduling
    open Lacjam.Integration
    open Lacjam.Core.Utility.Html
    open Lacjam.Core.Jobs
    
     
    [<Serializable>]
    type JiraRoadMapOutputJob() =
        inherit Lacjam.Core.Jobs.JobMessage()
        interface NServiceBus.IMessage


    type JiraRoadMapOutputJobHandler(log : ILogWriter) =
        do log.Write(Debug("JiraRoadMapOutputJobHandler ctr"))
        interface NServiceBus.IHandleMessages<JiraRoadMapOutputJob> with
            member x.Handle(sc) =
                log.Write (LogMessage.Info(sc.ToString()))
                try
                    Lacjam.Integration.Jira.outputRoadmap()
                with ex ->  log.Write (LogMessage.Info("--- JIRA Job failed ---"))
                            log.Write(LogMessage.Error(sc.GetType().ToString(), ex, true)) //Console.WriteLine(html)


    [<Serializable>]
    type SwellNetRatingJob(log:ILogWriter) =
        inherit Lacjam.Core.Jobs.JobMessage()
        do log.Write(Debug("SwellNetRatingJob ctr"))   

    type SwellNetRatingHandler(log : ILogWriter  ,  bus : IBus) =
        do log.Write (LogMessage.Debug("SwellNetRatingHandler"))
                                                                                                                
        interface NServiceBus.IHandleMessages<SwellNetRatingJob> with
            member x.Handle(job) =                   
                log.Write (LogMessage.Info(job.ToString()))    
                try
                    let doc = new HtmlAgilityPack.HtmlDocument()
                    doc.OptionFixNestedTags<-true;
                    doc.LoadHtml(job.Payload)
                    match doc.ParseErrors with | s -> if (s.Count() > 0) then failwith ("Errors" + doc.ParseErrors.First().ToString()) |> ignore

                    let lastUpdatedSpan = doc.DocumentNode.Descendants().FirstOrDefault(fun d -> d.Attributes.Contains("class") && d.Attributes.Item("class").Value.Contains("views-field views-field-field-surf-report-date") )
                    let lastUpdated = Utility.Html.findNodesByClassName(lastUpdatedSpan, "field-content")  
                    let (ratingSpan:HtmlNode) = doc.DocumentNode.Descendants().FirstOrDefault(fun d -> d.Attributes.Contains("class") && d.Attributes.Item("class").Value.Contains("views-field views-field-field-surf-report-rating") )
                    let rating = findNodesByClassName(ratingSpan, "field-content")
                    match rating with
                    | Some(a) ->    log.Write(Debug("Rating is " + rating.Value.OwnerNode.InnerText))
                                    job.IsCompleted <- true
                                    let jr = new Jobs.JobResult(job, true, rating.Value.OwnerNode.InnerText)
                                    bus.Reply(jr)
                    | None ->       let msg =  ("SwellNetRating cannot parse rating for job : - " + job.ToString())
                                    failwith msg   

                with ex -> 
                        log.Write(LogMessage.Error(job.GetType().ToString(), ex, true)) //Console.WriteLine(html)
                        let fail = new Jobs.JobResult(job, false, ex.Message,TimeSpan.FromMinutes(double 15))
                        bus.Reply(fail)


       type SchedulerStatsJobHandler(log : ILogWriter, bus : IBus) =
            do log.Write(Info("SchedulerStatsJobHandler"))
            interface IHandleMessages<Lacjam.Core.Jobs.SchedulerStatsJob> with
                member x.Handle(job) =
                    log.Write(Info(job.ToString()))
                       
                    try
                        log.Write(LogMessage.Debug("SchedulerStatsJobHandler - getting stats"))
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

                        log.Write(Info(sb.ToString()))  
                        let jr = Jobs.JobResult(job, true, sb.ToString() + " Completed" )
                        bus.Reply(jr)
                    with ex ->
                        log.Write(Error("SchedulerStatsJobHandler -- " + job.ToString(), ex, true)) //Console.WriteLine(html)
                        let fail = Jobs.JobResult(job, false, ex.ToString())
                        bus.Reply(fail)