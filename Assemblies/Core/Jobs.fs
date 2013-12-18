namespace Lacjam.Core.Jobs
open System
open System.Runtime.Serialization
open NServiceBus
open System
open System.Net
open System.Net.Http
open System
open System.Collections.Concurrent
open System.Collections.Generic
open System.IO
open System.Net
open System.Text.RegularExpressions

type Schedule = 
    | EveryFiveMinutes of TimeSpan
    | OncePerDay of DateTime

[<Serializable>]
type SiteRetrieverResult() = 
    
    member val Html = "BedlamPoll" with get, set
    //member this.Schedule = EveryFiveMinutes
    interface IMessage

[<Serializable>]
type BedlamPoll() = 
    
    member val JobName = "BedlamPoll" with get, set
    //member this.Schedule = EveryFiveMinutes
    interface IMessage


    type IAmAJob =  
        abstract Name : string with get
        abstract member CreateDate : DateTime with get
        abstract member Url : string  with get
        abstract member Execute : unit -> string with get
        inherit IMessage
    
    type Batch = {Id:System.Guid; Jobs:seq<IAmAJob>}
      
    type SiteScraper(name:string, url:string) =
        interface IAmAJob with
           member this.Name = name 
           member this.CreateDate = DateTime.UtcNow 
           member this.Url = System.String.Empty 
           member this.Execute =
                let client = new System.Net.WebClient()
                let result = client.DownloadString(url)
                result

    type Audit(name:string, url:string) =
        interface IAmAJob with
            member this.Name = name 
            member this.CreateDate = DateTime.UtcNow 
            member this.Url = System.String.Empty 
            member this.Execute =
                let client = new System.Net.WebClient()
                let result = client.DownloadString(url)
                result
   
type JobType =
| SiteScraperType of SiteScraper
| AuditType of Audit        