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
                GenerateReLabelDescriptionCommand(dispatcher, original, resource);
                GenerateChangeDataTypeCommand(dispatcher, original, resource);
                GenerateUpdateRegexCommand(dispatcher, original, resource);
                GenerateValues(dispatcher, original, resource);
            });
        }

        private static void GenerateReLabelCommand(ICommandDispatcher dispatcher,MetadataDefinitionResource original, MetadataDefinitionResource updated)
        {
            if (original.NameMatches(updated))
                return;

            dispatcher.Dispatch(new ReLabelMetadataDefinitionCommand(updated.Identity, new MetadataDefinitionName(updated.Name.Trim())));
        }

        private static void GenerateReLabelDescriptionCommand(ICommandDispatcher dispatcher, MetadataDefinitionResource original, MetadataDefinitionResource updated)
        {
            if (original.DescriptionMatches(updated))
                return;

            dispatcher.Dispatch(new ReLabelMetadataDefinitionDescriptionCommand(updated.Identity, new MetadataDefinitionDescription(updated.Description.Trim())));
        }

        private static void GenerateChangeDataTypeCommand(ICommandDispatcher dispatcher,MetadataDefinitionResource original, MetadataDefinitionResource updated)
        {
            if (original.DataTypeMatches(updated))
                return;

            dispatcher.Dispatch(new ChangeMetadataDefinitionDataTypeCommand(updated.Identity, updated.DataType));
        }

        private static void GenerateUpdateRegexCommand(ICommandDispatcher dispatcher,MetadataDefinitionResource original, MetadataDefinitionResource updated)
        {
            if (original.RegexMatches(updated))
                return;

            dispatcher.Dispatch(new UpdateMetadataDefinitionRegexCommand(updated.Identity,updated.Regex));
        }

        private static void GenerateValues(ICommandDispatcher dispatcher, MetadataDefinitionResource original, MetadataDefinitionResource updated)
        {
            if (original.ValuesMatch(updated))
                return;

            dispatcher.Dispatch(new UpdateMetadataDefinitionValuesCommand(updated.Identity, updated.Values));
        }
    }
}