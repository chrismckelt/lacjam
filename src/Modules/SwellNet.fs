namespace Lacjam.Integration

module SwellNet =
    open Autofac
    open HtmlAgilityPack
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain
    open Lacjam.Core.Jobs
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduling
    open Lacjam.Core.User
    open Lacjam.Core.Utility.Html
    open Lacjam.Integration
    open NServiceBus
    //open NServiceBus.Mailer
    open NServiceBus.MessageInterfaces
    open Newtonsoft.Json
    open Quartz
    open Quartz.Impl
    open Quartz.Spi
    open System
    open System.Diagnostics
    open System.Linq
    open System.Net.Mail
    open System.Runtime.Serialization.Json
    open System.ServiceModel
    open System.Text
    open log4net

    [<Serializable>]
    type Job(log : ILogWriter) =
        inherit Lacjam.Core.Jobs.JobMessage()
        do log.Write(LogMessage.Debug("SwellNetRatingJob ctr"))

    type Handler(log : ILogWriter, bus : IBus) =
        do log.Write(LogMessage.Debug("SwellNetRatingHandler"))
        interface NServiceBus.IHandleMessages<Job> with
            member x.Handle(job) =
                log.Write(LogMessage.Info(job.ToString()))
                try
                    let doc = new HtmlAgilityPack.HtmlDocument()
                    doc.OptionFixNestedTags <- true
                    doc.LoadHtml(job.Payload)
                    match doc.ParseErrors with
                    | s ->
                        if (s.Count() > 0) then failwith ("Errors" + doc.ParseErrors.First().ToString()) |> ignore
                    let lastUpdatedSpan =
                        doc.DocumentNode.Descendants()
                           .FirstOrDefault(fun d ->
                           d.Attributes.Contains("class")
                           && d.Attributes.Item("class")
                               .Value.Contains("views-field views-field-field-surf-report-date"))
                    let lastUpdated = Utility.Html.findNodesByClassName (lastUpdatedSpan, "field-content")
                    let (ratingSpan : HtmlNode) =
                        doc.DocumentNode.Descendants()
                           .FirstOrDefault(fun d ->
                           d.Attributes.Contains("class")
                           && d.Attributes.Item("class")
                               .Value.Contains("views-field views-field-field-surf-report-rating"))
                    let rating = findNodesByClassName (ratingSpan, "field-content")
                    match rating with
                    | Some(a) ->
                        log.Write(LogMessage.Debug("Rating is " + rating.Value.OwnerNode.InnerText))
                        job.IsCompleted <- true
                        let jr = new Jobs.JobResult(job, true, rating.Value.OwnerNode.InnerText)
                        bus.Reply(jr)
                    | None ->
                        let msg = ("SwellNetRating cannot parse rating for job : - " + job.ToString())
                        failwith msg
                with ex ->
                    log.Write(LogMessage.Error(job.GetType().ToString(), ex, true)) //Console.WriteLine(html)
                    let fail = new Jobs.JobResult(job, false, ex.Message, TimeSpan.FromMinutes(double 15))
                    bus.Reply(fail)