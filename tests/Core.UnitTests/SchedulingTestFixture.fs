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
open System.Linq



let fixture = Fixture().Customize(AutoFoqCustomization())

[<Fact>]
let ``BatchProcessor handles replies submits job``() =
                
                let guid = Guid.NewGuid()

                let testJobs = new System.Collections.Generic.List<JobMessage>()
                testJobs.Add(Jobs.StartUpJob(BatchId=guid, Id=Guid.NewGuid()) :> JobMessage)
                testJobs.Add(Jobs.PageScraperJob(BatchId=guid, Id=Guid.NewGuid(), Payload = "http://www.mckelt.com") :> JobMessage )
                             

                let batch = {Batch.BatchId=guid; Batch.CreatedDate=DateTime.UtcNow; Batch.Id=Guid.NewGuid(); Batch.Name="Test";Batch.Jobs=testJobs; Batch.Status=BatchStatus.Waiting; Batch.TriggerName="test-trigger";}
                
                let log = {new ILogWriter with member this.Write str = Debug.WriteLine(str)}
                let sched = fixture.Create<Quartz.IScheduler>()
                let cr = new CompletionResult()
                let objList = new System.Collections.Generic.List<obj>()
                objList.Add(new JobResult(new Jobs.SendEmailJob(),true,"test"))
                cr.Messages <- objList.ToArray()
                //let bus =   Mock<IBus>().Setup(fun x -> <@ x.Send(testJobs.Head)  @>).Returns(fun b -> cr).Create()
//                let callback =  { new ICallback member x.Value = ""}
                let bus =   Mock<IBus>().Setup(fun x -> <@ x.Send(testJobs.First())  @>).Returns(fun b -> Foq.Mock<ICallback>().Create()).Create()
                let js = new JobScheduler(log,bus, sched) :> IJobScheduler
                let trig = TriggerBuilder.Create()
                js.scheduleBatch(batch)
                Mock.Verify(<@ js.Scheduler.ScheduleJob(any(),any()) @>, once)
                