namespace Lacjam.AcceptanceTests
    open System
    open FsUnit.Xunit 
    open Xunit
    open Quartz
//    open Ploeh.AutoFixture
//    open Ploeh.AutoFixture.AutoFoq
//    open Ploeh.AutoFixture.DataAnnotations
//    
//    open Foq
    open NServiceBus
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain                                                               
    open Lacjam.Core.Runtime
    open Lacjam.Core.Jobs
    open Lacjam.Core.Scheduling
    open Lacjam.Core.Utility
    open Lacjam.Integration
    open Autofac


        module DomainIntegrationTestFixture = 
//            let fixture = Fixture().Customize(AutoFoqCustomization())
//            let guidId = Guid.NewGuid()
//            let swJob = CustomJobs.SwellNetRatingJob(Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>())
//            let swJobs = new System.Collections.Generic.List<Jobs.JobMessage>()
//            swJobs.Add(StartUpJob(BatchId=guidId) :> JobMessage)
//            swJobs.Add(PageScraperJob(BatchId=guidId, Id=guidId, Url = "http://www.swellnet.com/reports/australia/western-australia/perth") :> JobMessage)
//            swJobs.Add(swJob :> JobMessage)
//            SendTweetJob(To="chris_mckelt") :> JobMessage
//            SendEmailJob(Email={To="Chris@mckelt.com";From="Chris@mckelt.com";Subject="SwellNet Rating: {0}";Body="SwellNet Rating: {0}"}) :> JobMessage
                                                                     
            [<Fact>]
            let ``Clear database`` () =  ()//
                    //Lacjam.Core.Utility.RavenDB.deleteAll<Audit>(Ioc.Resolve<IDocumentSession>())
//                    let session = Ioc.Resolve<IDocumentSession>()
//                    session.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex("Raven/DocumentsByEntityName", new IndexQuery()) |> ignore
//                    session.SaveChanges()

            [<Fact>]
            let ``Audit save and get`` () = ()   
//                let aud = {Audit.Id=Guid.NewGuid();Audit.AuditType="test";Audit.CreatedDate=DateTime.Now;Audit.Message="test-message";}
//                use session = Ioc.Resolve<IDocumentSession>()
//                session.Store(aud)
//                session.SaveChanges()
//                let gotit = session.Load<Audit>(aud.Id)
//                gotit |> should not' (Null)
//                printfn "%A" gotit

            [<Fact>]
            let ``Batch save and get`` () = ()   
//                let bb = {Batch.BatchId=guidId; Batch.CreatedDate=DateTime.Now; Batch.Id=Guid.NewGuid(); Batch.Name="surfReportBatch";Batch.Jobs=swJobs; Batch.Status=BatchStatus.Waiting;Batch.TriggerName=Lacjam.Core.BatchSchedule.Daily.ToString();}
//                use session = Ioc.Resolve<IDocumentSession>()
//                session.Store(bb)
//                session.SaveChanges()
//                let gotit = session.Load<Batch>(bb.Id)
//                gotit |> should not' (Null)
//                printfn "%A" gotit


            [<Fact>]
            let ``Site save and get`` () = ()   
//                let ss = {Site.Id=Guid.NewGuid(); Site.Name="www.mckelt.com";Site.Url="http://www.mckelt.com";Site.CreatedDate=DateTime.Now}
//                use session = Ioc.Resolve<IDocumentSession>()
//                session.Store(ss)
//                session.SaveChanges()
//                let gotit = session.Load<Site>(ss.Id)
//                gotit |> should not' (Null)
//                printfn "%A" gotit
//                let found =  query {
//                                    for x in session.Query<Site>() do
//                                    where (x.Name = "www.mckelt.com")
//                                    select x
//                                    }
//                found |> should not' (Null)
                

    