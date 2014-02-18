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
    
    let deferJob (log:ILogWriter) (bus:IBus) (job:Jobs.JobMessage) (msg:String) (mins:DateTime) =           log.Write (LogMessage.Info(msg))
                                                                                                            log.Write (LogMessage.Info("Defer send again until " + mins.ToLongTimeString()))
                                                                                                            bus.Defer(mins, job).Register(Scheduling.callBackReceiver) |> ignore      

    type SwellNetRatingHandler(log : ILogWriter  ,  bus : IBus) =
        do log.Write (LogMessage.Debug("SwellNetRatingHandler"))
        let processJob (doc:HtmlAgilityPack.HtmlDocument) (dt:DateTime) (job:SwellNetRatingJob) =          if (dt.DayOfWeek = System.DateTime.Now.DayOfWeek) then
                                                                                                                let (ratingSpan:HtmlNode) = doc.DocumentNode.Descendants().FirstOrDefault(fun d -> d.Attributes.Contains("class") && d.Attributes.Item("class").Value.Contains("views-field views-field-field-surf-report-rating") )
                                                                                                                let rating = findNodesByClassName(ratingSpan, "field-content")
                                                                                                                match rating with
                                                                                                                | Some(a) ->    log.Write(Debug("Rating is " + rating.Value.OwnerNode.InnerText))
                                                                                                                                let jr = new Jobs.JobResult(job, true, rating.Value.OwnerNode.InnerText)
                                                                                                                                bus.Reply(jr)
                                                                                                                | None ->   let mins = DateTime.Now.AddMinutes(double 10)
                                                                                                                            let msg =  ("SwellNetRating - incorrect day found on page - " + "Resubmitting job for processing at : " + mins.ToLongTimeString())
                                                                                                                            deferJob log bus job msg mins
                                                                                                            else
                                                                                                                    let mins = DateTime.Now.AddMinutes(double 10)
                                                                                                                    let msg =  ("SwellNetRating - incorrect day found on page - " + "Resubmitting job for processing at : " + mins.ToLongTimeString())
                                                                                                                    deferJob log bus job msg mins
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
                    
                    let mutable cleaned = lastUpdated.Value.OwnerNode.InnerText
                    cleaned <- cleaned.Replace("pm", String.Empty)
                    cleaned <- cleaned.Replace("am", String.Empty)
                    let parsed = cleaned |> System.DateTime.TryParse
                    match parsed with 
                    | (true, dt) ->
                        log.Write(Debug("SwellNetJob parse date on page success : " + dt.ToString()))
                        processJob doc dt job 
                    | (false, noDt) -> 
                        log.Write(Info("SwellNetJob parsing date on page"))
                        let msg = "Deferring job for execution for 30 minutes: " + job.ToString()
                        deferJob log bus job msg (DateTime.Now.AddMinutes(double 30))
                           

                with ex -> 
                        log.Write(LogMessage.Error(job.GetType().ToString(), ex, true)) //Console.WriteLine(html)
                        let fail = new Jobs.JobResult(job, false, ex.Message)
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
                        sb.AppendLine("-- Quartz MetaData --")
                        sb.AppendLine("NumberOfJobsExecuted : " + meta.NumberOfJobsExecuted.ToString())
                        sb.AppendLine("Started : " + meta.Started.ToString())
                        sb.AppendLine("RunningSince : " + meta.RunningSince.ToString())
                        sb.AppendLine("SchedulerInstanceId : " + meta.SchedulerInstanceId.ToString())
                        sb.AppendLine("ThreadPoolSize : " + meta.ThreadPoolSize.ToString())
                        sb.AppendLine("ThreadPoolType : " + meta.ThreadPoolType.ToString())   
                        log.Write(Info(sb.ToString()))  
                        let jr = Jobs.JobResult(job, true, sb.ToString() + " Completed" )
                        bus.Reply(jr)
                    with ex ->
                        log.Write(Error("SchedulerStatsJobHandler -- " + job.ToString(), ex, true)) //Console.WriteLine(html)
                        let fail = Jobs.JobResult(job, false, ex.ToString())
                        bus.Reply(fail)