namespace Lacjam.Real.Tests
open System
open Xunit
open FsUnit.Xunit
open Lacjam.WebApi.Controllers.MetadataDefinition
open Lacjam.WebApi.Controllers.MetadataDefinitionGroup
open System.Net
open RestSharpWrapper


module WebApiTestFixture =
    
    module MetadataDefintionGroups =

        [<Fact>]
        let ``Should save, retrieve, update and delete metadata definition group `` () =

            //save
            let data = new MetadataDefinitionGroupResource()
            data.Identity <- Guid.NewGuid()
            data.Name <- ("Automated Test + " + data.Identity.ToString())
            data.Description <- ""

            let save = TestHelper.GetDeserializedApiResponse<MetadataDefinitionGroupResource>(fun x -> { x with RestResource = @"MetadataDefinitionGroup/"; Method = POST;  Data= data; })
            let saveResult = save(TestHelper.StartState)
            Console.WriteLine(saveResult.ContentRaw)
            saveResult.StatusCode |>  should equal HttpStatusCode.Created  
            Console.WriteLine("saveResult success")

            // retrieve
            let retrieve = TestHelper.GetDeserializedApiResponse<MetadataDefinitionGroupResource>(fun x -> { x with RestResource = (@"MetadataDefinitionGroup/" + data.Identity.ToString());  Method = GET; })
            let retrieveResult = retrieve(TestHelper.StartState)
            retrieveResult.StatusCode |>  should equal HttpStatusCode.OK 
            Console.WriteLine("retrieveResult success")

            // update
            let dataUpdated = new MetadataDefinitionGroupResource()
            dataUpdated.Identity <- data.Identity
            dataUpdated.Name <- ("Automated Test Updated + " + data.Identity.ToString())
            dataUpdated.Description <- "CHANGED --> Should save, retrieve, update and delete metadata definition group"
            let update = TestHelper.GetDeserializedApiResponse<MetadataDefinitionGroupResource>(fun x -> { x with RestResource = @"MetadataDefinitionGroup/" + data.Identity.ToString();  Method = PUT; Data=dataUpdated })
            let updateResult = update(TestHelper.StartState)
            Console.WriteLine(data.Identity.ToString())
            Console.WriteLine(updateResult.ContentRaw)
            updateResult.StatusCode |>  should equal HttpStatusCode.OK 
            Console.WriteLine("updateResult success")
           
            // delete
            let delete = TestHelper.GetDeserializedApiResponse<MetadataDefinitionGroupResource>(fun x -> { x with RestResource = @"MetadataDefinitionGroup/" + data.Identity.ToString();  Method = DELETE; Data=data })
            let deleteResult = delete(TestHelper.StartState)
            Console.WriteLine(data.Identity.ToString())
            deleteResult.StatusCode |>  should equal HttpStatusCode.OK 
            Console.WriteLine("saveResult success")
          
            ()

    module MetadataDefintions =

            [<Fact>]
            let ``Should save, retrieve, update and delete metadata definition  `` () =

                //save
                let data = new MetadataDefinitionResource()
                data.Identity <-  Guid.NewGuid()
                data.Name <- ("Automated Test + " + data.Identity.ToString())
                data.DataType <- "Text"
                data.Regex <- "\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b"

                let save = TestHelper.GetDeserializedApiResponse<MetadataDefinitionResource>(fun x -> { x with RestResource = @"MetadataDefinition/"; Method = POST;  Data= data; })
                let saveResult = save(TestHelper.StartState)
                Console.WriteLine(saveResult.ContentRaw)
                saveResult.StatusCode |>  should equal HttpStatusCode.Created  
                Console.WriteLine("saveResult success")

                // retrieve
                let retrieve = TestHelper.GetDeserializedApiResponse<MetadataDefinitionResource>(fun x -> { x with RestResource = (@"MetadataDefinition/" + data.Identity.ToString());  Method = GET; })
                let retrieveResult = retrieve(TestHelper.StartState)
                retrieveResult.StatusCode |>  should equal HttpStatusCode.OK 
                Console.WriteLine("retrieveResult success")

                // update
                let dataUpdated = new MetadataDefinitionResource()
                dataUpdated.Identity <- data.Identity
                dataUpdated.Name <- ("Automated Test Updated + " + data.Identity.ToString())
                dataUpdated.DataType <- "Integer"
                dataUpdated.Regex <- "^5[1-5][0-9]{14}$"
                let update = TestHelper.GetDeserializedApiResponse<MetadataDefinitionResource>(fun x -> { x with RestResource = @"MetadataDefinition/" + data.Identity.ToString();  Method = PUT; Data=dataUpdated })
                let updateResult = update(TestHelper.StartState)
                Console.WriteLine(data.Identity.ToString())
                Console.WriteLine(updateResult.ContentRaw)
                updateResult.StatusCode |>  should equal HttpStatusCode.OK 
                Console.WriteLine("updateResult success")
           
                // delete
                let delete = TestHelper.GetDeserializedApiResponse<MetadataDefinitionResource>(fun x -> { x with RestResource = @"MetadataDefinition/" + data.Identity.ToString();  Method = DELETE; Data=data })
                let deleteResult = delete(TestHelper.StartState)
                Console.WriteLine(data.Identity.ToString())
                deleteResult.StatusCode |>  should equal HttpStatusCode.OK 
                Console.WriteLine("saveResult success")
          
            ()


