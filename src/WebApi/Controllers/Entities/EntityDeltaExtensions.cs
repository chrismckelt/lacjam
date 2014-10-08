using Lacjam.Core.Domain.Entities;
using Lacjam.Core.Domain.Entities.Commands;
using Lacjam.Core.Domain.Entities.Events;
using Lacjam.Framework.Dispatchers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lacjam.WebApi.Controllers.Entities
{
    public static class EntityDeltaExtensions
    {
        public static void GenerateCommands(this ICommandDispatcher dispatcher, IEntityReadService readService, EntityResource resource)
        {
            var current = readService.FindByIdentity(resource.Identity);

            current.Foreach(original =>
            {
                GenerateReLabelCommand(dispatcher, original, resource);
                GenerateChangeGroupCommand(dispatcher, original, resource);
                GenerateValues(dispatcher, original, resource);
            });
        }

        private static void GenerateReLabelCommand(ICommandDispatcher dispatcher, EntityResource original, EntityResource updated)
        {
            if (original.DiscriptionMatches(updated))
                return;

            dispatcher.Dispatch(new ReLabelEntityCommand(updated.Identity, new EntityName(updated.Name.Trim())));
        }

        private static void GenerateChangeGroupCommand(ICommandDispatcher dispatcher, EntityResource original, EntityResource updated)
        {
            if (original.DefinitionGroup != null && Equals(original.DefinitionGroup.Id, updated.DefinitionGroup.Id))
                return;

            dispatcher.Dispatch(new EntityChangedGroupCommand(updated.Identity, updated.DefinitionGroup.Id));
        }

        private static void GenerateValues(ICommandDispatcher dispatcher, EntityResource original, EntityResource updated)
        {
            if (original.ValuesMatch(updated))
                return;

            if (updated.DefinitionValues == null || !updated.DefinitionValues.Any())
            {
                dispatcher.Dispatch(new RemoveAllEntityValuesCommand(updated.Identity));
                return;
            }

            dispatcher.Dispatch(new UpdateEntityValuesCommand(updated.Identity, BuildEntityValues(updated.Identity, updated.DefinitionValues)));
        }

        private static HashSet<EntityValueBag> BuildEntityValues(Guid identity, IEnumerable<EntityMetadataDefintionResource> definitionValues)
        {
            if (definitionValues == null || !definitionValues.Any())
                return new HashSet<EntityValueBag>(); 

            return new HashSet<EntityValueBag>(definitionValues.Select(x => new EntityValueBag(identity, x.Name, x.MetadataDefinitionIdentity, x.DataType, x.Regex, x.Values)));
        }
    }
}