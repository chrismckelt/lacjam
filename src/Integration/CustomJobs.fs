namespace Lacjam.Integration

module CustomJobs =

    open System
    open System.ServiceModel
    open System.Linq
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
     
    [<Serializable>]
    type JiraRoadMapOutputJob() =
        inherit Lacjam.Core.Jobs.JobMessage()
        interface NServiceBus.IMessage


    type JiraRoadMapOutputJobHandler(log : ILogWriter) =
        interface NServiceBus.IHandleMessages<JiraRoadMapOutputJob> with
            member x.Handle(sc) =
                log.Write (LogMessage.Debug(sc.CreatedDate.ToString() + "   " + sc.GetType().ToString()))                    

                try
                    Lacjam.Integration.Jira.outputRoadmap()
                with ex -> log.Write(LogMessage.Error(sc.GetType().ToString(), ex, true)) //Console.WriteLine(html)


    [<Serializable>]
    type SwellNetRatingJob(log:ILogWriter) =
        inherit Lacjam.Core.Jobs.JobMessage()
        do log.Write(Debug("SwellNetRatingJob ctr"))
        interface NServiceBus.IMessage
        interface Quartz.IJob with
            override x.Execute(context) = let fire = Lacjam.Core.Runtime.Ioc.Resolve<IBus>().Send(x).Register(Scheduling.callBackReceiver)
                                          do log.Write(Debug("SwellNetRatingJob fire ------"))
                                          fire |> ignore  
    
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
                    
                    try
                            let mutable cleaned = lastUpdated.Value.OwnerNode.InnerText
                            cleaned <- cleaned.Replace("pm", String.Empty)
                            cleaned <- cleaned.Replace("am", String.Empty)
                            let result = cleaned |> System.Convert.ToDateTime  
                            log.Write(Debug("SwellNetJob parse date on page success : " + result.ToString()))
                            processJob doc result job 
                    with | ex -> 
                           log.Write(Error("SwellNetJob parsing date on page",ex,false))
                           let msg = "Deferring job for execution for 30 minutes: " + job.ToString()
                           deferJob log bus job msg (DateTime.Now.AddMinutes(double 30))
                           

                with ex -> 
                        log.Write(LogMessage.Error(job.GetType().ToString(), ex, true)) //Console.WriteLine(html)
                        let fail = new Jobs.JobResult(job, false, ex.Message)
                        bus.Reply(fail)


