using Lacjam.Core.Domain.MetadataDefinitions.Commands;
using Lacjam.Framework.Dispatchers;
using Lacjam.WebApi.Infrastructure.Attributes;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    public class MetadataDefinitionController : ApiController
    {
        public MetadataDefinitionController(ICommandDispatcher dispatcher, IMetadataDefinitionReadService metadataDefinitionReadService)
        {
            _dispatcher = dispatcher;
            _metadataDefinitionReadService = metadataDefinitionReadService;
        }

        [HttpPost, Transactional]
        [Route("MetadataDefinition")]
        public HttpResponseMessage Create(MetadataDefinitionResource resource)
        {
            var command = resource.ToCreateAttributeCommand();
            _dispatcher.Dispatch(command);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost, Transactional]
        [Route("MetadataDefinition/{identity}")]
        public HttpResponseMessage ChangeAttributeDetails(Guid identity, MetadataDefinitionResource resource)
        {
            resource.Identity = identity;
            _dispatcher.GenerateCommands( _metadataDefinitionReadService, resource);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete,Transactional]
        [Route("MetadataDefinition/{identity}")]
        public HttpResponseMessage Delete(Guid identity)
        {
            var command = new DeleteMetadataDefinitionCommand(identity);
            _dispatcher.Dispatch(command);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private readonly ICommandDispatcher _dispatcher;
        private readonly IMetadataDefinitionReadService _metadataDefinitionReadService;
    }
}