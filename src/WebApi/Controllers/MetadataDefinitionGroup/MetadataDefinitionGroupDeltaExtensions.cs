using Lacjam.Core.Domain.MetadataDefinitionGroups;
using Lacjam.Core.Domain.MetadataDefinitionGroups.Commands;
using Lacjam.Framework.Dispatchers;

namespace Lacjam.WebApi.Controllers.MetadataDefinitionGroup
{
    public static class MetadataDefinitionGroupDeltaExtensions
    {
        public static void GenerateCommands(this ICommandDispatcher dispatcher, IMetadataDefinitonGroupReadService readService, MetadataDefinitionGroupResource resource)
        {
            var current = readService.FindByIdentity(resource.Identity);

            current.Foreach(original =>
            {
                GenerateReLabelCommand(dispatcher, original, resource);
                GenerateAssociationCommand(dispatcher, original, resource);
            });

        }

        private static void GenerateReLabelCommand(ICommandDispatcher dispatcher, MetadataDefinitionGroupResource original, MetadataDefinitionGroupResource updated)
        {
            if (original.DiscriptionMatches(updated))
                return;

            dispatcher.Dispatch(new ReLabelMetadataDefinitionGroupCommand(updated.Identity, new MetadataDefinitionGroupName(updated.Name), new MetadataDefinitionGroupDescription(updated.Description), updated));
        }

        private static void GenerateAssociationCommand(ICommandDispatcher dispatcher, MetadataDefinitionGroupResource original, MetadataDefinitionGroupResource updated)
        {
            if (original.DefinitionIdsMatch(updated))
                return;

            dispatcher.Dispatch(new AssociateDefinitionsToMetadataDefinitionGroupCommand(original.Identity, updated.SelectedDefinitionIds));
        }
    }
}