using Lacjam.Core.Domain.MetadataDefinitions.Commands;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    public static class MetadataDefinitionResourceExtensions
    {
        public static CreateMetadataDefinitionCommand ToCreateAttributeCommand(this MetadataDefinitionResource resource)
        {
            return new CreateMetadataDefinitionCommand(resource.Identity, resource.Name, resource.DataType, resource.Regex, resource.Values);
        }
    }
}