using System.Linq;
using Lacjam.Core.Domain.MetadataDefinitionGroups;
using Lacjam.Core.Domain.MetadataDefinitionGroups.Commands;
using Lacjam.Core.Infrastructure;
using Lacjam.WebApi.Controllers.MetadataDefinition;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    public static class MetadataDefinitionGroupResourceExtensions
    {
        public static CreateMetadataDefinitionGroupCommand ToCreateMetadataDefinitionGroupCommand(this MetadataDefinitionGroupResource resource)
        {
            return new CreateMetadataDefinitionGroupCommand(resource.Identity, resource.Name, resource.Description, new MetadataBag(resource.Definitions.Select(x=> x.Id).ToArray()), resource.Tracking);
        }

        public static MetadataDefinitionGroupResource ToMetadataDefinitionGroupResource(this MetadataDefinitionGroupProjection group)
        {
            if (group.Tracking != null)
                return new MetadataDefinitionGroupResource
                {
                    Identity = group.Identity,
                    Name = group.Name,
                    Description = group.Description,
                    Tracking = group.Tracking
                };

            return new MetadataDefinitionGroupResource
            {
                Identity = @group.Identity,
                Name = @group.Name,
                Description = @group.Description,
                Tracking = new TrackingBase(),
                Definitions = new MetadataDefinitionSelectResource[0]
            };
        }

        public static MetadataDefinitionGroupSelectResource ToSelectResource(
            this MetadataDefinitionGroupProjection group)
        {
            if (group == null)
            {
                return null;
            }

            return new MetadataDefinitionGroupSelectResource
            {
                Id = group.Identity,
                Text = group.Name
            };
        }
    }
}