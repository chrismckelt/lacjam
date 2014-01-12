namespace Lacjam.Core

module Scheduler =
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

        type JobType =
            | None
            | PageScrape
            | Execute
            | Audit
            | Email
            | Tweet

        [<Serializable>]
        type Job() =
            member val Id = Guid.NewGuid with get
            member val BatchId = Guid.NewGuid with get, set
            member val CreatedDate = DateTime.UtcNow with get
            member val NextJobType = JobType.Audit with get, set
            member val Payload = "" with get, set
            member val Status = false with get, set
            interface IMessage

        [<Serializable>]
        type JobResult(resultForJobId : Guid, success : bool, result : string) =
            let mutable suc = success
            member x.Id with get () = Guid.NewGuid, set
            member x.ResultForJobId with get () = resultForJobId
            member x.CreatedDate : DateTime = DateTime.UtcNow

            member x.Success
                with get () = suc
                and set (v : bool) = suc <- v

            member val Result = result
            override x.ToString() =
                String.Format
                    ("{0} {1} {2} {3}", x.Id, x.ResultForJobId, x.CreatedDate,
                     x.Success.ToString())
            interface IMessage

        [<Serializable>]
        type PageScraperJob() =
            inherit Job()
            interface IMessage

     type Batch =
        { Id : System.Guid
          Name : string
          RunOnSchedule : TimeSpan
          Jobs : seq<Jobs.Job> }

    module JobHandlers =
        open Autofac
        open Lacjam
        open Lacjam.Core
        open Lacjam.Core.Domain
        open Lacjam.Core.Runtime
        open NServiceBus
        open NServiceBus.MessageInterfaces
        open System
        open System.Collections.Concurrent
        open System.Collections.Generic
        open System.IO
        open System.Net
        open System.Net.Http
        open System.Runtime.Serialization
        open System.Text.RegularExpressions
        open log4net

        type JobResultHandler(logger : Lacjam.Core.Runtime.ILogWriter) =
            interface NServiceBus.IHandleMessages<Jobs.JobResult> with
                member x.Handle(jr) =
                    try
                        logger.Write(LogMessage.Debug(jr.ToString()))
                    with ex ->
                        logger.Write(LogMessage.Error(jr.ToString(), ex, true))

        type PagejobraperHandler(logger : ILogWriter) =
            interface IHandleMessages<Jobs.PageScraperJob> with
                member x.Handle(job) =
                    match job.Payload with
                    | "" -> failwith "Job.Payload empty"
                    | _ ->
                        logger.Write
                            (LogMessage.Debug
                                 (job.CreatedDate.ToString() + "   "
                                  + job.GetType().ToString()))
                        let html =
                            match Some(job.Payload) with
                            | None -> failwith "URL to jobrape required"
                            | Some(a) ->
                                let client = new System.Net.WebClient()
                                let result = client.DownloadString(a)
                                result

                        let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                        try
                            let jr = Jobs.JobResult(job.Id(), true, html)
                            bus.Reply(jr)
                        with ex ->
                            logger.Write
                                (LogMessage.Error
                                     (job.GetType().ToString(), ex, true)) //Console.WriteLine(html)