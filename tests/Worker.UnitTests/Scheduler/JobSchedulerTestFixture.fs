module JobSchedulerTestFixture

open System
open System.Diagnostics
open Microsoft.FSharp.Control
open Xunit
open FsUnit.Xunit
open Lacjam
open Lacjam.Core
open Lacjam.Worker
open Lacjam.Worker.Scheduler
open Ploeh
open NSubstitute
open Ploeh.AutoFixture  
open NServiceBus

let fixtue = AutoFixture.Fixture()
let logger = Substitute.For<ILogWriter>()
let bus = Substitute.For<IBus>();
let js = Substitute.For<IJobScheduler>();
let qs = Substitute.For<Quartz.IScheduler>();

[<Fact>] 
let ``Should return scheduler`` () =
    let js = new Worker.Scheduler.JobScheduler(logger,qs)
    js.Scheduler |> should equal qs
    ()
   