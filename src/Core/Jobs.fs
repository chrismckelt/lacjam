namespace Lacjam.Core

module Jobs = ()
//
//module JobHandlers =                                   
//    open Lacjam
//    open Lacjam.Core
//    open Lacjam.Core.Domain
//    open Lacjam.Core.Runtime
//    open NServiceBus
//   // open NServiceBus.Mailer
//    open NServiceBus.MessageInterfaces
//    open Newtonsoft.Json
//    open Newtonsoft.Json.Serialization
//    open System
//    open System.Collections.Concurrent
//    open System.Collections.Generic
//    open System.IO
//    open System.Net
//    open System.Net.Http
//    open System.Runtime.Serialization
//    open System.Text
//    open System.Text.RegularExpressions
//    open log4net
//
//
//    type StartupJobHandler(log : ILogWriter) =
//        do log.Write(Info("StartupJobHandler"))
//        interface IHandleMessages<Jobs.StartUpJob> with
//            member x.Handle(job) =
//                log.Write(Info(job.ToString()))
//                let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
//                try
//                    let jr = Jobs.JobResult(job, true, job.GetType().ToString() + " Completed")
//                    bus.Reply(jr)
//                with ex ->
//                    log.Write(Error("StartupJobHandler -- " + job.ToString(), ex, true))
//                    let fail = Jobs.JobResult(job, false, ex.ToString(), TimeSpan.FromMinutes(double 15))
//                    bus.Reply(fail)
//
//
//
//    type SendEmailJobHandler(log : ILogWriter) =
//        let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
//        do log.Write(Info("SendEmailJobHandler"))
//        interface IHandleMessages<Jobs.SendEmailJob> with
//            member x.Handle(job) =
//                match job.Email.To with
//                | "" -> failwith "Job.Email.To empty"
//                | _ ->
//                    log.Write(Info(" -- SendEmailJob --"))
//                    log.Write(Info(job.ToString()))
////                    try
////                        let mail = new Mail()
////                        let al = new AddressList()
////                        al.Add(job.Email.To)
////                        let subject =
////                            match job.Email.Subject.Contains("{") with
////                            | true -> String.Format(job.Email.Subject, job.Payload)
////                            | false -> job.Payload
////
////                        let body =
////                            match job.Email.Body.Contains("{") with
////                            | true -> String.Format(job.Email.Body, job.Payload)
////                            | false -> job.Payload
////
////                        let mail = new Mail(To = al, From = job.Email.From, Subject = subject, Body = body)
////                        log.Write(Info("Sending email"))
////                        let s = String.Format("{0} {1} {2} {3}", mail.To, mail.From, mail.Subject, mail.Body)
////                        log.Write(Info(s))
////                        bus.SendMail(mail)
////                        let jr = Jobs.JobResult(job, true, s)
////                        bus.Reply(jr)
////                    with ex ->
////                        log.Write(Error("SendEmailJobHandler -- " + job.ToString(), ex, true))
////                        let fail = Jobs.JobResult(job, false, ex.ToString())
////                        bus.Reply(fail)
//
//    type SendTweetJobHandler(log : ILogWriter) =
//        do log.Write(Info("SendTweetJobHandler"))
//        let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
//        interface IHandleMessages<Jobs.SendTweetJob> with
//            member x.Handle(job) =
//                match job.To with
//                | "" -> failwith "Job.To empty"
//                | _ ->
//                    log.Write(Info("--- Sending tweet --- "))
//                    log.Write(Info(job.To))
//                    log.Write(Debug(job.ToString()))
//                    try
//                        if (job.Payload.Length > 140) then failwith ("Tweet message too long - " + job.Payload)
//                        let cred = new LinqToTwitter.SingleUserInMemoryCredentialStore()
//                        cred.ConsumerKey <- job.Settings.ConsumerKey
//                        cred.ConsumerSecret <- job.Settings.ConsumerSecret
//                        cred.AccessToken <- job.Settings.AccessToken
//                        cred.AccessTokenSecret <- job.Settings.AccessTokenSecret
//                        cred.OAuthToken <- job.Settings.OAuthToken
//                        cred.OAuthTokenSecret <- job.Settings.OAuthTokenSecret
//                        cred.ScreenName <- job.Settings.ScreenName
//                        let auth = new SingleUserAuthorizer()
//                        auth.CredentialStore <- cred
//                        let twitter = new TwitterContext(auth)
//                        let result =
//                            twitter.NewDirectMessageAsync(job.To, job.Payload + "  " + DateTime.Now.ToShortDateString())
//                        result.Wait()
//                        let jr =
//                            Jobs.JobResult
//                                (job, true,
//
//                                 String.Format
//                                     ("Tweet sent {0} {1} {2}", job.Settings.ScreenName, job.Payload, result.ToString()))
//                        bus.Reply(jr)
//                    with ex ->
//                        log.Write(Error("SendTweetJobHandler -- " + job.ToString(), ex, true))
//                        let fail =
//                            Jobs.JobResult
//                                (job, false,
//
//                                 String.Format
//                                     ("Tweet failed {0} {1} {2}", job.Settings.ScreenName, job.Payload, ex.ToString()))
//                        bus.Reply(fail)