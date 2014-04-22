namespace Lacjam.Integration

module StockPrice =
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
    open Microsoft.FSharp
 
    open NServiceBus
    open NServiceBus.Mailer
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
    open FSharp
    open FSharp.Data


    type Prices = { Open: decimal; High: decimal; Low: decimal; Close: decimal }

    type Stocks = FSharp.Data.CsvProvider<"http://ichart.finance.yahoo.com/table.csv?s=MSFT">

    let url = "http://ichart.finance.yahoo.com/table.csv?s="

    let getStockPrices stock =
                                let item = Stocks.Load(url + stock)
                                [ for row in item.Rows->
                                                        { Open=row.Open; High=row.High; Low=row.Low; Close=row.Close } ]

    [<Serializable>]
    type Job() =
        inherit Lacjam.Core.Jobs.JobMessage()

        member val StockSymbol = "" with get,set
        member val AlertIfPriceOver = 0m with get,set
        

    type Handler(log : ILogWriter, bus : NServiceBus.IBus) =
        do log.Write(LogMessage.Debug("StockPriceJobHandler"))
        interface NServiceBus.IHandleMessages<Job> with
            member x.Handle(job) =  log.Write(LogMessage.Info(job.ToString()))
                                    let result = getStockPrices(job.StockSymbol)
                                    let item = result.Last();
                                    let str = String.Format("Open:{0} High:{1} Low:{2} Close:{3}",item.Open,item.High,item.Low,item.High)
                                    log.Debug(job.StockSymbol)
                                    log.Debug(str)
                                    if (item.High >= job.AlertIfPriceOver) then
                                        let jr = new Jobs.JobResult(job, true, str)
                                        bus.Reply(jr)
                
                //bus.Reply(new Jobs.JobReslt())
                    