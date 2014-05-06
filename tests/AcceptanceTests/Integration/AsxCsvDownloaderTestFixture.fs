namespace Lacjam.AcceptanceTests
    open Autofac
    open FSharp
    open FSharp.Data
    open HtmlAgilityPack
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain
    //open Lacjam.Core.Jobs
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduling
    open Lacjam.Core.User
    open Lacjam.Core.Utility.Html
    open Microsoft.FSharp
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
    open System.Text
    open log4net
    open Xunit


        module AsxCsvDownloaderTestFixture  =

            [<Fact>]
            let ``ASX stock downloader should get data `` () =  ()
                  //AsxCsvDownloader.downloadStock     
