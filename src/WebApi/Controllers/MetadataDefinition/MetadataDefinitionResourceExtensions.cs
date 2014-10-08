using System.Linq;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Domain.MetadataDefinitions.Commands;
using Lacjam.Framework.Extensions;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    public static class MetadataDefinitionResourceExtensions
    {
        public static CreateMetadataDefinitionCommand ToCreateCommand(this MetadataDefinitionResource resource)
        {
            var values = (resource.Values == null || !resource.Values.Any()) ? Enumerable.Empty<string>() : resource.Values.Where(x => !string.IsNullOrEmpty(x));
            return new CreateMetadataDefinitionCommand(resource.Identity.IfGuidEmptyCreateNew(), resource.Name, resource.Description, resource.DataType, resource.Regex, values);
        }

        public static MetadataDefinitionResource ToMetadataDefinitionResource(this MetadataDefinitionProjection projection)
        {
            return new MetadataDefinitionResource
            {
                DataType = projection.DataType,
                Identity = projection.Identity,
                Name=projection.Name,Description = projection.Description,
                Regex = projection.Regex,Tracking = projection.Tracking
            };
        }

        public static MetadataDefinitionSelectResource ToMetadataDefinitionSelectResource(
            this MetadataDefinitionProjection projection)
        {
            return new MetadataDefinitionSelectResource
            {
                Id = projection.Identity,
                Text = projection.Name
            };
        }
    }
}