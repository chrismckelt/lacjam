using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lacjam.Core.Services;
using Lacjam.Framework.Dispatchers;
using Lacjam.WebApi.Controllers.MetadataDefinitionGroup;

namespace Lacjam.WebApi.Controllers.Schema
{
    public class SchemaController : ApiController
    {
        public SchemaController(ICommandDispatcher dispatcher, ISchemaService schemaService)
        {
            _dispatcher = dispatcher;
            _schemaService = schemaService;
        }

        [HttpPost]
        [Route("schema")]
        public HttpResponseMessage CreateNewSchema(MetadataDefinitionGroupResource resource)
        {
            var command = resource.ToCreateMetadataDefinitionGroupCommand();
            _dispatcher.Dispatch(command);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpGet]
        [Route("schema/{identity}")]
        public HttpResponseMessage GetSchema(Guid identity)
        {
            return _schemaService.GetSchemaFor(identity)
                                .Fold(x => Request.CreateResponse(HttpStatusCode.OK, x.ToSchemaResource()),
                                      () => Request.CreateResponse(HttpStatusCode.NoContent));
        }

        private readonly ICommandDispatcher _dispatcher;
        private readonly ISchemaService _schemaService;
    }
}