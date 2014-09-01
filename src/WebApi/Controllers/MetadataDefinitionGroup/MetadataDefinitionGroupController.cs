using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Remotion.Linq.Utilities;
using Lacjam.Core.Domain.MetadataDefinitionGroups.Commands;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Logging;
using Lacjam.WebApi.Infrastructure.Attributes;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{

    public class MetadataDefinitionGroupController : ApiController
    {
        public MetadataDefinitionGroupController(ICommandDispatcher dispatcher, IMetadataDefinitonGroupReadService metadataDefinitonGroupReadService, ILogWriter logWriter)
        {
            _dispatcher = dispatcher;
            _metadataDefinitonGroupReadService = metadataDefinitonGroupReadService;
            _logWriter = logWriter;
        }

        [Transactional]
        [HttpPost]
        [Route("MetadataDefinitionGroup/create")]
        public HttpResponseMessage CreateMetadataDefinitionGroup(MetadataDefinitionGroupResource resource)
        {
            _logWriter.Info(String.Format("Beginning of processing creation of Definition Group: {0}", resource.Name));
            if (resource == null) throw new ArgumentEmptyException("resource");
            _logWriter.Debug(resource.ToString());
            var command = resource.ToCreateMetadataDefinitionGroupCommand();
            _dispatcher.Dispatch(command);

            _logWriter.Info(String.Format("Completion of processing creation of Definition Group: {0}",
                resource.Name));

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Transactional]
        [HttpGet]
        [Route("MetadataDefinitionGroup/{identity}")]
        public HttpResponseMessage Get([FromUri] Guid identity)
        {
            _logWriter.Info(String.Format("Recieving request for Metadata Definitiorn Group: {0}", identity));
            var rrr= _metadataDefinitonGroupReadService.FindByIdentity(identity)
                .Fold(x => Request.CreateResponse(HttpStatusCode.OK, x),
                    () => Request.CreateErrorResponse(HttpStatusCode.NotFound, "Resource not found"));

            return rrr;
           
        }

        [Transactional]
        [HttpPut]
        [HttpPost]
        [Route("MetadataDefinitionGroup/update/{identity}")]
        public HttpResponseMessage UpdateMetadataDefinitionGroup([FromUri] Guid identity, [FromBody] MetadataDefinitionGroupResource resource)
        {
            _logWriter.Info(String.Format("Update request for Metadata Definition Group: {0}", identity));
            resource.Identity = identity;
            _dispatcher.GenerateCommands(_metadataDefinitonGroupReadService, resource);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Transactional]
        [HttpDelete]
        [HttpGet]
        [Route("MetadataDefinitionGroup/delete/{identity}")]
        public HttpResponseMessage Delete([FromUri] Guid identity)
        {
            _logWriter.Info(String.Format("Deletion request for Metadata Definition Group: {0}", identity));
            
            var command = new DeleteMetadataDefinitionGroupCommand(identity);
            _dispatcher.Dispatch(command);

            _logWriter.Info(String.Format("Deletion request for Metadata Definition Group: {0} completed.", identity));
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private readonly ICommandDispatcher _dispatcher;
        private readonly IMetadataDefinitonGroupReadService _metadataDefinitonGroupReadService;
        private readonly ILogWriter _logWriter;
    }
}