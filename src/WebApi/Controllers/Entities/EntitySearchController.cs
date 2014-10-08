using System.Web.Http;
using Lacjam.Core.Services;

namespace Lacjam.WebApi.Controllers.Entities
{
    [RoutePrefix("Search/Entity")]
    public class EntitySearchController: ApiController
    {
        private readonly IEntityIndexer _indexer;

        public EntitySearchController(IEntityIndexer indexer)
        {
            _indexer = indexer;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get([FromUri]PagedQuery query)
        {
            return Ok(_indexer.SearchKeywords(query.Q, query.PageSize, query.Page));
        }
    }
}