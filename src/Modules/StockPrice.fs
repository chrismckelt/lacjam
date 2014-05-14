namespace Lacjam.Modules

module StockPrice =
    open FSharp
    open FSharp.Data
    open HtmlAgilityPack
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain
    open Lacjam.Core.Runtime
    open Microsoft.FSharp
    open Newtonsoft.Json
    open System
    open System.Diagnostics
    open System.Linq
    open System.Net.Mail
    open System.Text
    open log4net

    type Prices =
        { Open : decimal;
          High : decimal;
          Low : decimal;
          Close : decimal }

    type Stocks = FSharp.Data.CsvProvider< "http://ichart.finance.yahoo.com/table.csv?s=MSFT" >

    let url = "http://ichart.finance.yahoo.com/table.csv?s="

    let getStockPrices stock =
        let item = Stocks.Load(url + stock)
        [ for row in item.Rows ->
              { Open = row.Open;
                High = row.High;
                Low = row.Low;
                Close = row.Close } ]

    let getStockQuote who =   getStockPrices (who)
                        