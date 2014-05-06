namespace Lacjam.AcceptanceTests
    open System
    open FsUnit.Xunit 
    open Xunit
    open Quartz
    open NServiceBus
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain                                                               
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduling
    open Lacjam.Core.Utility
    open MongoDB
    open MongoDB
    open MongoDB.Driver
    open MongoDB.Driver.Linq
    open MongoDB.Driver.Communication
    open MongoDB.Driver.Communication.Security
    open MongoDB.Bson
    open MongoDB.Bson.Serialization
    open System
    open System.Configuration
   


        module DomainIntegrationTestFixture = 
            //let cs = System.Configuration.ConfigurationManager.AppSettings.Item("MongoDBConnectionString")
            let _settings = new MongoCollectionSettings()
            do
                        _settings.AssignIdOnInsert   <- true;
                        _settings.GuidRepresentation <-  GuidRepresentation .Standard;
            let _cs = @"mongodb://localhost/Lacjam"
            let _db =   let client = new MongoClient(_cs)// connect to localhost
                        let server = client.GetServer()
                        let database = server.GetDatabase("Lacjam")  
                        database

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
            let ``Mongo is good `` () =
                _db |> should not' (Null)
                                                                     
            [<Fact>]
            let ``Clear database`` () =  ()//
                    //Lacjam.Core.Utility.RavenDB.deleteAll<Audit>(Ioc.Resolve<IDocumentSession>())
//                    let session = Ioc.Resolve<IDocumentSession>()
//                    session.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex("Raven/DocumentsByEntityName", new IndexQuery()) |> ignore
//                    session.SaveChanges()

            [<Fact>]
            let ``Audit save and get`` () =   
                let aud = {Audit.Id=MongoDB.Bson.ObjectId.Empty;Audit.AuditType="test";Audit.CreatedDate=DateTime.Now;Audit.Message="test-message";}
                let (collection) = _db.GetCollection(typedefof<Audit>.Name,_settings)
                let result = collection.Insert(aud)
                result |> should not' (Null)    
                for xxx in collection.FindAll() do
                    Console.WriteLine(xxx.ToString())
                result.Ok |> should equal (true)
                printfn "%A" result                                             
                ()
                

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
                

    

