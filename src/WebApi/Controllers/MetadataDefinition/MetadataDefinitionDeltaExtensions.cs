using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Domain.MetadataDefinitions.Commands;
using Lacjam.Framework.Dispatchers;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    public static class MetadataDefinitionDeltaExtensions
    {
        public static void GenerateCommands(this ICommandDispatcher dispatcher, IMetadataDefinitionReadService readService, MetadataDefinitionResource resource)
        {
            var current = readService.FindByIdentity(resource.Identity);

            current.Foreach(original =>
            {
                GenerateReLabelCommand(dispatcher, original, resource);
                GenerateUpdateRegexCommand(dispatcher, original, resource);
                GenerateAssociationCommand(dispatcher, original, resource);
            });

        }

        private static void GenerateReLabelCommand(ICommandDispatcher dispatcher,MetadataDefinitionResource original, MetadataDefinitionResource updated)
        {
            if (original.DiscriptionMatches(updated))
                return;

            dispatcher.Dispatch(new ReLabelMetadataDefinitionCommand(updated.Identity, new MetadataDefinitionName(updated.Name)));
        }

        private static void GenerateUpdateRegexCommand(ICommandDispatcher dispatcher,MetadataDefinitionResource original, MetadataDefinitionResource updated)
        {
            if (original.RegexMatches(updated))
                return;

            dispatcher.Dispatch(new UpdateMetadataDefinitionRegexCommand(updated.Identity,updated.Regex));
        }

        private static void GenerateAssociationCommand(ICommandDispatcher dispatcher,MetadataDefinitionResource original, MetadataDefinitionResource updated)
        {
            //if (original.DefinitionIdsMatch(updated))
            //    return;

            //dispatcher.Dispatch(new AssociateDefinitionsToMetadataDefinitionGroupCommand(original.Identity,
            //    updated.SelectedDefinitionIds));
        }
    }
}