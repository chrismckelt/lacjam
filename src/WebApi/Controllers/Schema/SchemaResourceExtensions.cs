using System.Linq;
using Lacjam.Core.Services;

namespace Lacjam.WebApi.Controllers.Schema
{
    public static class SchemaResourceExtensions
    {
        public static SchemaResource ToSchemaResource(this SchemaTransferObject dto)
        {
            return new SchemaResource
            {
                Identity = dto.Entity.Identity,
                Name = dto.Entity.Name,
                Description = dto.Entity.Name,
                Attributes = dto.Attributes.Select(x => new SchemaResourceAttribute
                                                            {
                                                                Identity = x.ConceptIdentity,
                                                                Name = x.Name,
                                                                Description = x.Name
                                                            })
                                                            .ToList()
            };
        }
    }
}