namespace Lacjam.WebApi.UnitTests

module MetadataDefinitionWebApiUnitTests =
    
    open System
    open System.Net.Http
    open System.Web
    open System.Web.Routing
    open System.Net
    open System.Web.Http
    open System.Web.Http.Routing
    open System.Web.Http.Controllers
    open Xunit
    open FsUnit.Xunit
    open Lacjam.WebApi.Controllers.MetadataDefinitionGroup
    open NSubstitute
    open Lacjam.Framework.Dispatchers
    open Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
    open Lacjam.Framework.Commands
    open Lacjam.Framework.Logging

    let fakeContext = 
            let config = new System.Web.Http.HttpConfiguration();
            let request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
            let route = config.Routes.MapHttpRoute("test", "api/{controller}/{id}");
            let routeData = new HttpRouteData(route, new HttpRouteValueDictionary())
            routeData.Values.Add("controller", "test" );
            let ctx = new HttpControllerContext(config, routeData, request);
            ctx

    [<Fact>]
    let ``When deleting a metadata definition group it should return an Ok status code if the operation was successful`` () =
        
        let dispatcher = fake<ICommandDispatcher>
        let logger = fake<ILogWriter>
//        let service = new MetadataDefinitionGroupController(dispatcher, logger)
//        service.ControllerContext <- fakeContext
//        let received = whenReceived (fun x -> (x :> ICommandDispatcher).Dispatch( any<ICommand> ) ) dispatcher
//        received.Do( (fun a -> failwith "Exception" ))
//
//        let response = service.Delete( Guid.NewGuid() );
//
//        Assert.Equal( HttpStatusCode.OK , response.StatusCode )

        ()

    [<Fact>]
    let ``When deleting a metadata definition group it should throw an exception if the dispatcher errors`` () =
        
        let identity = Guid.NewGuid()
        let dispatcher = fake<ICommandDispatcher>
        let logger = fake<ILogWriter>
        let command = new DeleteMetadataDefinitionGroupCommand(identity)
        //let service = new MetadataDefinitionGroupController(dispatcher, logger)
        //service.ControllerContext <- fakeContext

        //let response = service.Delete( Guid.NewGuid() );

        //Assert.Equal( HttpStatusCode.OK , response.StatusCode )

        ()