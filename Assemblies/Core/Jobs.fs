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
        inherit IMessage
        abstract Name : string
        abstract member CreateDate : DateTime
        abstract member Url : string 
        abstract member Execute : unit -> string
    
    type SiteScraper(name:string, url:string) =
        interface IAmAJob with
           member this.Name = name
           member this.CreateDate = DateTime.UtcNow
           member this.Url = System.String.Empty
           member this.Execute() = 
                    let client = new System.Net.WebClient()
                    let result = client.DownloadString(url)
                    result
        

type JobNames =
| SiteScraperJob of Jobs.IAmAJob
| AuditJob 
   
                    