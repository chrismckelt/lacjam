namespace Lacjam.Integration

module SchedulerStats =
    open Autofac
    open HtmlAgilityPack
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain
    open Lacjam.Core.Jobs
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduling
    open Lacjam.Core.User
    open Lacjam.Core.Utility.Html
    open Lacjam.Integration
    open Microsoft.FSharp
    open NServiceBus
    open NServiceBus.Mailer
    open NServiceBus.MessageInterfaces
    open Newtonsoft.Json
    open Quartz
    open Quartz.Impl
    open Quartz.Spi
    open System
    open System.Diagnostics
    open System.Linq
    open System.Net.Mail
    open System.Runtime.Serialization.Json
    open System.ServiceModel
    open System.Text
    open log4net

    type Handler(log : ILogWriter, bus : IBus) =
        do log.Write(Info("SchedulerStatsJobHandler"))
        interface IHandleMessages<Lacjam.Core.Jobs.SchedulerStatsJob> with
            member x.Handle(job) =
                log.Write(Info(job.ToString()))
                try
                    log.Write(LogMessage.Debug("SchedulerStatsJobHandler - getting stats"))
                    let js = Ioc.Resolve<IJobScheduler>()
                    let sb = new StringBuilder()
                    let meta = js.Scheduler.GetMetaData()
                    sb.AppendLine("-- Quartz MetaData --") |> ignore
                    sb.AppendLine("NumberOfJobsExecuted : " + meta.NumberOfJobsExecuted.ToString()) |> ignore
                    sb.AppendLine("Started : " + meta.Started.ToString()) |> ignore
                    sb.AppendLine("RunningSince : " + meta.RunningSince.ToString()) |> ignore
                    sb.AppendLine("SchedulerInstanceId : " + meta.SchedulerInstanceId.ToString()) |> ignore
                    sb.AppendLine("ThreadPoolSize : " + meta.ThreadPoolSize.ToString()) |> ignore
                    sb.AppendLine("InStandbyMode : " + meta.InStandbyMode.ToString()) |> ignore
                    sb.AppendLine("InStandbyMode : " + meta.ToString()) |> ignore
                    sb.AppendLine("GetCurrentlyExecutingJobs ") |> ignore
                    let jobs = js.Scheduler.GetCurrentlyExecutingJobs()
                    for job in jobs do
                        sb.AppendLine(job.JobDetail.Key.Name) |> ignore
                    let jobGroupsNames = js.Scheduler.GetJobGroupNames()
                    for jobGroupName in jobGroupsNames do
                        let groupMatcher = Quartz.Impl.Matchers.GroupMatcher<JobKey>.GroupContains(jobGroupName)
                        let keys = js.Scheduler.GetJobKeys(groupMatcher)
                        for jobKey in keys do
                            let detail = js.Scheduler.GetJobDetail(jobKey)
                            let triggers = js.Scheduler.GetTriggersOfJob(jobKey)
                            sb.AppendLine(jobKey.Name) |> ignore
                            sb.AppendLine(detail.Description) |> ignore
                            for trig in triggers do
                                sb.AppendLine(trig.Key.Group + " " + trig.Key.Name) |> ignore
                                let nextFireTime = trig.GetNextFireTimeUtc()
                                if (nextFireTime.HasValue) then
                                    sb.AppendLine(nextFireTime.Value.LocalDateTime.ToString()) |> ignore
                    log.Write(Info(sb.ToString()))
                    let jr = Jobs.JobResult(job, true, sb.ToString() + " Completed")
                    bus.Reply(jr)
                with ex ->
                    log.Write(Error("SchedulerStatsJobHandler -- " + job.ToString(), ex, true)) //Console.WriteLine(html)
                    let fail = Jobs.JobResult(job, false, ex.ToString())
                    bus.Reply(fail)