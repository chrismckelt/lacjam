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
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain
    open Lacjam.Core.Runtime

        type Schedule = 
            | EveryFiveMinutes of TimeSpan
            | OncePerDayAtTime of DateTime   // time 
            | Custom of (DateTime -> DateTime)

        [<Serializable>]
        type SitePoll() = 
    
            member val JobName = "SitePoll-GetHTMLFromSite" with get, set
            //member this.Schedule = EveryFiveMinutes
            interface IMessage

    
        module Jobs =     
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
            open Lacjam
            open Lacjam.Core
            open Lacjam.Core.Domain
            open Lacjam.Core.Runtime

            [<AbstractClass>]
            type JobBase() =
                member this.Id with get() = Guid.NewGuid  
                member x.CreateDate = DateTime.UtcNow
                abstract Name : string with get
                abstract member Execute : unit -> string with get
                interface IMessage

    
            type JobResult(resultForJobId, success, result:string) =
                member this.Id with get() = Guid.NewGuid  
                member this.ResultForJobId with get() = resultForJobId
                member this.CreateDate with get() = DateTime.UtcNow
                member val Success = success 
                member val Result = result
                override this.ToString() = String.Format("{0} {1} {2} {3}", this.Id, this.ResultForJobId, this.CreateDate, this.Success.ToString())
                interface IMessage

            type SiteScraper(name:string, url:string) =
                inherit JobBase() with
                   override this.Name with get() = name
                   member this.Url with get() = url
                   override this.Execute =
                        let client = new System.Net.WebClient()
                        let result = client.DownloadString(url)
                        result
                    

    //         type JobType =
    //            | SiteScraperType of SiteScraper
    //            | AuditType of Audit    
 
            type Batch = {Id:System.Guid;Name:string; RunOnSchedule:Schedule; Jobs:seq<JobBase>}
   
   


    