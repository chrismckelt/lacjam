namespace Lacjam.Integration

module CustomJobs =

    open System
    open System.ServiceModel
    open System.Linq
    open System.Text
    open System.Runtime.Serialization.Json
    open System.Net.Mail;
    open System.Diagnostics
    open Microsoft.FSharp
    
    open Newtonsoft.Json
    open HtmlAgilityPack
    open NServiceBus
    open NServiceBus.MessageInterfaces
    open NServiceBus.Mailer
    open log4net
    open Autofac
    open NServiceBus
    open Quartz
    open Quartz.Impl
    open Quartz.Spi
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.User
    open Lacjam.Core.Domain
    open Lacjam.Core.Scheduling
    open Lacjam.Integration
    open Lacjam.Core.Utility.Html
    open Lacjam.Core.Jobs
    
     
//    [<Serializable>]
//    type JiraRoadMapOutputJob() =
//        inherit Lacjam.Core.Jobs.JobMessage()
//        interface NServiceBus.IMessage
//
//
//    type JiraRoadMapOutputJobHandler(log : ILogWriter) =
//        do log.Write(Debug("JiraRoadMapOutputJobHandler ctr"))
//        interface NServiceBus.IHandleMessages<JiraRoadMapOutputJob> with
//            member x.Handle(sc) =
//                log.Write (LogMessage.Info(sc.ToString()))
//                try
//                    Lacjam.Integration.Jira.outputRoadmap()
//                with ex ->  log.Write (LogMessage.Info("--- JIRA Job failed ---"))
//                            log.Write(LogMessage.Error(sc.GetType().ToString(), ex, true)) //Console.WriteLine(html)

       