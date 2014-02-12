﻿namespace Lacjam.Core
open Lacjam
open Lacjam.Core
open Lacjam.Core.Domain
open Lacjam.Core.Runtime
open NServiceBus
open NServiceBus.ObjectBuilder.Common
open System
open System.Collections.Concurrent
open System.Collections.Generic
open System.IO
open System.Net
open System.Net.Http
open System.Runtime.Serialization
open System.Text.RegularExpressions
open Quartz
open Quartz.Spi
open Autofac
open LinqToTwitter

  
  
   /// Fantomas
    /// Ctrl + K D   -- format document
    /// Ctrl + K F   -- format selection / format cursor position
    module Jobs =

        [<Serializable>]
        [<AbstractClass>]
        type JobMessage() =
            member val Id = Guid.Empty with get,set
            member val BatchId = Guid.Empty with get,set
            member val CreatedDate = DateTime.UtcNow with get
            member val Payload = "" with get, set
            member val Status = false with get, set
//            member val JobDetail =   { new IJobDetail with 
//                                         member this.Description = "job default description"
//                                         member this.Key = new JobKey("job")
//                                         member this.ConcurrentExecutionDisallowed = false
//                                         member this.GetJobBuilder() = (JobBuilder.Create())
//                                         member this.JobType =  typedefof<JobMessage>
//                                         member this.JobDataMap = null
//                                         member this.RequestsRecovery = false
//                                         member this.PersistJobDataAfterExecution = false
//                                         member this.Durable = false
//                                         member this.Clone() = new obj()
//                                     }  with get, set
            interface IMessage


//        type ScheduledJobMessage(jobMessage) =
//            interface IJobDetail with 
//                member this.Description = name
//                member this.Key = new JobKey(name)
//                member this.ConcurrentExecutionDisallowed = false
//                member this.GetJobBuilder() = (JobBuilder.Create())
//                member this.JobType =  typedefof<JobMessage>
//                member this.JobDataMap = null
//                member this.RequestsRecovery = false
//                member this.PersistJobDataAfterExecution = false
//                member this.Durable = false
                member this.Clone() = new obj()

        [<Serializable>]
        type JobResult(jm:JobMessage, success : bool, result : string) =
            let mutable j = jm
            let mutable suc = success
            let mutable res = result
            let mutable r = Guid.NewGuid()
            member x.JobMessage with get() =j and set(value) = j <- value
            member x.JobResultId with get () = r and set(value) = r <- value
            member val CreatedDate = DateTime.UtcNow with get

            member x.Success with get () = suc and set (v : bool) = suc <- v

            member x.Result with get () = res and set(v) = res <- v

            override x.ToString() =
                String.Format
                    ("Finished :: Job: {0} ResultId: {1} CreatedDate: {2} JobMessage: {3}", x.JobMessage.GetType(), x.JobResultId, x.CreatedDate, x.JobMessage.ToString()  )
            interface IMessage

        
        
        [<Serializable>]
        type StartUpJob() =
            inherit JobMessage()
            interface IMessage

        [<Serializable>]
        type AuditJob() =
            inherit JobMessage()
            member val Audit:Domain.Audit option = None with get,set
            interface IMessage

        [<Serializable>]
        type PageScraperJob() =
            inherit JobMessage()
            member val Url = String.Empty with get, set
            interface IMessage

        [<CLIMutable>]
        type Email = {To:string;From:string;Subject:string;Body:string;}
        
        [<Serializable>]
        type SendEmailJob() =
            inherit JobMessage()
            member val Email:Email = {Email.To="";Email.From="";Email.Subject="";Email.Body=""} with get, set
            interface IMessage

        [<Serializable>]
        type SendTweetJob() =
            inherit JobMessage()
            member val Settings:Lacjam.Core.Settings.TwitterSettings = Settings.getTwitterSettings with get, set
            member val To:string = String.Empty with get,set
            interface IMessage
     
//     [<AbstractClass>]
//     [<CLIMutable>]
//     type BatchJob() =
//            member val Id = Guid.Empty with get,set
//            member val Name = String.Empty with get,set
//            member val BatchId = Guid.Empty with get,set
//            member val CreatedDate = DateTime.UtcNow with get
//            member val Jobs = [] with get,set

     type BatchStatus = 
        | Waiting
        | Processing
        | Success
        | Failed

     [<CLIMutable>]
     type Batch =  {mutable Id: Guid; Name: String; BatchId : Guid; CreatedDate : DateTime; Jobs : Jobs.JobMessage list; Status:BatchStatus; TriggerBuilder:TriggerBuilder}

     type IContainBatches = 
            abstract Batches : Batch list

    [<Serializable>]
    type BatchSubmitterJob() =
        inherit Jobs.JobMessage()
        member val Batch:Batch = {Batch.BatchId=Guid.NewGuid(); Batch.CreatedDate=DateTime.UtcNow; Batch.Id=Guid.NewGuid(); Batch.Name="SeedBatchJob";Batch.Jobs=[]; Batch.Status=BatchStatus.Waiting;Batch.TriggerBuilder=TriggerBuilder.Create();} with get, set
        interface IMessage

    module JobHandlers =
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
        open Newtonsoft.Json
        open Newtonsoft.Json.Serialization
        open NServiceBus.MessageInterfaces
        open NServiceBus.Mailer



        type JobResultHandler(log : Lacjam.Core.Runtime.ILogWriter) =
            do log.Write(Info("JobResultHandler"))
            interface NServiceBus.IHandleMessages<Jobs.JobResult> with
                member x.Handle(jr) =
                    try
                        log.Write(LogMessage.Info("Handling Job Result : " + jr.ToString()))
                    with ex ->
                        log.Write(LogMessage.Error(jr.ToString(), ex, true))

        type StartupJobHandler(log : ILogWriter) =
            do log.Write(Info("StartupJobHandler"))
            interface IHandleMessages<Jobs.StartUpJob> with
                member x.Handle(job) =
                    log.Write(Info(job.ToString()))
                       
                    //TODO get all jobs in batch and list them
                    // any prenotifications
                    let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                    try
                        let jr = Jobs.JobResult(job, true, job.GetType().ToString() + " Completed" )
                        bus.Reply(jr)
                    with ex ->
                        log.Write
                            (LogMessage.Error
                                    ("StartupJobHandler" + job.GetType().ToString(), ex, true)) //Console.WriteLine(html)
                      


        type PageScraperJobHandler(log : ILogWriter) =
            do log.Write(Info("PageScraperJobHandler"))
            interface IHandleMessages<Jobs.PageScraperJob> with
                member x.Handle(job) =
                    match job.Url with
                    | "" -> failwith "Job.Url empty"
                    | _ ->
                        log.Write
                            (LogMessage.Debug
                                 (job.CreatedDate.ToString() + "   "
                                  + job.GetType().ToString()))
                        let html =
                            match Some(job.Url) with
                            | None -> failwith "URL to job scrape required"
                            | Some(a) ->
                                let client = new System.Net.WebClient()
                                let result = client.DownloadString(a)
                                result

                        let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                        try
                            let jr = Jobs.JobResult(job, true, html)
                            bus.Reply(jr)
                        with ex ->
                            log.Write
                                (LogMessage.Error
                                     (job.GetType().ToString(), ex, true)) //Console.WriteLine(html)

        type SendEmailJobHandler(log : ILogWriter) =
            let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
            do log.Write(Info("SendEmailJobHandler"))
            interface IHandleMessages<Jobs.SendEmailJob> with
                member x.Handle(job) =
                    match job.Email.To with
                    | "" -> failwith "Job.Email.To empty"
                    | _ ->
                        log.Write
                            (LogMessage.Debug
                                 (job.CreatedDate.ToString() + "   "
                                  + job.GetType().ToString()))                                           
                        
                        try
                            let mail = new Mail()
                            let al = new AddressList()
                            al.Add(job.Email.To)
                            let subject = match job.Email.Subject.Contains("{") with
                                            | true ->  String.Format(job.Email.Subject, job.Payload)
                                            | false -> job.Payload

                            let body = match job.Email.Body.Contains("{") with
                                            | true ->  String.Format(job.Email.Body, job.Payload)
                                            | false -> job.Payload


                            let mail = new Mail(
                                            To = al,
                                            From = job.Email.From,
                                            Subject = subject,
                                            Body = body
                                        )
                            log.Write(Info("Sending email"))
                            let s = String.Format("{0} {1} {2} {3}",mail.To, mail.From, mail.Subject, mail.Body)
                            log.Write(Info(s))
                            bus.SendMail(mail)
                            let jr = Jobs.JobResult(job, true, s)
                            bus.Reply(jr)
                        with ex ->
                            log.Write
                                (LogMessage.Error
                                     (job.GetType().ToString(), ex, true)) //Console.WriteLine(html)


         type SendTweetJobHandler(log : ILogWriter) =
            do log.Write(Info("SendTweetJobHandler"))
            let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
            interface IHandleMessages<Jobs.SendTweetJob> with
                member x.Handle(job) =
                    match job.To with
                    | "" -> failwith "Job.To empty"
                    | _ ->
                        log.Write(Info("Sending tweet for " + job.ToString()))
                        log.Write
                            (LogMessage.Debug
                                 (job.CreatedDate.ToString() + "   "
                                  + job.GetType().ToString()))                                           
                        
                        try
                           
                            let cred = new LinqToTwitter.SingleUserInMemoryCredentialStore()
                            cred.ConsumerKey <- job.Settings.ConsumerKey
                            cred.ConsumerSecret <- job.Settings.ConsumerSecret
                            cred.AccessToken <- job.Settings.AccessToken
                            cred.AccessTokenSecret <- job.Settings.AccessTokenSecret
                            cred.OAuthToken <- job.Settings.OAuthToken
                            cred.OAuthTokenSecret <- job.Settings.OAuthTokenSecret
                            cred.ScreenName <- job.Settings.ScreenName
                            let auth = new SingleUserAuthorizer()
                            auth.CredentialStore <- cred
                            let twitter = new TwitterContext(auth)
                            let result = twitter.NewDirectMessageAsync(job.To, job.Payload + "  " + DateTime.Now.ToShortDateString())                        
                            result.Wait()
                            let jr = Jobs.JobResult(job, true, String.Format("Tweet sent {0} {1} {2}",job.Settings.ScreenName, job.Payload, result.ToString()))
                            bus.Reply(jr)
                        with ex ->
                            log.Write(LogMessage.Error("SendTweet error: " + job.GetType().ToString(), ex, true)) //Console.WriteLine(html)
                            let fail = Jobs.JobResult(job, false, String.Format("Tweet failed {0} {1} {2}",job.Settings.ScreenName, job.Payload, ex.ToString()))
                            bus.Reply(fail)
                        
                       