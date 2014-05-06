module SendTweetJobHandlerTestFixture
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

    let public fixture = AutoFixture.Fixture()
    let logger = Substitute.For<Lacjam.Core.Runtime.ILogWriter>()
    let bus = Substitute.For<IBus>();
    let js = Substitute.For<IJobScheduler>();
    let qs = Substitute.For<Quartz.IScheduler>();

                                    
    [<Fact>] 
    let ``Should fail if total chars > 140`` () =
        let job = fixture.Create<Jobs.SendTweetJob>()                                 
        let list = [1..200] // |> Seq.forall(fun x -> System.Convert.ToChar(x) |> (fun x -> sb.Append(x))
        let sb = new System.Text.StringBuilder()
        for i in list do
            sb.Append i |> ignore 

        job.Payload <- sb.ToString()
        

        let handler = new Handlers.SendTweetJobHandler(bus,logger)
        Assert.Throws<ValidationException>(fun _ -> handler.Handle(job)) |> ignore
        ()
   