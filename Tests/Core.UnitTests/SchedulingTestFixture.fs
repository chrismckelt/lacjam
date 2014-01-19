module SchedulingTestFixture

open System
open System.Diagnostics
open Microsoft.FSharp.Control
open Xunit
open Autofac
open FsUnit.Xunit
open Quartz
open Ploeh.AutoFixture
open Ploeh.AutoFixture.AutoFoq
open Ploeh.AutoFixture.DataAnnotations
open NServiceBus
open Foq
open Lacjam
open Lacjam.Core
open Lacjam.Core.Domain
open Lacjam.Core.Runtime
open Lacjam.Core.Jobs
open Lacjam.Core.Scheduling
open Lacjam.Core.Utility


let fixture = Fixture().Customize(AutoFoqCustomization())

[<Fact>]
let ``BatchProcessor handles replies submits job``() =
                
                let guid = Guid.NewGuid()

                let testJobs = [
                                            Jobs.StartUpJob(BatchId=guid, Id=Guid.NewGuid()) :> JobMessage
                                            Jobs.PageScraperJob(BatchId=guid, Id=Guid.NewGuid(), Payload = "http://www.mckelt.com") :> JobMessage
                             ]

                let batch = {Batch.BatchId=guid; Batch.CreatedDate=DateTime.UtcNow; Batch.Id=Guid.NewGuid(); Batch.Name="Test";Batch.Jobs=testJobs; Batch.Status=BatchStatus.Waiting}
                
                let log = {new ILogWriter with member this.Write str = Debug.WriteLine(str)}
                let sched = fixture.Create<Quartz.IScheduler>()
                let bus =   Mock<IBus>().Setup(fun x -> <@ x.Send(testJobs.Head)  @>).Returns(fun b -> Scheduling.callBackReceiver(new CompletionResult())).Create()
                let cb = new ContainerBuilder()
                cb.Register(fun x -> log).As<ILogWriter>() |> ignore
                cb.Register(fun x -> sched).As<IScheduler>() |> ignore
                cb.Register(fun x -> bus).As<ILogWriter>() |> ignore

                let js = new JobScheduler(log, sched, bus) :> IJobScheduler
                js.processBatch(batch)
                