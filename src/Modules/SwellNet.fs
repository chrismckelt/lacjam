namespace Lacjam.Integration

module SwellNet =
    open HtmlAgilityPack
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain
    open Lacjam.Core.Runtime
    open System
    open System.Diagnostics
    open System.Linq
    open System.Net.Mail
    open System.Text
    open log4net

    let parseHtml html (log : ILogWriter) =
        try
            let doc = new HtmlAgilityPack.HtmlDocument()
            doc.OptionFixNestedTags <- true
            doc.LoadHtml(html)
            match doc.ParseErrors with
            | s ->
                if (s.Count() > 0) then failwith ("Errors" + doc.ParseErrors.First().ToString()) |> ignore
            let lastUpdatedSpan =
                doc.DocumentNode.Descendants()
                   .FirstOrDefault(fun d ->
                   d.Attributes.Contains("class")
                   && d.Attributes.Item("class").Value.Contains("views-field views-field-field-surf-report-date"))
            let lastUpdated = Lacjam.Core.Utility.Html.findNodesByClassName (lastUpdatedSpan, "field-content")
            let (ratingSpan : HtmlNode) =
                doc.DocumentNode.Descendants()
                   .FirstOrDefault(fun d ->
                   d.Attributes.Contains("class")
                   && d.Attributes.Item("class").Value.Contains("views-field views-field-field-surf-report-rating"))
            let rating = Utility.Html.findNodesByClassName (ratingSpan, "field-content")
            match rating with
            | Some(a) ->
                log.Write(LogMessage.Debug("Rating is " + rating.Value.OwnerNode.InnerText))
                rating.Value.OwnerNode.InnerText
            | _ -> failwith "SwellNet scrape failed"
        with ex ->
            log.Write(LogMessage.Error(html, ex, true)) //Console.WriteLine(html)
            raise ex