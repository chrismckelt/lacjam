using Lacjam.Core.Domain.MetadataDefinitionGroups.Commands;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    public static class MetadataDefinitionGroupResourceExtensions
    {
        public static CreateMetadataDefinitionGroupCommand ToCreateMetadataDefinitionGroupCommand(this MetadataDefinitionGroupResource resource)
        {
            return new CreateMetadataDefinitionGroupCommand(resource.Identity, resource.Name,resource.Description, resource);
        }
    }
}