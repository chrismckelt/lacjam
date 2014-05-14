module QuartzJobExecutorTestFixture

open FsUnit.Xunit
open Lacjam
open Lacjam.Core
open Lacjam.Worker
open Lacjam.Worker.Scheduler
open Microsoft.FSharp.Control
open NServiceBus
open NSubstitute
open Ploeh
open Ploeh.AutoFixture
open System
open System.Diagnostics
open Xunit
open Quartz
open Quartz.Job

let fixtue = AutoFixture.Fixture()
let logger = Substitute.For<ILogWriter>()
let bus = Substitute.For<IBus>()
let js = Substitute.For<IJobScheduler>()
let qs = Substitute.For<Quartz.IScheduler>()
    

[<Fact>]
let ``Get batches should return types to execute``() =
    QuartzJobExecutor.GetBatches() |> should not' (be Null)
    ()

[<Fact>]
let ``Batch name should exist``() =
    QuartzJobExecutor.GetBatches() |> should not' (be Null)
    ()

[<Fact>]
let ``Quartz job should execute internal batch``() =
  //  Lacjam.Core.Runtime.Ioc.Register(Castle.MicroKernel.Registration.Component.For<ILogWriter>().ImplementedBy<LogWriter>().LifestyleTransient())
    let qj = new QuartzJobExecutor(logger,js) :> Quartz.IJob
    let je = Substitute.For<Quartz.IJobExecutionContext>()
    let key = new Quartz.JobKey("InitBatch", "b")
    let jd = Substitute.For<Quartz.IJobDetail>()
    je.JobDetail.ReturnsForAnyArgs jd
    je.JobDetail.Key.ReturnsForAnyArgs key
    qj.Execute(je)

    js.Received().ProcessBatch(Arg.Any<Batches.Batch>())  
    ()