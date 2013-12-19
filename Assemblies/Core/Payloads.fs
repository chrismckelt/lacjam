namespace Lacjam.Core.Payloads
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
        | OncePerDayAtTime of DateTime   // time 
        | Custom of (DateTime -> DateTime)

    [<Serializable>]
    type SitePoll() = 
    
        member val JobName = "SitePoll-GetHTMLFromSite" with get, set
        //member this.Schedule = EveryFiveMinutes
        interface IMessage

    [<AbstractClass>]
    type JobBase() =
        member this.Id with get() = Guid.NewGuid  
        member x.CreateDate = DateTime.UtcNow
        abstract Name : string with get
        abstract member Execute : unit -> string with get
        interface IMessage
    
    module Jobs =     

        [<Serializable>]
        type SiteScraperResult() = 
            member val Html = String.Empty with get, set
            //member this.Schedule = EveryFiveMinutes
            interface IMessage

        type SiteScraper(name:string, url:string) =
            inherit JobBase() with
               override this.Name = "SiteScraper" 
               member this.Url = System.String.Empty 
               override this.Execute =
                    let client = new System.Net.WebClient()
                    let result = client.DownloadString(url)
                    result

        type Audit(name:string, url:string) =
            inherit JobBase() with
                override this.Name = "Audit" 
                member this.Url = System.String.Empty 
                override this.Execute =
                    let client = new System.Net.WebClient()
                    let result = client.DownloadString(url)
                    result

//         type JobType =
//            | SiteScraperType of SiteScraper
//            | AuditType of Audit    
 
    type Batch = {Id:System.Guid;Name:string; RunOnSchedule:Schedule; Jobs:seq<JobBase>}
   
   


    