namespace Lacjam.Core
    open System
 
    open Quartz
    open NServiceBus
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain                                                               
    open Lacjam.Core.Runtime

    open Autofac
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
   
    [<AutoOpen>]
    module Services =   
            type ServiceBase( logger) =
               
                                        let connection =
                                                        let settings = new MongoCollectionSettings()
                                                        settings.AssignIdOnInsert   <- true;
                                                        settings.GuidRepresentation <-  GuidRepresentation .Standard;
                                                        let cs = @"mongodb://localhost/Lacjam"
                                                        let client = new MongoClient(cs)// connect to localhost
                                                        let server = client.GetServer()
                                                        let database = server.GetDatabase("Lacjam")  
                                                        database
