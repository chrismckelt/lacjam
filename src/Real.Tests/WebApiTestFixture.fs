namespace Lacjam.Real.Tests
open System
open System.Linq
open System.Data.Sql
open Microsoft.FSharp
open Xunit
open FsUnit.Xunit
open FSharp.Data
open FSharp.Data
open System
open System.Linq
open System.Data.Sql
open Microsoft.FSharp
open Xunit
open FsUnit.Xunit
open Lacjam.Core
open Lacjam.Core.Domain.MetadataDefinitionGroups
open Lacjam.Core.Domain.MetadataDefinitionGroups.Events
open Lacjam.Framework.Events
open Lacjam.Framework.Storage
open Lacjam.Core.Infrastructure
open Lacjam.Core.Infrastructure.Ioc
open Lacjam.WebApi.Controllers.MetadataDefinitionGroup
open Lacjam.Core.Infrastructure.Database
open HibernatingRhinos.Profiler.Appender.NHibernate

open NHibernate

open FSharp
open FSharp.Data

open System.Net
open Newtonsoft
open Newtonsoft.Json

open RestSharpWrapper


module WebApiTestFixture =
    

    module MetadataDefintionGroups =
        
        [<Literal>]
        let json = """ {
Identity: "8cd4a5c0-ea88-4711-ad3a-8b889487e829",
Name: "aaa",
Description: "@fff"
} """

        type mdgProvider = JsonProvider<""" {"Identity":"6840c67a-da21-471c-b681-149fec559383","Name":"nnnn","Description":"DDD"} """>

        [<Fact>]
        let ``Should save, retrieve, update and delete metadata definition group `` () =

            //save
            let data = new MetadataDefinitionGroupResource()
            data.Identity <- Guid.NewGuid()
            data.Name <- "Automated Test"
            data.Description <- "Should save, retrieve, update and delete metadata definition group"

            let save = TestHelper.GetDeserializedApiResponse<MetadataDefinitionGroupResource>(fun x -> { x with RestResource = @"MetadataDefinitionGroup/create"; Method = POST;  Data= data; })
            let saveResult = save(TestHelper.StartState)
            //Console.WriteLine(saveResult.ContentRaw)
            saveResult.StatusCode |>  should equal HttpStatusCode.Created  
            Console.WriteLine("saveResult success")

            // retrieve
            let retrieve = TestHelper.GetDeserializedApiResponse<MetadataDefinitionGroupResource>(fun x -> { x with RestResource = (@"MetadataDefinitionGroup/" + data.Identity.ToString());  Method = GET; })
            let retrieveResult = retrieve(TestHelper.StartState)
           // Console.WriteLine(data.Identity.ToString())
           // Console.WriteLine(retrieveResult.ContentRaw)
            retrieveResult.StatusCode |>  should equal HttpStatusCode.OK 
            Console.WriteLine("retrieveResult success")

            // update
            let dataUpdated = new MetadataDefinitionGroupResource()
            dataUpdated.Identity <- data.Identity
            dataUpdated.Name <- "CHANGED --> Automated Test"
            dataUpdated.Description <- "CHANGED --> Should save, retrieve, update and delete metadata definition group"
            let update = TestHelper.GetDeserializedApiResponse<MetadataDefinitionGroupResource>(fun x -> { x with RestResource = @"MetadataDefinitionGroup/update/" + data.Identity.ToString();  Method = POST; Data=dataUpdated })
            let updateResult = update(TestHelper.StartState)
            Console.WriteLine(data.Identity.ToString())
            Console.WriteLine(updateResult.ContentRaw)
            updateResult.StatusCode |>  should equal HttpStatusCode.OK 
            //updateResult.ContentRaw |>  should contain  "False" 
            Console.WriteLine("updateResult success")
           
            // delete
            let delete = TestHelper.GetDeserializedApiResponse<MetadataDefinitionGroupResource>(fun x -> { x with RestResource = @"MetadataDefinitionGroup/delete/" + data.Identity.ToString();  Method = GET; Data=data })
            let deleteResult = delete(TestHelper.StartState)
            Console.WriteLine(data.Identity.ToString())
            //Console.WriteLine(deleteResult.ContentRaw)
            deleteResult.StatusCode |>  should equal HttpStatusCode.OK 
            Console.WriteLine("saveResult success")
          
            ()
