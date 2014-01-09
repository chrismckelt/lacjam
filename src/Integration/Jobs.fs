namespace Lacjam.Integration

module Jobs =

    open System
    open System.ServiceModel
    open System.Linq
    open System.Runtime.Serialization.Json
    open System.Diagnostics
    open Microsoft.FSharp.Linq
    open Microsoft.FSharp.Data.TypeProviders
    open Newtonsoft.Json
    open log4net
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.User
    open Lacjam.Core.Domain
    open Lacjam.Core.Scheduler
    open Lacjam.Integration
    open Lacjam.Core.Scheduler.Jobs
    
    [<Serializable>]
    type JiraRoadMapOutputJob() =
        inherit Lacjam.Core.Scheduler.Jobs.Job()
        interface NServiceBus.IMessage
        member x.JobType=JobType.Execute 


    type JiraRoadMapOutputJobHandler(logger : ILogWriter) =
        interface NServiceBus.IHandleMessages<JiraRoadMapOutputJob> with
            member x.Handle(sc) =
                logger.Write (LogMessage.Debug(sc.CreatedDate.ToString() + "   " + sc.JobType.ToString()))                    

                try
                    Lacjam.Integration.Jira.outputRoadmap()
                with ex -> logger.Write(LogMessage.Error(sc.JobType.ToString(), ex, true)) //Console.WriteLine(html)

