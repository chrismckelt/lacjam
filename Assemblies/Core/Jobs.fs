namespace Lacjam.Core
open System
open NServiceBus

module Jobs = 
    open System
    open System.Net
    open System.Net.Http
    open System
    open System.Collections.Concurrent
    open System.Collections.Generic
    open System.IO
    open System.Net
    open System.Text.RegularExpressions

    type IAmAJob =  
        abstract Name : string with get
        abstract member CreateDate : DateTime with get
        abstract member Url : string  with get
        abstract member Execute : unit -> string with get
        inherit IMessage
       
    type SiteScraper(name:string, url:string) =
        interface IAmAJob with
           member this.Name = name 
           member this.CreateDate = DateTime.UtcNow 
           member this.Url = System.String.Empty 
           member this.Execute =
                let client = new System.Net.WebClient()
                let result = client.DownloadString(url)
                result
        

    type Batch = {Id:System.Guid; Jobs:seq<IAmAJob>}

type JobNames =
| SiteScraperJob of Jobs.IAmAJob
| AuditJob 
   
                    