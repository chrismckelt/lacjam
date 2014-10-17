using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Lacjam.Core.Domain.MetadataDefinitionGroups.Commands;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Logging;
using Lacjam.WebApi.Infrastructure.Attributes;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    [ValidateModelState]
    [RoutePrefix("MetadataDefinitionGroup")]
    public class MetadataDefinitionGroupController : ApiController
    {
        public MetadataDefinitionGroupController(ICommandDispatcher dispatcher, IMetadataDefinitonGroupReadService metadataDefinitonGroupReadService, ILogWriter logWriter)
        {
            _dispatcher = dispatcher;
            _metadataDefinitonGroupReadService = metadataDefinitonGroupReadService;
            _logWriter = logWriter;
        }

        [ValidateModelState]
        [Transactional]
        [HttpPost]
        [Route]
        public HttpResponseMessage CreateMetadataDefinitionGroup(MetadataDefinitionGroupResource resource)
        {
            
            if (resource == null)
                throw new ArgumentNullException("resource");

            _logWriter.Info("Cleaning Record");
            resource.Clean();

            _logWriter.Info(String.Format("Beginning of processing creation of Definition Group: {0}", resource.Name));
            _logWriter.Debug(resource.ToString());

            if (_metadataDefinitonGroupReadService.FindByIdentity(resource.Identity).Exists)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"Identity already exists. Identities must be unique");

            if (_metadataDefinitonGroupReadService.FindByName(resource.Name).Exists)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,resource.Name + " already exist. Names must be unique");

            var command = resource.ToCreateMetadataDefinitionGroupCommand();
            _dispatcher.Dispatch(command);

            _logWriter.Info(String.Format("Completion of processing creation of Definition Group: {0}",resource.Name));

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [Transactional]
        [HttpGet]
        [Route("{identity}")]
        public HttpResponseMessage Get([FromUri] Guid identity)
        {
            _logWriter.Info(String.Format("Recieving request for Metadata Definitiorn Group: {0}", identity));
            var result= _metadataDefinitonGroupReadService.FindByIdentity(identity)
                                                          .Fold(x => Request.CreateResponse(HttpStatusCode.OK, x),
                                                                () => Request.CreateErrorResponse(HttpStatusCode.NotFound, "Metadata definition group not found"));

            return result;
           
        }

        [Transactional]
        [HttpGet]
        [Route("list/all")]
        public HttpResponseMessage GetAll()
        {
            _logWriter.Info(String.Format("Recieving request for Metadata Definitiorn Group: {0}", "All"));
            var arr = _metadataDefinitonGroupReadService.GetAll().ToArray();
            return Request.CreateResponse(HttpStatusCode.OK,new {result= arr});
        }


        [ValidateModelState]
        [Transactional]
        [HttpPut]
        [Route("{identity}")]
        public HttpResponseMessage UpdateMetadataDefinitionGroup([FromUri] Guid identity, [FromBody] MetadataDefinitionGroupResource resource)
        {

            if (resource == null)
                throw new ArgumentNullException("resource");

            _logWriter.Info("Cleaning Record");
            resource.Clean();

            _logWriter.Info(String.Format("Update request for Metadata Definition Group: {0}", identity));
            resource.Identity = identity;

            var current = _metadataDefinitonGroupReadService.FindByIdentity(resource.Identity);
            var matchingName = _metadataDefinitonGroupReadService.FindByName(resource.Name);

            if (matchingName.Exists && matchingName.Value.Identity != resource.Identity) 
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, resource.Name + " already exists. Names must be unique");

            if (current == null) 
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Group not found");

            if (current.Value.Identity != resource.Identity)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Group not found");

            _dispatcher.GenerateCommands(_metadataDefinitonGroupReadService, resource);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Transactional]
        [HttpDelete]
        [Route("{identity}")]
        public HttpResponseMessage Delete([FromUri] Guid identity)
        {
            _logWriter.Info(String.Format("Deletion request for Metadata Definition Group: {0}", identity));
            
            var command = new DeleteMetadataDefinitionGroupCommand(identity);
            _dispatcher.Dispatch(command);

            _logWriter.Info(String.Format("Deletion request for Metadata Definition Group: {0} completed.", identity));
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("select")]
        public IHttpActionResult SearchDefinitions([FromUri]PagedQuery args)
        {
            _logWriter.Info(String.Format("Searching definitions. q: {0}", args.Q));
            var results = _metadataDefinitonGroupReadService.SearchSelections(args.Q, args.PageSize, args.Page);

            return Ok(results);
        }

        [Transactional]
        [HttpGet]
        [Route("{identity}/definitions")]
        public IHttpActionResult GetDefinitions([FromUri] Guid identity)
        {
            _logWriter.Info(String.Format("Recieving request for Metadata Definitiorn Group: {0}", identity));
            return Ok(_metadataDefinitonGroupReadService.GetDefinitions(identity));

        }

        private readonly ICommandDispatcher _dispatcher;
        private readonly IMetadataDefinitonGroupReadService _metadataDefinitonGroupReadService;
        private readonly ILogWriter _logWriter;
    }
}