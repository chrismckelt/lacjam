using Remotion.Linq.Utilities;
using Lacjam.Core.Domain.Entities.Commands;
using Lacjam.Core.Services;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Logging;
using Lacjam.WebApi.Infrastructure.Attributes;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lacjam.WebApi.Controllers.Entities
{

    [ValidateModelState]
    [RoutePrefix("Entity")]
    public class EntityController : ApiController
    {
        public EntityController(ICommandDispatcher dispatcher, IEntityReadService readService, ILogWriter logWriter, IEntityIndexer entityIndexer)
        {
            _dispatcher = dispatcher;
            _readService = readService;
            _logWriter = logWriter;
            _entityIndexer = entityIndexer;
        }

        [HttpPost, Transactional]
        [Route]
        public HttpResponseMessage Create(EntityResource resource)
        {
            _logWriter.Info(String.Format("Beginning of processing creation of Entity : {0}", resource.Name));

            if (resource == null)
                throw new ArgumentEmptyException("resource");

            _logWriter.Debug(resource.ToString());

            if (_readService.FindByIdentity(resource.Identity).Exists)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Identity already exists. Identities must be unique");

            return _readService.FindByName(resource.Name).Fold(x =>
                                                                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, resource.Name + " already exists. Names must be unique"),
                                                                    () =>
                                                                    {
                                                                        var command = resource.ToCreateCommand();
                                                                        _dispatcher.Dispatch(command);

                                                                        _logWriter.Info(String.Format("Completion of processing creation of Entity : {0}", resource.Name));

                                                                        return Request.CreateResponse(HttpStatusCode.Created);
                                                                    });

        }

        [HttpGet, Transactional]
        [Route("{identity}")]
        public HttpResponseMessage Get([FromUri] Guid identity)
        {
            _logWriter.Info(String.Format("Recieving request for Entity: {0}", identity));
            return _readService.FindByIdentity(identity)
                               .Fold(x => Request.CreateResponse(HttpStatusCode.OK, x),
                                     () => Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entity Resource Not Found"));
        }

        [HttpPut, Transactional, ValidateModelState]
        [Route("{identity}")]
        public HttpResponseMessage UpdateEntity([FromUri] Guid identity, [FromBody] EntityResource resource)
        {
            _logWriter.Info(String.Format("Update request for Entity: {0}", identity));
            resource.Identity = identity;

            var current = _readService.FindByIdentity(resource.Identity);
            var matchingByName = _readService.FindByName(resource.Name);

            return matchingByName.Filter(x => x.Identity != resource.Identity)
                                 .Fold(x => Request.CreateErrorResponse(HttpStatusCode.BadRequest, resource.Name + " already exists. Names must be unique"),
                                       () => current.Fold(z =>
                                       {
                                           _dispatcher.GenerateCommands(_readService, resource);
                                           return Request.CreateResponse(HttpStatusCode.OK);
                                       },
                                                          () => Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entity Resource Not Found"))
                                 );
        }

        [HttpDelete, Transactional]
        [Route("{identity}")]
        public HttpResponseMessage Delete(Guid identity)
        {
            _logWriter.Info(String.Format("Deletion request for Entity: {0}", identity));

            var current = _readService.FindByIdentity(identity);

            return current.Fold(_ =>
            {
                var command = new DeleteEntityCommand(identity);
                _dispatcher.Dispatch(command);

                _logWriter.Info(String.Format("Deletion request for Entity: {0} completed.", identity));
                return Request.CreateResponse(HttpStatusCode.OK);
            }, () => Request.CreateErrorResponse(HttpStatusCode.NotFound, "Entity Resource Not Found"));

        }

        [HttpGet]
        [Route("List")]
        public IHttpActionResult List([FromUri]PagedQuery query)
        {
            return Ok(_entityIndexer.SearchAllMetadata(query.Q, query.PageSize, query.Page));
        }

        private readonly ICommandDispatcher _dispatcher;
        private readonly IEntityReadService _readService;
        private readonly ILogWriter _logWriter;
        private readonly IEntityIndexer _entityIndexer;
    }
}
