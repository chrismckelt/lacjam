namespace Lacjam.Integration
    open Autofac
    open FSharp
    open FSharp.Data
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
   // open NServiceBus.Mailer
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
    open System.Net
    open System
    open System.Text
    open System.Text.RegularExpressions
    open System.IO


    module AsxCsvDownloader =
        [<Literal>]
        let asxSample = """ "Company name","ASX code","GICS industry group"
                            "1300 SMILES LIMITED",ONT,"Health Care Equipment & Services"
                            "360 CAPITAL GROUP",TGP,"Real Estate"
                            "360 CAPITAL INDUSTRIAL FUND",TIX,"Real Estate"
                            "360 CAPITAL OFFICE FUND",TOF,"Real Estate"
                            "3D OIL LIMITED",TDO,"Energy"
                            "3D RESOURCES LIMITED",DDD,"Materials" """

        [<Literal>]
        let asxFilePath = @"C:\dev\Lacjam\src\Integration\ASXListedCompaniess.csv"

        [<Literal>]
        let downloadUrl = @"http://www.asx.com.au/asx/research/ASXListedCompanies.csv"

        let splitNewLine (x:string) = x.Split([|@"\r\n"; @"\n";Environment.NewLine|], StringSplitOptions.None)

        type Stocks = FSharp.Data.CsvProvider<asxSample>

        let downloadStock (uri:option<string>) = 
                            let wc = new WebClient()
                            let s = match uri with 
                                                | Some(uri) -> uri 
                                                |  _ -> downloadUrl
                            let sb = new StringBuilder()
                            let (lines:string) = wc.DownloadString(s)
                            lines |> splitNewLine  |> Seq.skip 1 |> Seq.iter(fun a ->  sb.AppendLine(a) |> ignore)
                            //Debug.WriteLine(sb.ToString())
                            use ms = new MemoryStream(ASCIIEncoding.Default.GetBytes(sb.ToString()))   
                            let allStock = Stocks.Load(ms)
                            allStock.Rows  
                            |> Seq.iter(fun x -> Debug.WriteLine(x.``ASX code`` + " "+ x.``Company name``))
                            |> ignore

