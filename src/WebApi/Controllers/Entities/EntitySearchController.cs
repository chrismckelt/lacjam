using System.Web.Http;
using Lacjam.Core.Services;

namespace Lacjam.WebApi.Controllers.Entities
{
    [RoutePrefix("Search/Entities")]
    public class EntitySearchController: ApiController
    {
        private static readonly string[] DEFAULT_SELECTED_ENTITIES =
        {
            "GP205", "Y3", "Y4", "Y11", "BH4", "N1", "N1 Tiled", "N1 Metal"
        };

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

        [HttpGet]
        [Route("Defaults")]
        public IHttpActionResult GetDefaults()
        {
            return Ok(_indexer.GetByNames(DEFAULT_SELECTED_ENTITIES));
        }
    }
}