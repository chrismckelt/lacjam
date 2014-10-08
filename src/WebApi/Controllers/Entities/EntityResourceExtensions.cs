using Lacjam.Core.Domain.Entities;
using Lacjam.Core.Domain.Entities.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Lacjam.WebApi.Controllers.Entities
{
    public static class EntityResourceExtensions
    {
        public static EntityResource ToEntityResource(this EntityProjection projection)
        {
            return new EntityResource
            {
                Identity = projection.Identity,
                Name = projection.Name
            };
        }

        public static CreateEntityCommand ToCreateCommand(this EntityResource resource)
        {
            return new CreateEntityCommand(resource.Identity, resource.DefinitionGroup.Id, resource.Name, GetValues(resource));
        }

        private static IEnumerable<ValueSet> GetValues(EntityResource resource)
        {

            if (resource.DefinitionValues == null || !resource.DefinitionValues.Any())
                return Enumerable.Empty<ValueSet>();

            return resource.DefinitionValues.Select(x => new ValueSet
            {
                MetadataDefinitionIdentity = x.MetadataDefinitionIdentity,
                Name = x.Name,
                DataType = x.DataType,
                Regex = x.Regex,
                Values = new List<string>(x.Values)
            });
        }
    }
}