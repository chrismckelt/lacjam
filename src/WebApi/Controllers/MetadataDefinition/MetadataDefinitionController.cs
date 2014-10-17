using System.Collections.Generic;
using System.Linq;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Domain.MetadataDefinitions.Commands;
using Lacjam.Core.Services;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Logging;
using Lacjam.WebApi.Infrastructure.Attributes;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    [ValidateModelState]
    [RoutePrefix("MetadataDefinition")]
    public class MetadataDefinitionController : ApiController
    {

        public MetadataDefinitionController(ICommandDispatcher dispatcher,IMetadataDefinitionReadService metadataDefinitonReadService, ILogWriter logWriter, IMetadataDefinitionIndexer indexer)
        {
            _dispatcher = dispatcher;
            _metadataDefinitonReadService = metadataDefinitonReadService;
            _logWriter = logWriter;
            _indexer = indexer;
        }

        [HttpPost, Transactional, ValidateModelState]
        [Route]
        public HttpResponseMessage Create(MetadataDefinitionResource resource)
        {
            _logWriter.Info(String.Format("Beginning of processing creation of Definition : {0}", resource.Name));

            if (resource == null)
                throw new ArgumentNullException("resource");

            _logWriter.Debug(resource.ToString());

            if (_metadataDefinitonReadService.FindByIdentity(resource.Identity).Exists)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Identity already exists. Identities must be unique");

            return _metadataDefinitonReadService.FindByName(resource.Name).Fold(x =>
                                                                                Request.CreateErrorResponse(HttpStatusCode.BadRequest, resource.Name + " already exist. Names must be unique"),
                                                                                () =>
                                                                                {
                                                                                    var errors = ValidateResource(resource);
                                                                                    if (!String.IsNullOrWhiteSpace(errors))
                                                                                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, errors);

                                                                                    var command = resource.ToCreateCommand();
                                                                                    _dispatcher.Dispatch(command);

                                                                                    _logWriter.Info(String.Format("Completion of processing creation of Definition : {0}", resource.Name));

                                                                                    return Request.CreateResponse(HttpStatusCode.Created);
                                                                                });

        }

        [HttpGet, Transactional]
        [Route("{identity}")]
        public HttpResponseMessage Get([FromUri] Guid identity)
        {
            _logWriter.Info(String.Format("Recieving request for Metadata Definition : {0}", identity));
            return _metadataDefinitonReadService.FindByIdentity(identity)
                                                .Fold(x => Request.CreateResponse(HttpStatusCode.OK, x),
                                                      () => Request.CreateErrorResponse(HttpStatusCode.NotFound, "Metadata Definition Resource Not Found"));
        }

        [HttpGet, Transactional]
        [Route("getdatatypes")]
        public HttpResponseMessage GetDataTypes()
        {
            _logWriter.Info(String.Format("Recieving request for Metadata Definition : {0}", "GetDataTypes"));
            var data = DataTypeBuilder.GetDataTypes().Where(x => !string.IsNullOrEmpty(x.Value.Tag)).Select(x => x.Value);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }


        [Transactional]
        [HttpGet]
        [Route("list")]
        public IHttpActionResult List([FromUri]PagedQuery query)
        {
            _logWriter.Info(String.Format("Recieving request for Metadata Definition: {0}", "All"));
            return Ok(_indexer.Search(query.Q, query.PageSize, query.Page));
        }


        [HttpPut, Transactional, ValidateModelState]
        [Route("{identity}")]
        public HttpResponseMessage UpdateMetadataDefinition([FromUri] Guid identity, [FromBody] MetadataDefinitionResource resource)
        {
            _logWriter.Info(String.Format("Update request for Metadata Definition : {0}", identity));
            resource.Identity = identity;

            var current = _metadataDefinitonReadService.FindByIdentity(resource.Identity);
            var matchingByName = _metadataDefinitonReadService.FindByName(resource.Name);

            return matchingByName.Filter(x => x.Identity != resource.Identity)
                                 .Fold(x => Request.CreateErrorResponse(HttpStatusCode.BadRequest, resource.Name + " already exists. Names must be unique"),
                                       () => current.Fold(z =>
                                                              {
                                                                  _dispatcher.GenerateCommands(_metadataDefinitonReadService, resource);
                                                                  return Request.CreateResponse(HttpStatusCode.OK);
                                                              },
                                                          () => Request.CreateErrorResponse(HttpStatusCode.NotFound, "Metadata Definition Resource Not Found"))
                                 );
        }

        [HttpDelete, Transactional]
        [Route("{identity}")]
        public HttpResponseMessage Delete(Guid identity)
        {
            _logWriter.Info(String.Format("Deletion request for Metadata Definition : {0}", identity));

            var current = _metadataDefinitonReadService.FindByIdentity(identity);

            return current.Fold(_ =>
            {
                var command = new DeleteMetadataDefinitionCommand(identity);
                _dispatcher.Dispatch(command);

                _logWriter.Info(String.Format("Deletion request for Metadata Definition : {0} completed.", identity));
                return Request.CreateResponse(HttpStatusCode.OK);
            }, () => Request.CreateErrorResponse(HttpStatusCode.NotFound, "Metadata Definition Resource Not Found"));

        }

        private string ValidateResource(MetadataDefinitionResource resource)
        {
            var sb = new StringBuilder();
            if (DataTypeBuilder.Create(resource.DataType) != null)
                return sb.ToString();

            sb.AppendLine("Invalid DataType");
            sb.AppendLine("Valid data type included");

            foreach (var dataType in DataTypeBuilder.GetDataTypes())
                sb.Append(dataType.Value.Tag + ",");

            return sb.ToString();
        }

        [HttpGet]
        [Route("select")]
        public IHttpActionResult SearchDefinitions([FromUri]PagedQuery args)
        {
            _logWriter.Info(String.Format("Searching definitions. q: {0}", args.Q));
            var results = _metadataDefinitonReadService.SearchSelections(args.Q, args.PageSize, args.Page);

            return Ok(results);
        }

        private readonly ICommandDispatcher _dispatcher;
        private readonly IMetadataDefinitionReadService _metadataDefinitonReadService;
        private readonly ILogWriter _logWriter;
        private readonly IMetadataDefinitionIndexer _indexer;
    }
}