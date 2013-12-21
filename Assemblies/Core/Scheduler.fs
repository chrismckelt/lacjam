namespace Lacjam.Core.Scheduler 

    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain
    open Lacjam.Core.Runtime
    open NServiceBus
    open System
    open System.Collections.Concurrent
    open System.Collections.Generic
    open System.IO
    open System.Net
    open System.Net.Http
    open System.Runtime.Serialization
    open System.Text.RegularExpressions

    /// Fantomas
    /// Ctrl + K D   -- format document
    /// Ctrl + K F   -- format selection / format cursor position


    type Schedule = 
        | EveryFiveMinutes of TimeSpan
        | OncePerDayAtTime of DateTime // time 
        | Custom of (DateTime -> DateTime)

    [<Serializable>]
    type SitePoll() = 
        member val JobName = "SitePoll-GetHTMLFromSite" with get, set
        //member x.Schedule = EveryFiveMinutes
        interface IMessage

    module Jobs = 
        open Lacjam
        open Lacjam.Core
        open Lacjam.Core.Domain
        open Lacjam.Core.Runtime
        open NServiceBus
        open System
        open System.Collections.Concurrent
        open System.Collections.Generic
        open System.IO
        open System.Net
        open System.Net.Http
        open System.Runtime.Serialization
        open System.Text.RegularExpressions
    
        [<AbstractClass>]
        [<Serializable>]
        type JobBase() = 
            member x.Id with get () = Guid.NewGuid
            member x.CreatedDate : DateTime = DateTime.UtcNow 
            abstract member Name : string with get, set
            abstract member Payload : string with get, set
            interface IMessage

        [<Serializable>]    
        type JobResult(resultForJobId : Guid, success : bool, result : string) = 
            let mutable suc = success
            member x.Id with get() = Guid.NewGuid, set
            member x.ResultForJobId with get () = resultForJobId      
            member x.CreatedDate : DateTime = DateTime.UtcNow 
            member x.Success with get()= suc and set(v : bool) = suc <- v
            member val Result = result
            override x.ToString() = 
                String.Format
                    ("{0} {1} {2} {3}", x.Id, x.ResultForJobId, 
                        x.CreatedDate, x.Success.ToString())
            interface IMessage
    
        [<Serializable>]
        type SiteScraper(name:string, url) =
                inherit JobBase() with
                    let mutable ln = name
                    override x.Name with get() = ln and set(v : string) = ln <- v 
                    let mutable ls = url   
                    member x.Url with get() = ls and set(v : string) = ls <- v
                    let mutable payload = String.Empty
                    override x.Payload with get() = payload and set(v : string) = payload <- v  
    
        type JobType =
        | SiteScraperType of SiteScraper
       

        type Batch =  { Id : System.Guid; Name : string;  RunOnSchedule : Schedule; Jobs : seq<JobBase> }
    
    module JobHandlers = 
        open Lacjam
        open Lacjam.Core
        open Lacjam.Core.Domain
        open Lacjam.Core.Runtime
        open Autofac
        open NServiceBus
        open NServiceBus.MessageInterfaces
        open log4net
        open System
        open System.Collections.Concurrent
        open System.Collections.Generic
        open System.IO
        open System.Net
        open System.Net.Http
        open System.Runtime.Serialization
        open System.Text.RegularExpressions
        
        type JobResultHandler(logger : Lacjam.Core.Runtime.ILogWriter) = 
            interface NServiceBus.IHandleMessages<Jobs.JobResult> with
                member x.Handle(jr) = 
                    try 
                        logger.Write(LogMessage.Debug(jr.ToString()))
                    with ex -> logger.Write(LogMessage.Error(jr.ToString(), ex, true))
        
        type SiteScraperHandler(logger : ILogWriter) = 
            interface IHandleMessages<Jobs.SiteScraper> with
                member x.Handle(sc) = 
                    let job = (sc :> Jobs.JobBase)
                    logger.Write
                        (LogMessage.Debug(job.CreatedDate.ToString() + "   " + job.Name))
                    let html =                         
                        match Some(sc.Url) with 
                            | None ->
                                let client = new System.Net.WebClient()
                                let result = client.DownloadString("http://www.bedlam.net.au")
                                result
                            | Some(a) -> 
                                let client = new System.Net.WebClient()
                                let result = client.DownloadString(a)
                                result
                    let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                    try 
                        let jr = Jobs.JobResult(sc.Id(), true, html)
                        bus.Reply(jr)
                    with ex -> logger.Write(LogMessage.Error(job.Name, ex, true)) //Console.WriteLine(html)
                                                                                  
                                                                                  
                                                                              
                                                                              
